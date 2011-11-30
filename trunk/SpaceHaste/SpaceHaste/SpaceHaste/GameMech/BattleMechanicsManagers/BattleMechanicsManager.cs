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
using SpaceHaste.DPSFParticles;

namespace SpaceHaste.GameMech.BattleMechanicsManagers
{
    
    public class BattleMechanicsManager
    {
        public static BattleMechanicsManager Instance;
        public KeyboardState kState;
        public Random random;

        //AI
        private AI Enemy;
        Tuple<GridCube, ShipSelectionMode, ShipAttackSelectionMode> action;

        //Current Modes
        enum CameraMode { OnCenter, OnShip }
        private CameraMode currentCameraMode = CameraMode.OnCenter;
        public ShipSelectionMode ShipModeSelection;
        public ShipAttackSelectionMode ShipAttackModeSelection;

        //For selection and display thereof
        Vector3 CurrentGridCubeSelected;
        public GameObject CurrentGameObjectSelected;
        Line YSelectedSquareLine;
        Line ZSelectedSquareLine;
        Line XSelectedSquareLine;
        List<Line> AttackLineList;

        //Player checks 
        bool enabled = true;
        public bool MoveEnabled;
        public bool WaitEnabled;
        public bool AttackEnabled;
        
        //Particles
        ThrustersParticle ShipThrustersParticle;

        public BattleMechanicsManager()
        {
            random = new Random();
            Instance = this;
            AttackLineList = new List<Line>();
            Enemy = new AI(Map.map);
        }

        public Vector3 getSelectedCube() { return CurrentGridCubeSelected; }

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
            if (!enabled)
            {
                return;
            }
            GameObject nextShipToMove = GameMechanicsManager.GameObjectList[0];
            AddEnergyToShips(nextShipToMove);

            CurrentGameObjectSelected = nextShipToMove;
            CurrentGridCubeSelected = nextShipToMove.GridPosition;

            MoveEnabled = true;
            WaitEnabled = true;
            AttackEnabled = true;
            UpdateSelectionLine();
            NextShipAction();
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

        #region Selection Functions
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
                GameMechanicsManager.gamestate = GameState.MovingShipAnimation;
                
                SoundManager.Sounds.PlaySound(SoundEffects.laser);

                LaserParticle.CreateLaserParticle(offender.DrawPosition, target.DrawPosition);
                /*
                Line line = new Line(offender.DrawPosition, target.DrawPosition, Color.Aqua);
                AttackLineList.Add(line);
                LineManager.AddLine(line);*/
                target.isHit(offender.GetLaserDamage(target));
                offender.energy[0] -= offender.AttackEnergyCost;
                if (offender.energy[0] < 0)
                    offender.energy[0] = 0;
                if (offender.energy[0] < offender.AttackEnergyCost)
                    AttackEnabled = false;
                NextShipAction();
               
