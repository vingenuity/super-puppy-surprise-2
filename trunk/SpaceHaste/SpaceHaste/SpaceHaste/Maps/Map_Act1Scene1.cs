using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;
using SpaceHaste.DPSFParticles;

namespace SpaceHaste.Maps
{
    class Map_Act1Scene1 : Map
    {
        public Map_Act1Scene1()
            : base(new Vector3(6, 4, 12))
        {
            Act = 1;
            Scene = 1;
        }

        protected override void InitMapGameObjects()
        {
            this.addGameObject(new StandardShip("Ceruleo", new Vector3(2, 2, 10), GameObject.Team.Player), new Vector3(2, 2, 10));
            this.addGameObject(new MissileShip ("Reubber", new Vector3(4, 2, 10), GameObject.Team.Player), new Vector3(4, 2, 10));
            this.addGameObject(new AttackShip  ("Viridis", new Vector3(2, 2, 6), GameObject.Team.Player), new Vector3(2, 2, 6));
     //       this.AddEnvObject(GridCube.TerrainType.nebula,1,1,10);
     //       this.AddEnvObject(GridCube.TerrainType.nebula, 1, 2, 10);
     //       this.AddEnvObject(GridCube.TerrainType.nebula, 1, 3, 10);
            LightShip Rebel1 = new LightShip   ("Grau Rebel", new Vector3(2, 2, 3), GameObject.Team.Enemy);
            Rebel1.MissileCount = 0;
            this.addGameObject(Rebel1, new Vector3(2, 2, 3));
            LightShip Rebel2 = new LightShip   ("Grau Rebel", new Vector3(4, 2, 3), GameObject.Team.Enemy);
            Rebel2.MissileCount = 0;
            this.addGameObject(Rebel2, new Vector3(4, 2, 3));
        }
    }
}
