using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceHaste.GameObjects;
using SpaceHaste.Maps;
using SpaceHaste.Primitives;
using SpaceHaste.Huds;
using SpaceHaste.Sounds;

namespace SpaceHaste.GameMech
{
    public class GameMechanicsManager : GameComponent
    {
        public enum ShipSelectionMode
        {
            Movement,
            Attack,
            Wait,
        };

        public enum GameState
        {
            Other,
            SelectShipAction,
            EnterShipAction,
        };
        List<Line> AttackLineList;
        public bool MoveEnabled;
        public bool WaitEnabled;
        public bool AttackEnabled;

        public GameState gamestate;
        public ShipSelectionMode ShipModeSelection;
        //For controls, we need a singleton
        public static GameMechanicsManager MechMan;

        //For selection and display thereof
        Vector3 CurrentGridCubeSelected;
        GameObject CurrentGameObjectSelected;
        Line YSelectedSquareLine;

        public static List<GameObject> GameObjectList;

        Del update;

        public GameMechanicsManager(Game g): base(g)
        {
            MechMan = this;
            GameObjectList = new List<GameObject>();
            AttackLineList = new List<Line>();
            update = EmptyMethod;
        }

        delegate void Del(GameTime gameTime);
        void EmptyMethod(GameTime gameTime)
        {
            NextShipTurn();
        }
        void SortGameObjectList()
        {
            List<GameObject> list = new List<GameObject>();
            while (GameObjectList.Count > 0)
            {
                GameObject Greatest = GameObjectList[0];
                for (int i = 0; i < GameObjectList.Count; i++)
                {

                    if (GameObjectList[i] > Greatest)
                        Greatest = GameObjectList[i];
                }
                list.Add(Greatest);
                GameObjectList.Remove(Greatest);
            }
            GameObjectList = list;
        }
        void NextShipTurn()
        {
            SortGameObjectList();
           // GameObjectList.Sort();
            GameObject nextShipToMove = GameObjectList[0];
            AddEnergyToShips(nextShipToMove);
        
            MoveEnabled = true;
            WaitEnabled = true;
            AttackEnabled = true;
            UpdateSelectionLine();
            NextShipAction();
        }

        private void AddEnergyToShips(GameObject nextShipToMove)
        {
            if (nextShipToMove.Energy == 0 && nextShipToMove.waitTime == 0)
            {
                double energyAdded = nextShipToMove.Energy + nextShipToMove.waitTime;
                for (int i = 0; i < GameObjectList.Count; i++)
                {
                    GameObjectList[i].AddEnergy(energyAdded);
                }
            }
        }
        private void NextShipAction()
        {
            ClearLineList();
            gamestate = GameState.SelectShipAction;
            ScrollDownInUnitListIfActionIsDisabled();
            GameObject nextShipToMove = GameObjectList[0];

            double energy = nextShipToMove.Energy;

            if (energy - nextShipToMove.MovementEnergyCost < 0)
                MoveEnabled = false;

            if (energy - nextShipToMove.AttackEnergyCost < 0)
                AttackEnabled = false;

            if (nextShipToMove.team == GameObject.Team.Player)
            {
                PlayerTurn(nextShipToMove);
            }
            else
                ComputerTurn();
            UpdateSelectionLine();
        }
        private void ResetActionSelectionMenu()
        {
            ClearLineList();
            GameObject nextShipToMove = GameObjectList[0];
            CurrentGridCubeSelected = nextShipToMove.GridPosition;
            double energy = nextShipToMove.Energy;
            if (energy - nextShipToMove.MovementEnergyCost < 0)
                MoveEnabled = false;

            if (energy - nextShipToMove.AttackEnergyCost < 0)
                AttackEnabled = false;
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
        private void PlayerTurn(GameObject nextShipToMove)
        {
            update = PlayerSelectShipAction;
            CurrentGameObjectSelected = nextShipToMove;
            CurrentGridCubeSelected = nextShipToMove.GridPosition;
        }      
        
        void PlayerSelectShipAction(GameTime gameTime)
        {
            
        }
        void PlayerTurn(GameTime gameTime)
        {
        }
        void ComputerTurn()
        {
        }

        void SelectionMovement()
        {
            List<GridCube> InRange = Map.map.GetGridCubesInRange(CurrentGameObjectSelected.GridPosition, CurrentGameObjectSelected.MovementRange);
            if (InRange.Find(item => item == Map.map.GetCubeAt(CurrentGridCubeSelected)) != null)
            {

                Vector3 Distance = CurrentGameObjectSelected.GridPosition - CurrentGridCubeSelected;
                float DistanceMoved = Math.Abs(Distance.X) + Math.Abs(Distance.Y) + Math.Abs(Distance.Z);
                if (CurrentGameObjectSelected.Energy - DistanceMoved * CurrentGameObjectSelected.MovementEnergyCost> 0)
                {
                    Map.map.MoveObject(CurrentGameObjectSelected, (int)CurrentGridCubeSelected.X, (int)CurrentGridCubeSelected.Y, (int)CurrentGridCubeSelected.Z);

                    CurrentGameObjectSelected.Energy -= DistanceMoved * CurrentGameObjectSelected.MovementEnergyCost;
                    if (CurrentGameObjectSelected.Energy - CurrentGameObjectSelected.MovementEnergyCost < 0)
                        MoveEnabled = false;
                    NextShipAction();
                }
            }
        }

        void SelectionWait()
        {
            //CurrentGameObjectSelected.Energy -= 40;
            CurrentGameObjectSelected.waitTime = 40;
            NextShipTurn();
        }
        void SelectionAttack()
        {
            GameObject offender = CurrentGameObjectSelected;
            GameObject tempTarget = Map.map.GetCubeAt(CurrentGridCubeSelected).GetObject();

            if (tempTarget == null || !(tempTarget is Ship) || tempTarget.getTeam() == offender.getTeam())
                return;
            Ship target = tempTarget as Ship;

            if (offender.Energy - offender.AttackEnergyCost < 0)
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
                target.isHit(offender.LaserDamage);
                offender.Energy -= offender.AttackEnergyCost;
                if (offender.Energy < 0)
                    offender.Energy = 0;
            }
            else return;
        }

