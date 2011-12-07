using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;

namespace SpaceHaste.Maps
{
    public class Map_Act2Scene2 : Map
    {
        public Map_Act2Scene2()
            : base(new Vector3(10, 10, 13))
        {
            Act = 2;
            Scene = 2;
        }

        protected override void InitMapGameObjects()
        {
            this.addGameObject(new AttackShip("Ceruleo", new Vector3(12, 13, 19), GameObject.Team.Player), new Vector3(9, 9, 12));
            this.addGameObject(new HeavyShip("Reubber", new Vector3(13, 14, 20), GameObject.Team.Player), new Vector3(8, 9, 11));
            this.addGameObject(new LightShip("RED TWO", new Vector3(13, 14, 19), GameObject.Team.Player), new Vector3(9, 8, 10));
            this.addGameObject(new LightShip("RED THREE", new Vector3(12, 13, 20), GameObject.Team.Player), new Vector3(8, 8, 11));

            this.addGameObject(new HeavyShip("Rebel Lt. Roht", new Vector3(2, 2, 3), GameObject.Team.Enemy), new Vector3(2, 2, 3));
            this.addGameObject(new LightShip("Rebel Hulk", new Vector3(4, 2, 3), GameObject.Team.Enemy), new Vector3(4, 2, 3));
            this.addGameObject(new LightShip("Rebel Payne", new Vector3(5, 2, 2), GameObject.Team.Enemy), new Vector3(5, 2, 2));
           
            this.addGameObject(new MissileShip("Rebel Tim", new Vector3(2, 5, 3), GameObject.Team.Enemy), new Vector3(2, 5, 3));


            this.AddEnvObject(GridCube.TerrainType.nebula, 5, 3, 9);
            this.AddEnvObject(GridCube.TerrainType.nebula, 4, 6, 8);
            this.AddEnvObject(GridCube.TerrainType.nebula, 8, 5, 3);
            this.AddEnvObject(GridCube.TerrainType.nebula, 6, 2, 5);
            this.AddEnvObject(GridCube.TerrainType.nebula, 3, 5, 5);
            this.AddEnvObject(GridCube.TerrainType.nebula, 5, 2, 5);
            this.AddEnvObject(GridCube.TerrainType.nebula, 5, 6, 3);
            this.AddEnvObject(GridCube.TerrainType.nebula, 3, 4, 3);
            this.AddEnvObject(GridCube.TerrainType.nebula, 1, 2, 2);
            this.AddEnvObject(GridCube.TerrainType.nebula, 4, 2, 3);
            this.AddEnvObject(GridCube.TerrainType.nebula, 4, 9, 5);
            this.AddEnvObject(GridCube.TerrainType.nebula, 6, 8, 6);
            this.AddEnvObject(GridCube.TerrainType.nebula, 7, 5, 6);
            this.AddEnvObject(GridCube.TerrainType.nebula, 7, 3, 10);
            this.AddEnvObject(GridCube.TerrainType.nebula, 8, 6, 8);

            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 3, 9);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 4, 2, 11);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 5, 4, 9);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 1, 5, 8);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 4, 6, 7);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 4, 3, 8);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 2, 2, 11);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 2, 2, 7);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 2, 8);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 4, 2, 7);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 6, 3, 9);
            

            AddGridIsometric();
        }
    }
}
