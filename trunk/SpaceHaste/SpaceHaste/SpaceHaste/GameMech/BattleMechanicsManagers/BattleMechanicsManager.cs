using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using SpaceHaste.GameObjects;
using SpaceHaste.Primitives;
using SpaceHaste.Maps;
using SpaceHaste.Sounds;
using GameStateManagement;
using SpaceHaste.Controls;

namespace SpaceHaste.GameMech.BattleMechanicsManagers
{
    
    public class BattleMechanicsManager
    {
        public KeyboardState kState;
        public static BattleMechanicsManager Instance;

        bool enabled = true;

        //For selection and display thereof
        Vector3 CurrentGridCubeSelected;
        public GameObject CurrentGameObjectSelected;

        Line YSelectedSquareLine;
        Line ZSelectedSquareLine;
        Line XSelectedSquareLine;
        private AI Enemy;

        List<Line> AttackLineList;
        public bool MoveEnabled;
        public bool WaitEnabled;
        public bool AttackEnabled;

        
        public ShipSelectionMode ShipModeSelection;

        public BattleMechanicsManager()
        {
            Instance = this;
            AttackLineList = new List<Line>();
            Enemy = new AI(Map.map);
        }
        void SortGameObjectList()
        {
            List<GameObject> list = new List<GameObject>();
            while (GameMechanicsManager.GameObjectList.Count > 0)
            {
                GameObject Greatest = GameMechanicsManager.GameObjectList[0];
                for (int i = 0; i < GameMechanicsManager.GameObjectList.Count; i++)
                {

                    if (GameMechanicsManager.GameObjectList[i] > Greatest)
                        Greatest = GameMechanicsManager.GameObjectList[i];
                }
                list.Add(Greatest);
                GameMechanicsManager.GameObjectList.Remove(Greatest);
            }
            GameMechanicsManager.GameObjectList = list;
        }

        private void AddEnergyToShips(GameObject nextShipToMove)
        {
            if (nextShipToMove.energy[0] < 100)
            {
                double energyAdded = 100 - nextShipToMove.energy[0];
                for (int i = 0; i < GameMechanicsManager.GameObjectList.Count; i++)
                {
                    GameMechanicsManager.GameObjectList[i].AddEnergy(energyAdded);
                }
            }
        }

        private void NextShipTurn()
        {
            CheckVictory();
            SortGameObjectList();

            GameObject nextShipToMove = GameMechanicsManager.GameObjectList[0];
            AddEnergyToShips(nextShipToMove);

            CurrentGameObjectSelected = nextShipToMove;
            CurrentGridCubeSelected = nextShipToMove.GridPosition;

            MoveEnabled = true;
            WaitEnabled = true;
            AttackEnabled = true;
            UpdateSelectionLine();
            Tuple<GridCube, ShipSelectionMode> action;
            if (nextShipToMove.team == GameObject.Team.Player)
                NextShipAction();
            else
            {
                action = Enemy.TakeTurn(GameMechanicsManager.GameObjectList);
                while (action.Item2 != ShipSelectionMode.Wait)
                {
                    CurrentGridCubeSelected = action.Item1.AsVector();
                    ShipModeSelection = action.Item2;
                    GameMechanicsManager.gamestate = GameState.EnterShipAction;
                    Selection();
                    action = Enemy.TakeTurn(GameMechanicsManager.GameObjectList);
                }
                SelectionWait();
            }
        }       
        private void NextShipAction()
        {

            GameMechanicsManager.gamestate = GameState.SelectShipAction;
            ScrollDownInUnitListIfActionIsDisabled();
            GameObject nextShipToMove = GameMechanicsManager.GameObjectList[0];

            double energy = nextShipToMove.energy[0];

            if (energy - nextShipToMove.MovementEnergyCost < 0)
                MoveEnabled = false;

            //if (energy - nextShipToMove.AttackEnergyCost < 0)
            //    AttackEnabled = false;

            UpdateSelectionLine();
        }
       
