using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;
using SpaceHaste.DPSFParticles;

namespace SpaceHaste.Maps
{
    class Map_Level1 : Map
    {
        public Map_Level1()
            : base(new Vector3(6, 4, 12))
        { }

        protected override void InitMapGameObjects()
        {
            this.addGameObject(new HeavyShip("Player Ship 1", new Vector3(2, 2, 10), GameObject.Team.Player), new Vector3(2, 2, 10));
            this.addGameObject(new LightShip("Enemy Ship 1", new Vector3(2, 2, 3), GameObject.Team.Enemy), new Vector3(2, 2, 3));
        }
    }
}
