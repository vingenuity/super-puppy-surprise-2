using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;
using SpaceHaste.DPSFParticles;

namespace SpaceHaste.Maps
{
    class Map_Act2Scene3 : Map
    {
        public Map_Act2Scene3()
            : base(new Vector3(12, 12, 12))
        {
            Act = 2;
            Scene = 3;
        }

        protected override void InitMapGameObjects()
        {
            GameObject Ceruleo = new StandardShip("Ceruleo",   new Vector3(2, 2, 09), GameObject.Team.Player);
            GameObject Reubber = new HeavyShip   ("Reubber",   new Vector3(3, 2, 11), GameObject.Team.Player);
            GameObject Red2    = new HeavyShip   ("RED TWO",   new Vector3(1, 2, 11), GameObject.Team.Player);
            GameObject Red3    = new HeavyShip   ("RED THREE", new Vector3(2, 1, 11), GameObject.Team.Player);
            GameObject Red4    = new HeavyShip   ("RED FOUR",  new Vector3(2, 3, 11), GameObject.Team.Player);
            this.addGameObject(Ceruleo, Ceruleo.GridPosition);
            this.addGameObject(Reubber, Reubber.GridPosition);
            this.addGameObject(Red2,    Red2.GridPosition);
            this.addGameObject(Red3, Red3.GridPosition);
            this.addGameObject(Red4, Red4.GridPosition);

            GameObject Escourt1 = new LightShip("ESCOURT", new Vector3(10, 10, 2), GameObject.Team.Enemy);
            GameObject Escourt2 = new LightShip("ESCOURT", new Vector3(9,  10, 4), GameObject.Team.Enemy);
            GameObject Escourt3 = new LightShip("ESCOURT", new Vector3(10, 9,  4), GameObject.Team.Enemy);
            this.addGameObject(Escourt1, Escourt1.GridPosition);
            this.addGameObject(Escourt2, Escourt2.GridPosition);
            this.addGameObject(Escourt3, Escourt3.GridPosition);

            GameObject Security1 = new LightShip("SECURITY DETAIL", new Vector3(2, 10, 2), GameObject.Team.Enemy);
            GameObject Security2 = new LightShip("SECURITY DETAIL", new Vector3(4, 10, 2), GameObject.Team.Enemy);
            GameObject Security3 = new LightShip("SECURITY DETAIL", new Vector3(2, 8, 2), GameObject.Team.Enemy);
            this.addGameObject(Security1, Security1.GridPosition);
            this.addGameObject(Security2, Security2.GridPosition);
            this.addGameObject(Security3, Security3.GridPosition);

            GameObject rebel1 = new HeavyShip("REBEL SCUM", new Vector3(10, 2, 10), GameObject.Team.Enemy);
            GameObject rebel2 = new HeavyShip("REBEL SCUM", new Vector3(10, 4, 10), GameObject.Team.Enemy);
            GameObject rebel3 = new HeavyShip("REBEL SCUM", new Vector3(08, 2, 10), GameObject.Team.Enemy);
            this.addGameObject(rebel1, rebel1.GridPosition);
            this.addGameObject(rebel2, rebel2.GridPosition);
            this.addGameObject(rebel3, rebel3.GridPosition);

            this.AddPlanet(2, 2, 2, 8);
        }
    }
}