        private void ResetActionSelectionMenu()
        {
            ClearLineList();
            GameObject nextShipToMove = GameMechanicsManager.GameObjectList[0];
            CurrentGridCubeSelected = nextShipToMove.GridPosition;
            double energy = nextShipToMove.energy[0];
            if (energy - nextShipToMove.MovementEnergyCost < 0)
                MoveEnabled = false;

            //if (energy - nextShipToMove.AttackEnergyCost < 0)
            //    AttackEnabled = false;
            ScrollDownInUnitListIfActionIsDisabled();
            UpdateSelectionLine();
        }
        private void ClearLineList()
        {
            for (int i = 0; i < AttackLineList.Count; i++)
            {
                LineManager.RemoveLine(AttackLineList[i]);
            }
            AttackLineList.Clear();
        }
        private void ScrollDownInUnitListIfActionIsDisabled()
        {
            if (ShipModeSelection == ShipSelectionMode.Movement && !MoveEnabled)
                ScrollDownInUnitActionList();
            if (ShipModeSelection == ShipSelectionMode.Attack && !AttackEnabled)
                ScrollDownInUnitActionList();
        }

        void SelectionAttack()
        {
            GameObject offender = CurrentGameObjectSelected;
            GameObject tempTarget = Map.map.GetCubeAt(CurrentGridCubeSelected).GetObject();

            if (tempTarget == null || !(tempTarget is GameObject) || tempTarget.getTeam() == offender.getTeam())
                return;
            GameObject target = tempTarget as GameObject;

            if (offender.energy[0] < offender.AttackEnergyCost)
            {
                AttackEnabled = false;
                NextShipAction();
            }

            if (Map.map.IsObjectInRange(offender, target))
            {
                SoundManager.Sounds.PlaySound(SoundEffects.laser);
                Line line = new Line(offender.DrawPosition, target.DrawPosition, Color.Aqua);
                AttackLineList.Add(line);
                LineManager.AddLine(line);
                target.isHit(offender.GetLaserDamage(target));
                offender.energy[0] -= offender.AttackEnergyCost;
                if (offender.energy[0] < 0)
                    offender.energy[0] = 0;
                if (offender.energy[0] < offender.AttackEnergyCost)
                    AttackEnabled = false;
                NextShipAction();
            }
                               
            else return; 

        }
        void SelectionMissile() 
        {
            GameObject offender = CurrentGameObjectSelected;
            GameObject tempTarget = Map.map.GetCubeAt(CurrentGridCubeSelected).GetObject();

            if (tempTarget == null || !(tempTarget is GameObject) || tempTarget.getTeam() == offender.getTeam())
                return;
            GameObject target = tempTarget as GameObject;

            if (offender.MissileCount <= 0)
            {
                AttackEnabled = false;
                NextShipAction();
            }

            if (Map.map.IsTargetCubeInRange(offender.GridLocation, tempTarget.GridLocation))
            {
                //play missile sound
                target.isHit(offender.dmg[1]);
                offender.MissileCount--;
                AttackEnabled = false;
                NextShipAction();
            }
            else return;

        }
        void SelectionMovement()
        {
            List<GridCube> InRange = Map.map.GetGridCubesInRange(CurrentGameObjectSelected.GridPosition, CurrentGameObjectSelected.MovementRange);
            if (InRange.Find(item => item == Map.map.GetCubeAt(CurrentGridCubeSelected)) != null)
            {
                GridCube c = Map.map.GetCubeAt(CurrentGridCubeSelected);
                Vector3 Distance = CurrentGameObjectSelected.GridPosition - CurrentGridCubeSelected;
                //OLD DISTANCE MOVING
                //float DistanceMoved = Math.Abs(Distance.X) + Math.Abs(Distance.Y) + Math.Abs(Distance.Z);
                float DistanceMoved = Map.map.GetCubeAt(CurrentGridCubeSelected).GetPath().Count;
                if (CurrentGameObjectSelected.energy[0] - DistanceMoved * CurrentGameObjectSelected.MovementEnergyCost >= 0)
                {
                    Map.map.MoveObject(CurrentGameObjectSelected, (int)CurrentGridCubeSelected.X, (int)CurrentGridCubeSelected.Y, (int)CurrentGridCubeSelected.Z);

                    CurrentGameObjectSelected.energy[0] -= DistanceMoved * CurrentGameObjectSelected.MovementEnergyCost;
                    if (CurrentGameObjectSelected.energy[0] - CurrentGameObjectSelected.MovementEnergyCost < 0)
                        MoveEnabled = false;
                    NextShipAction();
                    //CurrentGameObjectSelected.
                   // GameMechanicsManager.gamestate = GameState.MovingShipAnimation;
                }
            }
            CurrentGameObjectSelected.updateBoundingSphere();
        }
        void SelectionWait()
        {
            ClearLineList();
            if (CurrentGameObjectSelected.energy[0] == 100)
                CurrentGameObjectSelected.energy[0] -= 5;
            CurrentGameObjectSelected.waitTime += 40;
            if (CurrentGameObjectSelected.energy[0] < 0)
                CurrentGameObjectSelected.energy[0] = 0;
            NextShipTurn();
        }

