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
            GameObject Ceruleo = new StandardShip("Ceruleo", new Vector3(2, 2, 9), GameObject.Team.Player);
            GameObject Reubber = new HeavyShip   ("Reubber", new Vector3(4, 2, 9), GameObject.Team.Player);
            GameObject Viridis = new LightShip   ("Viridis", new Vector3(3, 2, 8), GameObject.Team.Player);
            this.addGameObject(Ceruleo, Ceruleo.GridPosition);
            this.addGameObject(Reubber, Reubber.GridPosition);
            this.addGameObject(Viridis, Viridis.GridPosition);
           this.AddEnvObject(GridCube.TerrainType.nebula,1,1,9);
     //       this.AddEnvObject(GridCube.TerrainType.nebula, 1, 2, 10);
           // this.AddEnvObject(GridCube.TerrainType.nebula, 1, 3, 10);
            this.AddEnvObject(GridCube.TerrainType.wreck, 1, 3, 10);
            LightShip Rebel1 = new LightShip   ("Grau Rebel", new Vector3(2, 2, 3), GameObject.Team.Enemy);
            Rebel1.MissileCount = 0;
            this.addGameObject(Rebel1, new Vector3(2, 2, 3));
            LightShip Rebel2 = new LightShip   ("Grau Rebel", new Vector3(4, 2, 3), GameObject.Team.Enemy);
            Rebel2.MissileCount = 0;
            this.addGameObject(Rebel2, new Vector3(4, 2, 3));

           // DPSFParticles.FireOnShipsParticle.CreateParticle(new Vector3(0, 0, 0));
        }
    }
}
