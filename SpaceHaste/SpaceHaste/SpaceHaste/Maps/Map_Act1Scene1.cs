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
            this.addGameObject(new AttackShip ("Viridis", new Vector3(2, 2, 6), GameObject.Team.Player), new Vector3(2, 2, 6));

            this.addGameObject(new LightShip ("Rebel Ship 1", new Vector3(2, 2, 3), GameObject.Team.Enemy), new Vector3(2, 2, 3));
            this.addGameObject(new HeavyShip ("Rebel Ship 2", new Vector3(4, 2, 3), GameObject.Team.Enemy), new Vector3(4, 2, 3));
        }
    }
}
