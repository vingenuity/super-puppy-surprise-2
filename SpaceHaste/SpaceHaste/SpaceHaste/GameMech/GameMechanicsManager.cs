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
        public static List<GameObject> WaitingGameObjectList;
        Del update;

        public GameMechanicsManager(Game g): base(g)
        {
            MechMan = this;
            GameObjectList = new List<GameObject>();
            WaitingGameObjectList = new List<GameObject>();
            update = EmptyMethod;
        }


        GameObject NextShipToMove()
        {
            GameObject go = null;
            if (GameObjectList.Count > 0)
                go = GameObjectList[0];
            for (int i = 0; i < GameObjectList.Count; i++)
            {
                if (go.Energy < GameObjectList[i].Energy)
                    go = GameObjectList[i];
            }
            return go;
        }

        delegate void Del(GameTime gameTime);
        void EmptyMethod(GameTime gameTime)
        {
            NextShipTurn();
        }
        //Rename to something more descriptive
        List<GameObject> ShipTurnOrderList = new List<GameObject>();

        void CreateMovementOrderList()
        {         
            ShipTurnOrderList = CloneGameObjectList();
            ShipTurnOrderList = SortGameObjectsByEnergy(ShipTurnOrderList);
            ShipTurnOrderList = MoveShipsThatWaitedLastTurnToFront(ShipTurnOrderList);

        }

        private List<GameObject> MoveShipsThatWaitedLastTurnToFront(List<GameObject> MovementOrderList)
        {
            int counter = 0;
            while (WaitingGameObjectList.Count > 0)
            {
                AddShipWithMostEnergyToFrontOfList(MovementOrderList, WaitingGameObjectList, counter);
            }
            return MovementOrderList;
        }
       
        private void AddShipWithMostEnergyToFrontOfList(List<GameObject> MovementOrderList, List<GameObject> WaitingGameObjectList, int counter)
        {
            GameObject go = MovementOrderList[0];
            for (int i = 0; i < MovementOrderList.Count; i++)
                if (go.Energy < MovementOrderList[i].Energy)
                    go = MovementOrderList[i];
            MovementOrderList.Remove(go);
            MovementOrderList.Insert(counter, go);
            counter++;
        }

        private List<GameObject> SortGameObjectsByEnergy(List<GameObject> l)
        {
            List<GameObject> list = new List<GameObject>();
            while(l.Count > 0)
                AddFastestShipEnergyToList(l, list);
            return list;
        }

        private void AddFastestShipEnergyToList(List<GameObject> ListFrom, List<GameObject> ListTo)
        {
            GameObject go = ListFrom[0];
            for (int i = 1; i < ListFrom.Count; i++)
            {
                if (go.EnergyEfficiency < ListFrom[i].EnergyEfficiency)
                    go = ListFrom[i];
            }
            ListFrom.Remove(go);
            ListTo.Add(go);
        }

        private List<GameObject> CloneGameObjectList()
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < GameObjectList.Count; i++)
            {
               list.Add(GameObjectList[i]);
            }
            return list;
        }
      
        
        void NextShipTurn()
        {
            if (ShipTurnOrderList.Count == 0 || ShipTurnOrderList == null)
                NextTurn();
            if (ShipTurnOrderList.Count == 0)
                return;
            MoveEnabled = true;
            WaitEnabled = true;
            AttackEnabled = true;
            NextShipAction();
        }
        
        private void NextShipAction()
        {
            gamestate = GameState.SelectShipAction;
            ScrollDownInUnitListIfActionIsDisabled();
            GameObject nextShipToMove = ShipTurnOrderList[0];

            double energy = nextShipToMove.Energy;
 
            if (nextShipToMove.Team == 0)
            {
                PlayerTurn(nextShipToMove);
            }
            else
                ComputerTurn();
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

        /*
        void RecoverEnergyToNextShip()
        {
           
            GameObject nextShipToMove = NextShipToMove();
            if (nextShipToMove == null)
                return;
            double energyToRecover = nextShipToMove.Energy;
            for (int i = 0; i < GameObjectList.Count; i++)
            {
                GameObjectList[i].Energy += energyToRecover;           
         * 
            {
                update = PlayerTurn;
                CurrentGameObjectSelected = nextShipToMove;
                CurrentGridCubeSelected = nextShipToMove.GridPosition;
            }
            else
                ComputerTurn();
        }
        */
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
            List<GridCube> InRange = Map.map.GetGridSquaresInRange(CurrentGameObjectSelected.GridPosition, CurrentGameObjectSelected.MovementRange);
            if (InRange.Find(item => item == Map.map.GetCubeAt(CurrentGridCubeSelected)) != null)
            {
                Vector3 Distance = CurrentGameObjectSelected.GridPosition - CurrentGridCubeSelected;
                Map.map.MoveObject(CurrentGameObjectSelected, (int)CurrentGridCubeSelected.X, (int)CurrentGridCubeSelected.Y, (int)CurrentGridCubeSelected.Z);
                float DistanceMoved = Math.Abs(Distance.X) + Math.Abs(Distance.Y) + Math.Abs(Distance.Z);
                CurrentGameObjectSelected.Energy -= DistanceMoved * CurrentGameObjectSelected.MovementEnergyCost;
                MoveEnabled = false;
                NextShipAction();

            }
        }
        void SelectionWait()
        {
            WaitingGameObjectList.Add(CurrentGameObjectSelected);
            GameObjectList.Remove(CurrentGameObjectSelected);
           
            NextShipAction();
        }



        void SelectionAttack()
        {
            GameObject offender = CurrentGameObjectSelected;
            GameObject tempTarget = Map.map.GetCubeAt(CurrentGridCubeSelected).GetObject();

            if (tempTarget == null || !(tempTarget is Ship))
                return;
            Ship target = tempTarget as Ship;

            if (Map.map.IsObjectInRange(offender, offender.LaserRange, target))
            {
                LineManager.AddLine(new Line(offender.DrawPosition, target.DrawPosition, Color.Aqua));
                target.isHit(offender.LaserDamage);
                offender.Energy -= 10;
                if (offender.Energy < 0)
                    offender.Energy = 0;
            }
            else return;
        }
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
               if(ShipModeSelection == ShipSelectionMode.Wait)
                   SelectionWait();
                return;
            }
            if(gamestate == GameState.EnterShipAction)
                switch (ShipModeSelection)
                {
                    case(ShipSelectionMode.Attack):
                        SelectionAttack();
                        return;
                  
                    case (ShipSelectionMode.Movement):
                        SelectionMovement();
                        return;
                  
                    case(ShipSelectionMode.Wait):
                        SelectionWait();
                        return;
                }
        }

        void NextTurn()
        {
            CreateMovementOrderList();
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

        public override void Update(GameTime gameTime)
        {
            update(gameTime);
            base.Update(gameTime);
        }

      
    }
}