        void UpdateSelectionLine()
        {
            UpdateYSlectionLine();
            UpdateZSlectionLine();
            UpdateXSelectionLine();
        }

        private void UpdateXSelectionLine()
        {
            if (XSelectedSquareLine != null)
            {
                LineManager.RemoveLine(XSelectedSquareLine);
            }
            Vector3 botCube = CurrentGridCubeSelected;
            if (!MapManager.isDrawingXGridBottom)
            {
                botCube.X = 0;
                botCube = Map.map.GetCubeAt(botCube).Center;
                botCube.X -= 400;
            }
            else
            {
                botCube.X = Map.map.Size - 1;
                botCube = Map.map.GetCubeAt(botCube).Center;
                botCube.X += +400;
            }
            XSelectedSquareLine = new Line(Map.map.GetCubeAt(CurrentGridCubeSelected).Center, botCube);
            LineManager.AddLine(XSelectedSquareLine);
        }
        
        private void UpdateZSlectionLine()
        {
            if (ZSelectedSquareLine != null)
            {
                LineManager.RemoveLine(ZSelectedSquareLine);
            }
            Vector3 botCube = CurrentGridCubeSelected;
            if (MapManager.isDrawingZGridBottom)
            {
                botCube.Z = 0;
                botCube = Map.map.GetCubeAt(botCube).Center;
                botCube.Z -= 400;
            }
            else
            {
                botCube.Z = Map.map.Size - 1;
                botCube = Map.map.GetCubeAt(botCube).Center;
                botCube.Z += +400;
            }
            ZSelectedSquareLine = new Line(Map.map.GetCubeAt(CurrentGridCubeSelected).Center, botCube);
            LineManager.AddLine(ZSelectedSquareLine);
        }

        private void UpdateYSlectionLine()
        {
            if (YSelectedSquareLine != null)
            {
                LineManager.RemoveLine(YSelectedSquareLine);
            }
            Vector3 botCube = CurrentGridCubeSelected;
            botCube.Y = 0;
            botCube = Map.map.GetCubeAt(botCube).Center;
            botCube.Y -= +400;
            YSelectedSquareLine = new Line(Map.map.GetCubeAt(CurrentGridCubeSelected).Center, botCube);
            LineManager.AddLine(YSelectedSquareLine);
        }

        void CheckVictory()
        {
            if (!enabled)
                return;
            bool PlayerFound = false;
            bool EnemyFound = false;
            foreach(GameObject obj in GameMechanicsManager.GameObjectList)
            {
                if (obj.getTeam() == GameObject.Team.Player)
                    PlayerFound = true;
                else
                    EnemyFound = true;
            }
            if (PlayerFound && EnemyFound)
                return;
            string VictoryDefeatScreenText = "";
            if (!PlayerFound)
                VictoryDefeatScreenText = "You have been Destroyed";
            else if (!EnemyFound)
                VictoryDefeatScreenText = "Enemy Destroyed";
            if (Game1.USEMENUS)
            {
                enabled = false;
                Game1.game.ScreenManager.AddScreen(new VictoryDefeatScreen(VictoryDefeatScreenText, ""), PlayerIndex.One);
            }
            else
                Game1.game.LoadGameComponents();
        }
        double timer = 0;
        List<Vector3> ListOfMovementSquares = new List<Vector3>();
        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (GameMechanicsManager.gamestate == GameState.StartBattle)
            {
                QuadManager.AddQuad(new Quad(Vector3.Zero, Vector3.Left, Vector3.Up, 400, 400));
                GameMechanicsManager.gamestate = GameState.EnterShipAction;
                NextShipTurn();

            }
            if(GameMechanicsManager.gamestate == GameState.EnterShipAction ||  GameMechanicsManager.gamestate == GameState.SelectShipAction)
                UpdateSelectionLine();
            if (GameMechanicsManager.gamestate == GameState.AttackingAnimation)
            {

            }
            if (GameMechanicsManager.gamestate == GameState.MovingShipAnimation)
            {
                while (ListOfMovementSquares.Count > 0)
                {
                    if (timer < 1)
                    {

                    }
                    else
                    {
                        timer = 0;
                        ListOfMovementSquares.RemoveAt(0);
                    }
                }
                if(ListOfMovementSquares.Count == 0)
                    NextShipAction();
            }
        }
        #region Control Delegates
        int i = 0;
        internal void ChangeCameraFocus()
        {
            if (i == 0)
            {
                ControlManager.camera.ChangeToFocusedPosition(Map.map.GetCubeAt(CurrentGridCubeSelected).Center);
                i++;
            }
            else
            {
                ControlManager.camera.ChangeToFocusCenter();
                i = 0;
            }
        }

