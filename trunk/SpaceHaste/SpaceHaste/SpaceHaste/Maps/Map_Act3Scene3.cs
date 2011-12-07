using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;
using SpaceHaste.DPSFParticles;

namespace SpaceHaste.Maps
{
    class Map_Act3Scene3 : Map
    {
        public Map_Act3Scene3()
            : base(new Vector3(18, 18, 18))
        {
            Act = 3;
            Scene = 3;
        }

        protected override void InitMapGameObjects()
        {
            GameObject Ceruleo = new StandardShip("Ceruleo", new Vector3(3, 5, 15), GameObject.Team.Player);
            GameObject Grau = new AttackShip("Grau", new Vector3(3, 3, 15), GameObject.Team.Player);
            GameObject Katie = new MissileShip("Katie", new Vector3(3, 7, 17), GameObject.Team.Player);

            GameObject Reubber = new HeavyShip   ("Reubber", new Vector3(3, 14, 15), GameObject.Team.Player);
            GameObject Red1    = new HeavyShip   ("Red1",    new Vector3(3, 12, 17), GameObject.Team.Player);
            GameObject Red2    = new HeavyShip   ("Red2",    new Vector3(3, 16, 17), GameObject.Team.Player);

            GameObject Viridis = new LightShip   ("Viridis", new Vector3(12, 5, 15), GameObject.Team.Player);
            GameObject Green1  = new LightShip   ("Green1",  new Vector3(11, 5, 17), GameObject.Team.Player);
            GameObject Green2  = new LightShip   ("Green2",  new Vector3(13, 5, 17), GameObject.Team.Player);

            this.addGameObject(Ceruleo, Ceruleo.GridPosition);
            this.addGameObject(Grau, Grau.GridPosition);
            this.addGameObject(Katie, Katie.GridPosition);
            this.addGameObject(Reubber, Reubber.GridPosition);
            this.addGameObject(Red1, Red1.GridPosition);
            this.addGameObject(Red2, Red2.GridPosition);
            this.addGameObject(Viridis, Viridis.GridPosition);
            this.addGameObject(Green1, Green1.GridPosition);
            this.addGameObject(Green2, Green2.GridPosition);
            
            GameObject Albid = new MissileShip ("Traitor Albid", new Vector3(2, 2, 1), GameObject.Team.Enemy);
            GameObject Thug1 = new StandardShip("Albid Thug", new Vector3(1, 3, 1), GameObject.Team.Enemy);
            GameObject Thug2 = new StandardShip("Albid Thug", new Vector3(3, 1, 1), GameObject.Team.Enemy);
            GameObject Thug3 = new StandardShip("Albid Thug", new Vector3(1, 1, 1), GameObject.Team.Enemy);
            GameObject Thug4 = new StandardShip("Albid Thug", new Vector3(3, 3, 1), GameObject.Team.Enemy);

            GameObject Door   = new MissileShip("George Door", new Vector3(15, 10, 4), GameObject.Team.Enemy);
            GameObject Apfel1 = new LightShip  ("Apfel Peon",  new Vector3(14, 8, 3), GameObject.Team.Enemy);
            GameObject Apfel2 = new AttackShip ("Apfel Peon",  new Vector3(16, 8, 5), GameObject.Team.Enemy);
            GameObject Apfel3 = new HeavyShip  ("Apfel Peon",  new Vector3(16, 8, 3), GameObject.Team.Enemy);
            GameObject Apfel4 = new HeavyShip  ("Apfel Peon",  new Vector3(14, 8, 5), GameObject.Team.Enemy);

            this.addGameObject(Albid, Albid.GridPosition);
            this.addGameObject(Thug1, Thug1.GridPosition);
            this.addGameObject(Thug2, Thug2.GridPosition);
            this.addGameObject(Thug3, Thug3.GridPosition);
            this.addGameObject(Thug4, Thug4.GridPosition);
            this.addGameObject(Door, Door.GridPosition);
            this.addGameObject(Apfel1, Apfel1.GridPosition);
            this.addGameObject(Apfel2, Apfel2.GridPosition);
            this.addGameObject(Apfel3, Apfel3.GridPosition);
            this.addGameObject(Apfel4, Apfel4.GridPosition);

            this.AddPlanet(3, 3, 5, 6, 1);
            this.AddPlanet(11, 11, 11, 8, 0);
        }
    }
}
