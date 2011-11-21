using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;

namespace SpaceHaste.Maps
{
    public class Map1 : Map
    {
        public Map1()
            : base(12)
        { }

        protected override void InitMapGameObjects()
        {
            this.addGameObject(new HeavyShip("Player Ship 1", new Vector3(1, 4, 7), GameObject.Team.Player), new Vector3(1, 4, 7));
            this.addGameObject(new HeavyShip("Player Ship 2", new Vector3(4, 3, 7), GameObject.Team.Player), new Vector3(4, 3, 7));
            this.addGameObject(new LightShip("Player Ship 3", new Vector3(7, 4, 7), GameObject.Team.Player), new Vector3(7, 4, 7));

            this.addGameObject(new LightShip("Enemy Ship 1", new Vector3(1, 4, 3), GameObject.Team.Enemy), new Vector3(1, 4, 3));
            this.addGameObject(new HeavyShip("Enemy Ship 2", new Vector3(4, 3, 3), GameObject.Team.Enemy), new Vector3(4, 3, 3));
            //this.addGameObject(createBasicShip("Enemy Ship 3", new Vector3(7, 4, 3), GameObject.Team.Enemy));

            for (int x = 1; x < 9; x++)
            {
                for (int y = 2; y < 6; y++)
                {
                    for (int z = 4; z < 7; z++)
                    {
                      //  this.AddEnvObject(GridCube.TerrainType.wreck, x, y, z);
                    }
                }
            }

            AddGridIsometric();
        }
    }
}
