using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;

namespace SpaceHaste.Maps
{
    public class Map_Act2Scene1 : Map
    {
        public Map_Act2Scene1()
            : base(new Vector3(13, 9, 22))
        {
            Act = 2;
            Scene = 1;
        }

        protected override void InitMapGameObjects()
        {
            this.addGameObject(new MissileShip("Ceruleo", new Vector3(6, 2, 19), GameObject.Team.Player), new Vector3(6, 2, 19));
            this.addGameObject(new HeavyShip("Reubber ", new Vector3(6, 2, 21), GameObject.Team.Player), new Vector3(6, 2, 21));
            this.addGameObject(new AttackShip("RED TWO", new Vector3(7, 2, 20), GameObject.Team.Player), new Vector3(7, 2, 20));
            this.addGameObject(new LightShip("RED THREE", new Vector3(5, 2, 20), GameObject.Team.Player), new Vector3(5, 2, 20));

            this.addGameObject(new MissileShip("Rebel Lt. Gruen", new Vector3(6, 2, 3), GameObject.Team.Enemy), new Vector3(2, 2, 3));
            this.addGameObject(new LightShip("Rebel Joe", new Vector3(4, 2, 3), GameObject.Team.Enemy), new Vector3(4, 2, 3));
            this.addGameObject(new LightShip("Rebel Howe", new Vector3(5, 2, 2), GameObject.Team.Enemy), new Vector3(5, 2, 2));
            this.addGameObject(new AttackShip("Rebel John", new Vector3(4, 4, 4), GameObject.Team.Enemy), new Vector3(4, 4, 4));


            for (int i = 0; i <= 1; i++)
            {
                for (int j = 0; j <= 1; j++)
                {
                    this.AddEnvObject(GridCube.TerrainType.asteroid, i + 6, j, 15);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, 4, j + 5, i + 11);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, j + 4, 4, i + 11);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, i + 3, j + 3, 4);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, 7, j, i + 13);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, 7, j, i + 16);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, i + 5, j + 2, 4);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, 7, j + 4, i + 9);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, i + 4, 6, j + 19);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, i + 3, 4, j + 8);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, i + 6, j + 4, 16);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, i + 3, j + 6, 17);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, 2, i + 3, j + 3);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, 9, i + 5, j + 10);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, 6, 7, i + 14);
                    this.AddEnvObject(GridCube.TerrainType.asteroid, 6, i + 7, 14);

                }
                this.AddEnvObject(GridCube.TerrainType.asteroid, 6, 7, i + 14);
                this.AddEnvObject(GridCube.TerrainType.asteroid, 6, i + 7, 14);
            }

            AddGridIsometric();
        }
    }
}
