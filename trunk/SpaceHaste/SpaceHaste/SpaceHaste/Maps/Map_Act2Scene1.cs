using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;
using SpaceHaste.DPSFParticles;

namespace SpaceHaste.Maps
{
    class Map_Act2Scene1 : Map
    {
        public Map_Act2Scene1()
            : base(new Vector3(6, 6, 12))
        {
            Act = 1;
            Scene = 2;
        }
        protected override void InitMapGameObjects()
        {
            this.addGameObject(new AttackShip("Ceruleo", new Vector3(2, 2, 10), GameObject.Team.Player), new Vector3(2, 2, 10));
            this.addGameObject(new HeavyShip("Reubber", new Vector3(4, 2, 10), GameObject.Team.Player), new Vector3(4, 2, 10));
            this.addGameObject(new LightShip("Midori", new Vector3(2, 2, 11), GameObject.Team.Player), new Vector3(2, 2, 6));
            this.addGameObject(new LightShip("Groc", new Vector3(3, 4, 11), GameObject.Team.Player), new Vector3(3, 4, 7));

            this.addGameObject(new AttackShip("Rebel Boss", new Vector3(6, 2, 3), GameObject.Team.Enemy), new Vector3(2, 2, 3));
            this.addGameObject(new LightShip("Rebel Joe", new Vector3(4, 2, 3), GameObject.Team.Enemy), new Vector3(4, 2, 3));
            this.addGameObject(new LightShip("Rebel Howe", new Vector3(5, 2, 2), GameObject.Team.Enemy), new Vector3(5, 2, 2));
            this.addGameObject(new LightShip("Rebel Rick", new Vector3(5, 3, 3), GameObject.Team.Enemy), new Vector3(5, 3, 3));

            int z = 9;
            /*
            for (int i = 0; i <= 1; i++)
                for (int j = 0; j <= 1; i++)
                {
                    this.AddEnvObject(GridCube.TerrainType.asteroid, i + 6, j, 4);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, 4, j + 5, i);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, i, j + 3, 0);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, 7, j, i);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, i + 5, j + 2, 4);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, 7, j + 4, i + 1);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, i + 4, 0, j + 2);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, i + 7, 0, j);

                }*/
        }
    }
}