        /// <summary>
        /// The following functions all return void and take no arguments.
        /// During the instantiation of the class, these are tied to keys and used to perform actions.
        /// </summary>
        internal void Selection()
        {
            if (GameMechanicsManager.gamestate == GameState.SelectShipAction)
            {
                ShipModeSelection = (ShipSelectionMode)(((int)ShipModeSelection) % 3);
                GameMechanicsManager.gamestate = GameState.EnterShipAction;
                if (ShipModeSelection == ShipSelectionMode.Wait)
                    SelectionWait();
                return;
            }
            if (GameMechanicsManager.gamestate == GameState.EnterShipAction)
                switch (ShipModeSelection)
                {
                    case (ShipSelectionMode.Attack):
                        SelectionAttack();
                        return;

                    case (ShipSelectionMode.Movement):
                        SelectionMovement();
                        return;

                    case (ShipSelectionMode.Wait):
                        SelectionWait();
                        return;
                }
        }
        internal void Back()
        {
            if (GameMechanicsManager.gamestate == GameState.EnterShipAction)
            {
                GameMechanicsManager.gamestate = GameState.SelectShipAction;
                ResetActionSelectionMenu();
            }
        }
        internal void MoveSelectionUp()
        {
            if (GameMechanicsManager.gamestate == GameState.EnterShipAction)
            {
                if (CurrentGridCubeSelected.X < Map.map.Size - 1) CurrentGridCubeSelected.X++;
                UpdateSelectionLine();
            }
            if (GameMechanicsManager.gamestate == GameState.SelectShipAction)
            {
                ScrollUpInUnitActionsList();
            }
        }
        internal void MoveSelectionDown()
        {
            if (GameMechanicsManager.gamestate == GameState.EnterShipAction)
            {
                if (CurrentGridCubeSelected.X > 0) CurrentGridCubeSelected.X--;
                UpdateSelectionLine();
            }
            if (GameMechanicsManager.gamestate == GameState.SelectShipAction)
            {
                ScrollDownInUnitActionList();
            }
        }
        internal void MoveSelectionLeft()
        {
            if (GameMechanicsManager.gamestate == GameState.EnterShipAction)
            {
                if (CurrentGridCubeSelected.Z > 0) CurrentGridCubeSelected.Z--;
                UpdateSelectionLine();
            }
        }
        internal void MoveSelectionRight()
        {
            if (GameMechanicsManager.gamestate == GameState.EnterShipAction)
            {
                if (CurrentGridCubeSelected.Z < Map.map.Size - 1) CurrentGridCubeSelected.Z++;
                UpdateSelectionLine();
            }
        }
        internal void MoveSelectionHigher()
        {
            if (GameMechanicsManager.gamestate == GameState.EnterShipAction)
            {
                if (CurrentGridCubeSelected.Y < Map.map.Size - 1) CurrentGridCubeSelected.Y++;
                UpdateSelectionLine();
            }
            
        }
        internal void MoveSelectionLower()
        {
            if (GameMechanicsManager.gamestate == GameState.EnterShipAction)
            {
                if (CurrentGridCubeSelected.Y > 0) CurrentGridCubeSelected.Y--;
                UpdateSelectionLine();
            }
           
        }
        private void ScrollUpInUnitActionsList()
        {
            int i = (int)ShipModeSelection - 1;
            if (i < 0)
                i += 3;
            i = i % 3;
            if (i == 0 && MoveEnabled == false)
                i++;
            if (i == 1 && AttackEnabled == false)
                i++;
            ShipModeSelection = (ShipSelectionMode)(i % 3);

        }
        private void ScrollDownInUnitActionList()
        {
            int i = (int)ShipModeSelection + 1;
            i = i % 3;
            if (i == 1 && AttackEnabled == false)
                i--;
            if (i == 0 && MoveEnabled == false)
                i--;
            if (i < 0)
                i += 3;
            i = i % 3;
            ShipModeSelection = (ShipSelectionMode)(i);
        }
    #endregion
    }
}
