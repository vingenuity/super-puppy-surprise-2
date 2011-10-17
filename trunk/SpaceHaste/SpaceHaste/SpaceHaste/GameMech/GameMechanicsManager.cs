using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using SpaceHaste.Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceHaste.GameMech
{
    public class GameMechanicsManager : GameComponent
    {
        private KeyboardState kState;
        public static List<GameObject> MoveableSceneGameObjectList;
        public GameMechanicsManager(Game g): base(g)
        {
            MoveableSceneGameObjectList = new List<GameObject>();
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

        void RecoverEnergyToNextShip()
        {
            GameObject nextShipToMove = NextShipToMove();
            double energyToRecover = nextShipToMove.NeededEnergy;
            for (int i = 0; i < MoveableSceneGameObjectList.Count; i++)
            {
                MoveableSceneGameObjectList[i].NeededEnergy -= energyToRecover;           
            }
            if (nextShipToMove.Team == 0)
                PlayerTurn();
            else
                ComputerTurn();
        }
        void PlayerTurn()
        {
            if (kState.IsKeyDown(Keys.M))
            {
            }
        }
        void ComputerTurn()
        { 
        }
    }
}