                GameMechanicsManager.gamestate = GameState.AttackingLaserAnimation;
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
                //ListOfMissilePath = Map.map.GetCubeAt(CurrentGridCubeSelected).GetPath();
                //play missile sound
                SoundManager.Sounds.PlaySound(SoundEffects.missExpl);
                target.isHit(offender.dmg[1]);
                offender.MissileCount--;
                AttackEnabled = false;
                NextShipAction();
                GameMechanicsManager.gamestate = GameState.AttackingMissileAnimation;
            }
            else return;
        }
        void SelectionTargetLasers()
        {
            GameObject offender = CurrentGameObjectSelected;
            GameObject tempTarget = Map.map.GetCubeAt(CurrentGridCubeSelected).GetObject();

            int percent = random.Next(0, 100);
            if (percent > offender.accuracy) return;

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
                GameMechanicsManager.gamestate = GameState.MovingShipAnimation;

                SoundManager.Sounds.PlaySound(SoundEffects.laser);
                //make a new color
                LaserParticle.CreateLaserParticle(offender.DrawPosition, target.DrawPosition);

                target.LasersDisabled = true;

                NextShipAction();
                GameMechanicsManager.gamestate = GameState.AttackingDisableEngineAnimation;
            }
            else return;
        }
        void SelectionTargetEngines()
        {
            GameObject offender = CurrentGameObjectSelected;
            GameObject tempTarget = Map.map.GetCubeAt(CurrentGridCubeSelected).GetObject();

            int percent = random.Next(0, 100);
            if (percent > offender.accuracy) return;

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
                GameMechanicsManager.gamestate = GameState.MovingShipAnimation;

                SoundManager.Sounds.PlaySound(SoundEffects.laser);
                //make a new color
                LaserParticle.CreateLaserParticle(offender.DrawPosition, target.DrawPosition);

                target.EnginesDisabled = true;

                NextShipAction();
                GameMechanicsManager.gamestate = GameState.AttackingDisableEngineAnimation;
            }
            else return;
        }
        void SelectionMovement()
        {
            List<GridCube> InRange = Map.map.GetGridCubesInRange(CurrentGameObjectSelected.GridPosition, CurrentGameObjectSelected.MovementRange);
            if (InRange.Find(item => item == Map.map.GetCubeAt(CurrentGridCubeSelected)) != null)
            {
                GridCube c = Map.map.GetCubeAt(CurrentGridCubeSelected);
               // Vector3 Distance = CurrentGameObjectSelected.GridPosition - CurrentGridCubeSelected;
                //OLD DISTANCE MOVING
                //float DistanceMoved = Math.Abs(Distance.X) + Math.Abs(Distance.Y) + Math.Abs(Distance.Z);
                float DistanceMoved = Map.map.GetCubeAt(CurrentGridCubeSelected).GetPath().Count;
                if (CurrentGameObjectSelected.energy[0] - DistanceMoved * CurrentGameObjectSelected.MovementEnergyCost >= 0)
                {
                    
                    //NextShipAction();
                    //CurrentGameObjectSelected.
                    ListOfMovementSquares = Map.map.GetCubeAt(CurrentGridCubeSelected).GetPath();
                    
                    ListOfMovementSquares.Add(CurrentGridCubeSelected);
                    timer = 0;
                    GameMechanicsManager.gamestate = GameState.MovingShipAnimation;
                    SoundManager.Sounds.PlaySound(SoundEffects.engines);
                    if (ListOfMovementSquares.Count > 0)
                        ListOfMovementSquares.RemoveAt(0);
                    
                    Vector3 offset = CurrentGameObjectSelected.DrawPosition - Map.map.GetCubeAt(CurrentGridCubeSelected).Center ;
                    offset.Normalize();
                    offset *= GridCube.GRIDSQUARELENGTH * 4 / 5;
                    ShipThrustersParticle = ThrustersParticle.CreateParticle(CurrentGameObjectSelected.DrawPosition, offset);
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
        #endregion

        #region Update Selection Lines
        void UpdateSelectionLine()
        {
            UpdateYSelectionLine();
            UpdateZSelectionLine();
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
                botCube.X = Map.map.Size.X - 1;
                botCube = Map.map.GetCubeAt(botCube).Center;
                botCube.X += +400;
            }
            XSelectedSquareLine = new Line(Map.map.GetCubeAt(CurrentGridCubeSelected).Center, botCube);
            LineManager.AddLine(XSelectedSquareLine);
        }
        private void UpdateYSelectionLine()
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
        private void UpdateZSelectionLine()
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
                botCube.Z = Map.map.Size.Z - 1;
                botCube = Map.map.GetCubeAt(botCube).Center;
                botCube.Z += +400;
            }
            ZSelectedSquareLine = new Line(Map.map.GetCubeAt(CurrentGridCubeSelected).Center, botCube);
            LineManager.AddLine(ZSelectedSquareLine);
        }
        #endregion

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
        Vector3 InterpDistance;

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime.TotalSeconds;
            if (GameMechanicsManager.GameObjectList[0].team == GameObject.Team.Enemy && GameMechanicsManager.gamestate == GameState.SelectShipAction)
            {
                CheckVictory();
                action = Enemy.TakeTurn(GameMechanicsManager.GameObjectList);
                if (action.Item2 == ShipSelectionMode.Wait)
                    SelectionWait();
                else
                {
                    timer = 0;
                    GameMechanicsManager.gamestate = GameState.AITurnAnimation;
                }
            }
            if (GameMechanicsManager.gamestate == GameState.StartBattle)
            {
                QuadManager.AddQuad(new Quad(Vector3.Zero, Vector3.Left, Vector3.Up, 400, 400));
                GameMechanicsManager.gamestate = GameState.EnterShipAction;
               //USE THIS LINE TO TEST PARTICLES DeathParticle.CreateDeathParticle(Vector3.Zero);
                NextShipTurn();

            }
            if(GameMechanicsManager.gamestate == GameState.EnterShipAction ||  GameMechanicsManager.gamestate == GameState.SelectShipAction)
                UpdateSelectionLine();
            if (GameMechanicsManager.gamestate == GameState.AITurnAnimation)
            {
                if (timer > 1 && timer < 2)
                {
                    CurrentGridCubeSelected = action.Item1.AsVector();
                    ShipModeSelection = action.Item2;
                    ShipAttackModeSelection = action.Item3;
                    UpdateSelectionLine();
                }
                else if (timer > 2)
                {
                    timer = 0;
                    if (action.Item2 == ShipSelectionMode.Attack)
                        GameMechanicsManager.gamestate = GameState.SelectShipAttackAction;
                    else
                        GameMechanicsManager.gamestate = GameState.EnterShipAction;
                    Selection();
                }
            }
            if (GameMechanicsManager.gamestate == GameState.AttackingLaserAnimation)
            {
                if (timer > 1)
                {
                     NextShipAction();
                     timer = 0;
                }                
            }
            if (GameMechanicsManager.gamestate == GameState.AttackingMissileAnimation)
            {
                if (timer > 1)
                {
                    NextShipAction();
                    timer = 0;
                }
            }
            if (GameMechanicsManager.gamestate == GameState.MovingShipAnimation)
            {
                if (ListOfMovementSquares.Count > 0)
                {
                    ShipThrustersParticle.Position = CurrentGameObjectSelected.DrawPosition; 
                    GridCube c =  Map.map.GetCubeAt(ListOfMovementSquares[0]);


                    if (timer < 1)
                    {
                          InterpDistance = CurrentGameObjectSelected.GridLocation.Center - c.Center;
                          CurrentGameObjectSelected.DrawPosition = CurrentGameObjectSelected.GridLocation.Center - InterpDistance * (float)(timer / 1.0);
                    }
                    else
                    {
                        Map.map.MoveObject(CurrentGameObjectSelected, (int)c.AsVector().X, (int)c.AsVector().Y, (int)c.AsVector().Z);

                        CurrentGameObjectSelected.energy[0] -= CurrentGameObjectSelected.MovementEnergyCost;
                        
                        
                        ListOfMovementSquares.RemoveAt(0);
                        timer = 0;
                        ParticleManager.Instance.Remove(ShipThrustersParticle);
                    }
                }
                if (ListOfMovementSquares.Count == 0)
                {
                    if (CurrentGameObjectSelected.energy[0] - CurrentGameObjectSelected.MovementEnergyCost < 0)
                        MoveEnabled = false;
                    NextShipAction();
                    ParticleManager.Instance.Remove(ShipThrustersParticle);
                }
            }
        }

        #region Control Delegates
        /// <summary>
        /// The following functions all return void and take no arguments.
        /// During the instantiation of the class, these are tied to keys and used to perform actions.
        /// </summary>
        internal void ChangeCameraMode()
        {
            if (currentCameraMode == CameraMode.OnCenter)
            {
                ControlManager.camera.ChangeToFocusedPosition(Map.map.GetCubeAt(CurrentGridCubeSelected).Center);
                currentCameraMode = CameraMode.OnShip;
            }
            else
            {
                ControlManager.camera.ChangeToFocusCenter();
                currentCameraMode = CameraMode.OnCenter;
            }
        }
        internal void CenterOnShip(int PlusMinus)
        {
            if (currentCameraMode != CameraMode.OnShip)
                return;
            int focusIndex = GameMechanicsManager.GameObjectList.FindIndex(obj => obj.GridLocation.Center == ControlManager.camera.FocusedPosition);
            if (focusIndex == -1)
                return;
            if (focusIndex == 0 && PlusMinus < 0)
                focusIndex = GameMechanicsManager.GameObjectList.Count;
            if (focusIndex == GameMechanicsManager.GameObjectList.Count - 1 && PlusMinus > 0)
                focusIndex = -1;
            ControlManager.camera.ChangeToFocusedPosition(GameMechanicsManager.GameObjectList[focusIndex + PlusMinus].GridLocation.Center);
        }
        internal void CenterOnNextShip()
        {
            CenterOnShip(1);
        }
        internal void CenterOnPrevShip()
        {
            CenterOnShip(-1);
        }
        internal void Selection()
        {
            
            if (GameMechanicsManager.gamestate == GameState.SelectShipAction)
            {
                ShipModeSelection = (ShipSelectionMode)(((int)ShipModeSelection) % 3);
                GameMechanicsManager.gamestate = GameState.EnterShipAction;
                if (ShipModeSelection == ShipSelectionMode.Wait)
                    SelectionWait();
                if (ShipModeSelection == ShipSelectionMode.Attack)
                    GameMechanicsManager.gamestate = GameState.SelectShipAttackAction;
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
            if (GameMechanicsManager.gamestate == GameState.SelectShipAttackAction)
            {
                GameMechanicsManager.gamestate = GameState.EnterShipAction;
                switch (ShipAttackModeSelection)
                {
                    case (ShipAttackSelectionMode.Laser):
                        SelectionAttack();
                        return;
                    case (ShipAttackSelectionMode.Missile):
                        SelectionMissile();
                        return;
                    case (ShipAttackSelectionMode.TargetEngine):
                        SelectionTargetEngines();
                        return;
                    case (ShipAttackSelectionMode.TargetWeapon):
                        SelectionTargetLasers();
                        return;
                }
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
                if (CurrentGridCubeSelected.X < Map.map.Size.X - 1) CurrentGridCubeSelected.X++;
                UpdateSelectionLine();
            }
            if (GameMechanicsManager.gamestate == GameState.SelectShipAction)
            {
                ScrollUpInUnitActionsList();
            }
            if (GameMechanicsManager.gamestate == GameState.SelectShipAttackAction)
            {
                ScrollUpInAttackUnitActionsList();
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
            if (GameMechanicsManager.gamestate == GameState.SelectShipAttackAction)
            {
                ScrollDownInAttackUnitActionsList();
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
                if (CurrentGridCubeSelected.Z < Map.map.Size.X - 1) CurrentGridCubeSelected.Z++;
                UpdateSelectionLine();
            }
        }
        internal void MoveSelectionHigher()
        {
            if (GameMechanicsManager.gamestate == GameState.EnterShipAction)
            {
                if (CurrentGridCubeSelected.Y < Map.map.Size.Y - 1) CurrentGridCubeSelected.Y++;
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

        private void ScrollUpInAttackUnitActionsList()
        {
            int a = ((int)(ShipAttackModeSelection)-1);
            if(a < 0)
                a = 3;
            ShipAttackModeSelection = (ShipAttackSelectionMode)(a % 4);
        }
        private void ScrollDownInAttackUnitActionsList()
        {
            ShipAttackModeSelection = (ShipAttackSelectionMode)(((int)(ShipAttackModeSelection) + 1) % 4);
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
