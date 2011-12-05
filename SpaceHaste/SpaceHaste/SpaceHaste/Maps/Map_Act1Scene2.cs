using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;
using SpaceHaste.DPSFParticles;

namespace SpaceHaste.Maps
{
    class Map_Act1Scene2 : Map
    {
        public Map_Act1Scene2()
            : base(new Vector3(6, 6, 12))
        {
            Act = 1;
            Scene = 2;
        }
        protected override void InitMapGameObjects()
        {
            this.addGameObject(new AttackShip("Ceruleo", new Vector3(2, 2, 10), GameObject.Team.Player), new Vector3(2, 2, 10));
            this.addGameObject(new HeavyShip("Reubber", new Vector3(4, 2, 10), GameObject.Team.Player), new Vector3(4, 2, 10));
            this.addGameObject(new LightShip("Viridis", new Vector3(2, 2, 6), GameObject.Team.Player), new Vector3(2, 2, 6));

            this.addGameObject(new LightShip("Rebel Ship 1", new Vector3(2, 2, 3), GameObject.Team.Enemy), new Vector3(2, 2, 3));
            this.addGameObject(new LightShip("Rebel Ship 2", new Vector3(4, 2, 3), GameObject.Team.Enemy), new Vector3(4, 2, 3));

            int z = 9;

            for (int x = 1; x < 5; x++)
                for (int y = 1; y < 5; y++)
            //        for (int z = 8; z < 11; z++)
                        this.AddEnvObject(GridCube.TerrainType.nebula, x, y, z);
        }
    }
}
