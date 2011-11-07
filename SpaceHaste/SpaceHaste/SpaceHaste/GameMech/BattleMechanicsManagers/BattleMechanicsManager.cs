﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceHaste.GameObjects;
using SpaceHaste.Primitives;
using SpaceHaste.Maps;
using SpaceHaste.Sounds;
using GameStateManagement;

namespace SpaceHaste.GameMech.BattleMechanicsManagers
{
    
    public class BattleMechanicsManager
    {
        public static BattleMechanicsManager Instance;

        bool enabled = true;

        //For selection and display thereof
        Vector3 CurrentGridCubeSelected;
        GameObject CurrentGameObjectSelected;

        Line YSelectedSquareLine;
        bool loaded = false;
        
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
            if (nextShipToMove.Energy < 100)
            {
                double energyAdded = 100 - nextShipToMove.Energy;
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

            double energy = nextShipToMove.Energy;

            if (energy - nextShipToMove.MovementEnergyCost < 0)
                MoveEnabled = false;

            if (energy - nextShipToMove.AttackEnergyCost < 0)
                AttackEnabled = false;

            UpdateSelectionLine();
        }
       
        private void ResetActionSelectionMenu()
        {
            ClearLineList();
            GameObject nextShipToMove = GameMechanicsManager.GameObjectList[0];
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

        void SelectionAttack()
        {
            GameObject offender = CurrentGameObjectSelected;
            GameObject tempTarget = Map.map.GetCubeAt(CurrentGridCubeSelected).GetObject();

            if (tempTarget == null || !(tempTarget is GameObject) || tempTarget.getTeam() == offender.getTeam())
                return;
            GameObject target = tempTarget as GameObject;

            if (offender.Energy < offender.AttackEnergyCost)
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
                if (offender.Energy < offender.AttackEnergyCost)
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
                if (CurrentGameObjectSelected.Energy - DistanceMoved * CurrentGameObjectSelected.MovementEnergyCost>= 0)
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
            ClearLineList();
            CurrentGameObjectSelected.Energy -= 5;
            CurrentGameObjectSelected.waitTime += 40;
            if (CurrentGameObjectSelected.Energy < 0)
                CurrentGameObjectSelected.Energy = 0;
            NextShipTurn();
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

        public void Update(GameTime gameTime)
        {
            if (!loaded)
            {
                loaded = true;
                NextShipTurn();
            }
        }
        #region Control Delegates
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