        void NextTurn()
        {
            
            CheckVictory();
            GameObjectList.Sort();
        }

        void UpdateSelectionLine()
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
            bool PlayerFound = false;
            bool EnemyFound = false;
            foreach(GameObject obj in GameObjectList)
            {
                if (obj.getTeam() == GameObject.Team.Player)
                    PlayerFound = true;
                else
                    EnemyFound = true;
            }
            if (!PlayerFound)
                ; //Call Game Over
            if (!EnemyFound)
                ; //Call Victory
        }

        public override void Update(GameTime gameTime)
        {
            update(gameTime);
            base.Update(gameTime);
        }
        #region Control Delegates
        /// <summary>
        /// The following functions all return void and take no arguments.
        /// During the instantiation of the class, these are tied to keys and used to perform actions.
        /// </summary>
        internal void Selection()
        {
            if (gamestate == GameState.SelectShipAction)
            {
                ShipModeSelection = (ShipSelectionMode)(((int)ShipModeSelection) % 3);
                gamestate = GameState.EnterShipAction;
                if (ShipModeSelection == ShipSelectionMode.Wait)
                    SelectionWait();
                return;
            }
            if (gamestate == GameState.EnterShipAction)
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
            if (gamestate == GameState.EnterShipAction)
            {
                gamestate = GameState.SelectShipAction;
                ResetActionSelectionMenu();
            }
        }
        internal void MoveSelectionUp()
        {
            if (gamestate == GameState.EnterShipAction)
            {
                if (CurrentGridCubeSelected.X < Map.map.Size - 1) CurrentGridCubeSelected.X++;
                UpdateSelectionLine();
            }
            if (gamestate == GameState.SelectShipAction)
            {
                ScrollUpInUnitActionsList();
            }
        }
        internal void MoveSelectionDown()
        {
            if (gamestate == GameState.EnterShipAction)
            {
                if (CurrentGridCubeSelected.X > 0) CurrentGridCubeSelected.X--;
                UpdateSelectionLine();
            }
            if (gamestate == GameState.SelectShipAction)
            {
                ScrollDownInUnitActionList();
            }
        }
        internal void MoveSelectionLeft()
        {
            if (gamestate == GameState.EnterShipAction)
            {
                if (CurrentGridCubeSelected.Z > 0) CurrentGridCubeSelected.Z--;
                UpdateSelectionLine();
            }
        }
        internal void MoveSelectionRight()
        {
            if (gamestate == GameState.EnterShipAction)
            {
                if (CurrentGridCubeSelected.Z < Map.map.Size - 1) CurrentGridCubeSelected.Z++;
                UpdateSelectionLine();
            }
        }
        internal void MoveSelectionHigher()
        {
            if (gamestate == GameState.EnterShipAction)
            {
                if (CurrentGridCubeSelected.Y < Map.map.Size - 1) CurrentGridCubeSelected.Y++;
                UpdateSelectionLine();
            }
            
        }
        internal void MoveSelectionLower()
        {
            if (gamestate == GameState.EnterShipAction)
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
