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
            this.addGameObject(new StandardShip("Ceruleo", new Vector3(2, 2, 10), GameObject.Team.Player), new Vector3(2, 2, 10));
            this.addGameObject(new HeavyShip ("Reubber", new Vector3(4, 2, 10), GameObject.Team.Player), new Vector3(4, 2, 10));
            this.addGameObject(new AttackShip  ("Viridis", new Vector3(3, 2, 8), GameObject.Team.Player), new Vector3(3, 2, 8));

            LightShip Rebel1 = new LightShip   ("Grau Rebel", new Vector3(2, 2, 3), GameObject.Team.Enemy);
            Rebel1.MissileCount = 0;
            this.addGameObject(Rebel1, Rebel1.GridPosition);
            LightShip Rebel2 = new LightShip   ("Grau Rebel", new Vector3(4, 2, 3), GameObject.Team.Enemy);
            Rebel2.MissileCount = 0;
            this.addGameObject(Rebel2, Rebel2.GridPosition);
            LightShip Rebel3 = new LightShip("Grau Rebel", new Vector3(3, 2, 4), GameObject.Team.Enemy);
            Rebel2.MissileCount = 0;
            this.addGameObject(Rebel3, Rebel3.GridPosition);

            this.AddEnvObject(GridCube.TerrainType.nebula, 2, 2, 10);
            this.AddEnvObject(GridCube.TerrainType.nebula, 3, 2, 10);
            this.AddEnvObject(GridCube.TerrainType.nebula, 4, 2, 10);
            this.AddEnvObject(GridCube.TerrainType.nebula, 2, 2, 9);
            this.AddEnvObject(GridCube.TerrainType.nebula, 2, 5, 10);
            this.AddEnvObject(GridCube.TerrainType.nebula, 3, 3, 5);
        }
    }
}
