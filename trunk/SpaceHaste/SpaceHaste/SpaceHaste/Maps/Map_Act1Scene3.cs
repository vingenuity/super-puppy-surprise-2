using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;
using SpaceHaste.DPSFParticles;

namespace SpaceHaste.Maps
{
    class Map_Act1Scene3 : Map
    {
        public Map_Act1Scene3()
            : base(new Vector3(5, 5, 11))
        {
            Act = 1;
            Scene = 3;
        }
        protected override void InitMapGameObjects()
        {
            this.addGameObject(new LightShip("Ceruleo", new Vector3(2, 2, 9), GameObject.Team.Player), new Vector3(2, 2, 9));
            this.addGameObject(new MissileShip ("Reubber", new Vector3(3, 2, 9), GameObject.Team.Player), new Vector3(3, 2, 9));
            this.addGameObject(new AttackShip  ("Viridis", new Vector3(3, 2, 8), GameObject.Team.Player), new Vector3(3, 2, 8));
            this.addGameObject(new StandardShip("RED ONE", new Vector3(2, 2, 8), GameObject.Team.Player), new Vector3(2, 2, 8));

            HeavyShip Rebel1 = new HeavyShip("Rebel Pilot", new Vector3(2, 2, 3), GameObject.Team.Enemy);
            Rebel1.MissileCount = 0;
            this.addGameObject(Rebel1, new Vector3(2, 2, 3));
            HeavyShip Rebel2 = new HeavyShip ("Rebel Officer", new Vector3(4, 2, 3), GameObject.Team.Enemy);
            Rebel2.MissileCount = 0;
            this.addGameObject(Rebel2, new Vector3(3, 2, 3));

            this.AddEnvObject(GridCube.TerrainType.asteroid, 2, 1, 4);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 1, 2, 4);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 2, 4);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 1, 4);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 3, 4);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 2, 3, 4);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 1, 3, 4);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 1, 2, 4);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 1, 1, 4);

            this.AddEnvObject(GridCube.TerrainType.asteroid, 2, 1, 6);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 1, 2, 6);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 2, 6);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 1, 6);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 3, 6);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 2, 3, 6);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 1, 3, 6);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 1, 2, 6);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 1, 1, 6);

            for (int i = 0; i < 5; i++) {
                for (int j = 0; j < 5; j++) {
                    if(i != 2 || j != 2)
                        this.AddEnvObject(GridCube.TerrainType.asteroid, i, j, 5);
                }
            }


        }
    }
}
