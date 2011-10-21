using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SpaceHaste.GameObjects;
using SpaceHaste.Maps;
using SpaceHaste.Primitives;

namespace SpaceHaste.GameMech
{
    public class GameMechanicsManager : GameComponent
    {
        //For controls, we need a singleton
        public static GameMechanicsManager MechMan;

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
            MechMan = this;
            GameObjectList = new List<GameObject>();
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
            RecoverEnergyToNextShip();
        }


        void RecoverEnergyToNextShip()
        {
            GameObject nextShipToMove = NextShipToMove();
            if (nextShipToMove == null)
                return;
            double energyToRecover = nextShipToMove.Energy;
            for (int i = 0; i < GameObjectList.Count; i++)
            {
                GameObjectList[i].Energy += energyToRecover;           
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
        }
        void ComputerTurn()
        {
        }

        /// <summary>
        /// The following functions all return void and take no arguments.
        /// During the instantiation of the class, these are tied to keys and used to perform actions.
        /// </summary>
        internal void Selection()
        {
            List<GridCube> InRange = Map.map.GetGridSquaresInRange(CurrentGameObjectSelected.GridPosition, CurrentGameObjectSelected.MovementRange);
            if (InRange.Find(item => item == Map.map.GetCubeAt(CurrentGridCubeSelected)) != null)
            {
                Vector3 Distance = CurrentGameObjectSelected.GridPosition - CurrentGridCubeSelected;
                Map.map.MoveObject(CurrentGameObjectSelected, (int)CurrentGridCubeSelected.X, (int)CurrentGridCubeSelected.Y, (int)CurrentGridCubeSelected.Z);
                float DistanceMoved = Math.Abs(Distance.X) + Math.Abs(Distance.Y) + Math.Abs(Distance.Z);
                CurrentGameObjectSelected.Energy += DistanceMoved * CurrentGameObjectSelected.MovementEnergy;
                RecoverEnergyToNextShip();
            }
        }
        internal void MoveSelectionUp()
        {
            if(CurrentGridCubeSelected.X < Map.map.Size - 1) CurrentGridCubeSelected.X++;
            UpdateSelectionLine();
        }
        internal void MoveSelectionDown()
        {
            if (CurrentGridCubeSelected.X > 0) CurrentGridCubeSelected.X--;
            UpdateSelectionLine();
        }
        internal void MoveSelectionLeft()
        {
            if (CurrentGridCubeSelected.Z > 0) CurrentGridCubeSelected.Z--;
            UpdateSelectionLine();
        }
        internal void MoveSelectionRight()
        {
            if (CurrentGridCubeSelected.Z < Map.map.Size - 1) CurrentGridCubeSelected.Z++;
            UpdateSelectionLine();
        }
        internal void MoveSelectionHigher()
        {
            if (CurrentGridCubeSelected.Y < Map.map.Size - 1) CurrentGridCubeSelected.Y++;
            UpdateSelectionLine();
        }
        internal void MoveSelectionLower()
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
