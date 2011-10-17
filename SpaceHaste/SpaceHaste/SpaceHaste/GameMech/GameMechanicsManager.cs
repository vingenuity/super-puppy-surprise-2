using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;

namespace SpaceHaste.GameMech
{
    public class GameMechanicsManager
    {
        public List<GameObject> MoveableSceneGameObjectList;
        public GameMechanicsManager()
        {
            MoveableSceneGameObjectList = new List<GameObject>();
        }

        public GameObject NextShipToMove()
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

        public void asdf()
        {
        }
    }
}
