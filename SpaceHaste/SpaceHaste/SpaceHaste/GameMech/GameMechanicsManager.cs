using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using SpaceHaste.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceHaste.Primitives;

namespace SpaceHaste.GameMech
{
    public class GameMechanicsManager : GameComponent
    {
        //For controls
        private delegate void KeyAction();
        private static Dictionary<Microsoft.Xna.Framework.Input.Keys, KeyAction> KeyMap;
        KeyboardState lastKState;

        //For selection and display thereof
        Vector3 CurrentGridCubeSelected;
        GameObject CurrentGameObjectSelected;
        Line YShipLine;
        Line YSelectedSquareLine;

        //For other stuff
        public static List<GameObject> GameObjectList;
        Del update;

        public GameMechanicsManager(Game g): base(g)
        {
            GameObjectList = new List<GameObject>();
            update = EmptyMethod;

            //Initialize Keys
            KeyMap = new Dictionary<Microsoft.Xna.Framework.Input.Keys, KeyAction>();
            KeyMap.Add(Keys.Enter, new KeyAction(Selection));
            KeyMap.Add(Keys.I, new KeyAction(MoveSelectionUp));
            KeyMap.Add(Keys.K, new KeyAction(MoveSelectionDown));
            KeyMap.Add(Keys.J, new KeyAction(MoveSelectionLeft));
            KeyMap.Add(Keys.L, new KeyAction(MoveSelectionRight));
            KeyMap.Add(Keys.O, new KeyAction(MoveSelectionHigher));
            KeyMap.Add(Keys.U, new KeyAction(MoveSelectionLower));
            lastKState = Keyboard.GetState();
        }

        GameObject NextShipToMove()
        {
            GameObject go = null;
            if (GameObjectList.Count > 0)
                go = GameObjectList[0];
            for (int i = 0; i < GameObjectList.Count; i++)
            {
                if (go.NeededEnergy > GameObjectList[i].NeededEnergy)
                    go = GameObjectList[i];
            }
            return go;
        }
        delegate void Del(GameTime gameTime);
        void EmptyMethod(GameTime gameTime)
        {
            RecoverEnergyToNextShip();
        }


        void RecoverEnergyToNextShip()
        {
            GameObject nextShipToMove = NextShipToMove();
            if (nextShipToMove == null)
                return;
            double energyToRecover = nextShipToMove.NeededEnergy;
            for (int i = 0; i < GameObjectList.Count; i++)
            {
                GameObjectList[i].NeededEnergy -= energyToRecover;           
            }
            if (nextShipToMove.Team == 0)
            {
                update = PlayerTurn;
                CurrentGameObjectSelected = nextShipToMove;
                CurrentGridCubeSelected = nextShipToMove.GridPosition;
            }
            else
                ComputerTurn();
        }

        void PlayerTurn(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            foreach (Microsoft.Xna.Framework.Input.Keys key in KeyMap.Keys)
                if (state.IsKeyDown(key) && lastKState.IsKeyUp(key))
                    KeyMap[key]();
            lastKState = state;
        }
        void ComputerTurn()
        {
        }

        /// <summary>
        /// The following functions all return void and take no arguments.
        /// During the instantiation of the class, these are tied to keys and used to perform actions.
        /// </summary>
        private void Selection()
        {
            List<GridCube> InRange = Map.map.GetGridSquaresInRange(CurrentGameObjectSelected.GridPosition, CurrentGameObjectSelected.MovementRange);
            if (InRange.Find(item => item == Map.map.GetCubeAt(CurrentGridCubeSelected)) != null)
            {
                Vector3 Distance = CurrentGameObjectSelected.GridPosition - CurrentGridCubeSelected;
                Map.map.MoveObject(CurrentGameObjectSelected, (int)CurrentGridCubeSelected.X, (int)CurrentGridCubeSelected.Y, (int)CurrentGridCubeSelected.Z);
                float DistanceMoved = Math.Abs(Distance.X) + Math.Abs(Distance.Y) + Math.Abs(Distance.Z);
                CurrentGameObjectSelected.NeededEnergy += DistanceMoved * CurrentGameObjectSelected.MovementEnergy;
                RecoverEnergyToNextShip();
            }
        }
        private void MoveSelectionUp()
        {
            if(CurrentGridCubeSelected.X < Map.map.Size - 1) CurrentGridCubeSelected.X++;
            UpdateSelectionLine();
        }
        private void MoveSelectionDown()
        {
            if (CurrentGridCubeSelected.X > 0) CurrentGridCubeSelected.X--;
            UpdateSelectionLine();
        }
        private void MoveSelectionLeft()
        {
            if (CurrentGridCubeSelected.Z > 0) CurrentGridCubeSelected.Z--;
            UpdateSelectionLine();
        }
        private void MoveSelectionRight()
        {
            if (CurrentGridCubeSelected.Z < Map.map.Size - 1) CurrentGridCubeSelected.Z++;
            UpdateSelectionLine();
        }
        private void MoveSelectionHigher()
        {
            if (CurrentGridCubeSelected.Y < Map.map.Size - 1) CurrentGridCubeSelected.Y++;
            UpdateSelectionLine();
        }
        private void MoveSelectionLower()
        {
            if (CurrentGridCubeSelected.Y > 0) CurrentGridCubeSelected.Y--;
            UpdateSelectionLine();
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
