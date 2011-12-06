using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;
using SpaceHaste.DPSFParticles;

namespace SpaceHaste.Maps
{
    class MapPlanetTest : Map
    {
        public MapPlanetTest()
            : base(new Vector3(12, 12, 12))
        {
            Act = 3;
            Scene = 2;
        }
        protected override void InitMapGameObjects()
        {
            GameObject Ceruleo = new StandardShip("Ceruleo", new Vector3(2, 2, 08), GameObject.Team.Player);
            //Ceruleo.energy[0] = 100000;
            //
            Ceruleo.MovementRange = 80000;
            this.addGameObject(Ceruleo, Ceruleo.GridPosition);

            GameObject Enemy = new StandardShip("test", new Vector3(2, 2, 2), GameObject.Team.Enemy);
            this.addGameObject(Enemy, Enemy.GridPosition);

            this.AddPlanet(2, 2, 2, 6);
        }
    }
}
