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
        KeyboardState kState;
        KeyboardState lastKState;
        public static List<GameObject> MoveableSceneGameObjectList;
        Del update;
        public GameMechanicsManager(Game g): base(g)
        {
            MoveableSceneGameObjectList = new List<GameObject>();
            update = EmptyMethod;
            kState = Keyboard.GetState();
            lastKState = Keyboard.GetState();
            
        }

        GameObject NextShipToMove()
        {
            GameObject go = null;
            if (MoveableSceneGameObjectList.Count > 0)
                go = MoveableSceneGameObjectList[0];
            for (int i = 0; i < MoveableSceneGameObjectList.Count; i++)
            {
                if (go.NeededEnergy > MoveableSceneGameObjectList[i].NeededEnergy)
                    go = MoveableSceneGameObjectList[i];
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
            for (int i = 0; i < MoveableSceneGameObjectList.Count; i++)
            {
                MoveableSceneGameObjectList[i].NeededEnergy -= energyToRecover;           
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


        Vector3 CurrentGridCubeSelected;
        GameObject CurrentGameObjectSelected;

        void PlayerTurn(GameTime gameTime)
        {
            kState = Keyboard.GetState();
            UpdateGridSquareSelection();
            MoveToSelectedGridSquare();
            lastKState = kState;
        }
        Line YShipLine;
        Line YSelectedSquareLine;
        private void MoveToSelectedGridSquare()
        {
            if (kState.IsKeyDown(Keys.Enter) && lastKState.IsKeyUp(Keys.Enter))
            {
                Vector3 pos = (CurrentGridCubeSelected - CurrentGameObjectSelected.GridPosition);
                float r = pos.X + pos.Y + pos.Z;
                if (r <= CurrentGameObjectSelected.MovementRange)
                {
                    Map.map.MoveObject(CurrentGameObjectSelected, (int)CurrentGridCubeSelected.X, (int)CurrentGridCubeSelected.Y, (int)CurrentGridCubeSelected.Z);
                }
            }
        }

        private void UpdateGridSquareSelection()
        {
            
            if (kState.IsKeyDown(Keys.I) && lastKState.IsKeyUp(Keys.I))
            {
                CurrentGridCubeSelected.X++;
                if (CurrentGridCubeSelected.X >= Map.map.Size)
                {
                    CurrentGridCubeSelected.X = Map.map.Size - 1;
                }
            }
            if (kState.IsKeyDown(Keys.K) && lastKState.IsKeyUp(Keys.K))
            {
                CurrentGridCubeSelected.X--;
                if (CurrentGridCubeSelected.X < 0)
                {
                    CurrentGridCubeSelected.X = 0;
                }
            }
            if (kState.IsKeyDown(Keys.J) && lastKState.IsKeyUp(Keys.J))
            {
                CurrentGridCubeSelected.Z++;
                if (CurrentGridCubeSelected.Z >= Map.map.Size)
                {
                    CurrentGridCubeSelected.Z = Map.map.Size - 1;
                }
            }
            if (kState.IsKeyDown(Keys.L) && lastKState.IsKeyUp(Keys.L))
            {
                CurrentGridCubeSelected.Z--;
                if (CurrentGridCubeSelected.Z < 0)
                {
                    CurrentGridCubeSelected.Z = 0;
                }
            }
            if (kState.IsKeyDown(Keys.U) && lastKState.IsKeyUp(Keys.U))
            {
                CurrentGridCubeSelected.Y++;
                if (CurrentGridCubeSelected.Y >= Map.map.Size)
                {
                    CurrentGridCubeSelected.Y = Map.map.Size - 1;
                }
            }
            if (kState.IsKeyDown(Keys.O) && lastKState.IsKeyUp(Keys.O))
            {
                CurrentGridCubeSelected.Y--;
                if (CurrentGridCubeSelected.Y < 0)
                {
                    CurrentGridCubeSelected.Y = 0;
                }
            }
            if(YSelectedSquareLine != null)
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
        void ComputerTurn()
        { 
        }
        public override void Update(GameTime gameTime)
        {
            update(gameTime);
            base.Update(gameTime);
        }
    }
}
