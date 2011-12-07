using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;
using SpaceHaste.DPSFParticles;
using System;

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
            GameObject Ceruleo = new AttackShip("Ceruleo", new Vector3(3, 5, 15), GameObject.Team.Player);
            GameObject Blau = new AttackShip("Blau", new Vector3(3, 3, 15), GameObject.Team.Player);
            GameObject Katie = new MissileShip("Katie", new Vector3(3, 7, 17), GameObject.Team.Player);

            GameObject Reubber = new HeavyShip   ("Reubber", new Vector3(3, 14, 15), GameObject.Team.Player);
            GameObject Red1    = new HeavyShip   ("Red1",    new Vector3(3, 12, 17), GameObject.Team.Player);
            GameObject Red2    = new HeavyShip   ("Red2",    new Vector3(3, 16, 17), GameObject.Team.Player);

            GameObject Viridis = new LightShip   ("Viridis", new Vector3(6, 5, 15), GameObject.Team.Player);
            GameObject Green1  = new LightShip   ("Green1",  new Vector3(5, 5, 17), GameObject.Team.Player);
            GameObject Green2  = new LightShip   ("Green2",  new Vector3(7, 5, 17), GameObject.Team.Player);

            this.addGameObject(Ceruleo, Ceruleo.GridPosition);
            this.addGameObject(Blau, Blau.GridPosition);
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

            GameObject traitor1 = new LightShip("Traitor", new Vector3(1, 15, 1), GameObject.Team.Enemy);
            GameObject traitor2 = new HeavyShip("Traitor", new Vector3(1, 14, 1), GameObject.Team.Enemy);
            GameObject traitor3 = new AttackShip("Traitor", new Vector3(3, 13, 1), GameObject.Team.Enemy);

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
            this.addGameObject(traitor1, traitor1.GridPosition);
            this.addGameObject(traitor2, traitor2.GridPosition);
            this.addGameObject(traitor3, traitor3.GridPosition);

            this.AddPlanet(3, 3, 5, 6, 1);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 9, 8);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 1, 9, 6);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 10, 8);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 2, 8, 9);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 5, 6, 7);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 6, 8, 8);

            this.AddPlanet(11, 11, 11, 8, 0);

            //int radius = 3;
            //int x = 3;
            //int y = 3;
            //int z = 5;
            //for (int i = x - (int)radius; i < x + (int)radius; i++)
            //    for (int j = y - (int)radius; j < y + (int)radius; j++)
            //        for (int k = z - (int)radius; k < z + (int)radius; k++)
            //        {
            //            Vector3 toCenter = new Vector3(i - x, j - y, k - z);
            //            if (toCenter.Length() == radius - 0)
            //            {
            //                this.AddEnvObject(GridCube.TerrainType.nebula, i, j, k);
            //            }
                        
            //        }
        }
    }
}
