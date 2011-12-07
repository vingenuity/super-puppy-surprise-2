using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;
using SpaceHaste.DPSFParticles;

namespace SpaceHaste.Maps
{
    class Map_Act3Scene2 : Map
    {
        public Map_Act3Scene2()
            : base(new Vector3(12, 12, 12))
        {
            Act = 3;
            Scene = 2;
        }
        protected override void InitMapGameObjects()
        {
            GameObject Ceruleo = new AttackShip  ("Ceruleo", new Vector3(3, 3, 08), GameObject.Team.Player);
            GameObject Grau    = new AttackShip  ("Grau",    new Vector3(4, 4, 09), GameObject.Team.Player);
            GameObject Reubber = new HeavyShip   ("Reubber", new Vector3(2, 3, 10), GameObject.Team.Player);
            GameObject Viridis = new LightShip   ("Viridis", new Vector3(4, 3, 10), GameObject.Team.Player);
            this.addGameObject(Ceruleo, Ceruleo.GridPosition);
            this.addGameObject(Grau, Grau.GridPosition);
            this.addGameObject(Reubber, Reubber.GridPosition);
            this.addGameObject(Viridis, Viridis.GridPosition);

            GameObject Katie = new MissileShip("Katie", new Vector3(9, 7, 4), GameObject.Team.Player);
            this.addGameObject(Katie, Katie.GridPosition);

            GameObject Thug1 = new HeavyShip("Albid Thug", new Vector3(2, 2, 2), GameObject.Team.Enemy);
            this.addGameObject(Thug1, Thug1.GridPosition);

            this.AddEnvObject(GridCube.TerrainType.wreck, 2, 2, 5);
        }
    }
}
