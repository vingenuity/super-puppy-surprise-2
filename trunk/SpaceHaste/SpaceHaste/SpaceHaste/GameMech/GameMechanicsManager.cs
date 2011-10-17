using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;

namespace SpaceHaste.GameMech
{
    public class GameMechanicsManager : GameComponent
    {
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
            for (int i = 0; i < MoveableSceneGameObjectList.Count; i++ )
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
                PlayerControlShip();
            else
                ComputerControlShip();
        }
        void PlayerControlShip()
        {
        }
        void ComputerControlShip()
        { 
        }
    }
}
