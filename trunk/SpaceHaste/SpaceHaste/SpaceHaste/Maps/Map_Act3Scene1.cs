using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;
using SpaceHaste.DPSFParticles;

namespace SpaceHaste.Maps
{
    class Map_Act3Scene1 : Map
    {
        public Map_Act3Scene1()
            : base(new Vector3(6, 6, 18))
        {
            Act = 3;
            Scene = 3;
        }


        protected override void InitMapGameObjects()
        {
            GameObject Ceruleo = new StandardShip("Ceruleo", new Vector3(2, 4, 15), GameObject.Team.Player);
            GameObject Grau = new AttackShip("Grau", new Vector3(2, 5, 13), GameObject.Team.Player);
            GameObject Reubber = new HeavyShip("Reubber", new Vector3(3, 4, 16), GameObject.Team.Player);
            GameObject Viridis = new LightShip("Viridis", new Vector3(3, 3, 15), GameObject.Team.Player);
            GameObject Katie = new MissileShip("Katie", new Vector3(4, 2, 14), GameObject.Team.Player);
            this.addGameObject(Ceruleo, Ceruleo.GridPosition);
            this.addGameObject(Grau, Grau.GridPosition);
            this.addGameObject(Reubber, Reubber.GridPosition);
            this.addGameObject(Viridis, Viridis.GridPosition);
            this.addGameObject(Katie, Katie.GridPosition);

            GameObject Rebel1 = new StandardShip("Federalist Grunt", new Vector3(5, 3, 1), GameObject.Team.Enemy);
            GameObject Rebel2 = new AttackShip("Federalist Grunt", new Vector3(2, 2, 2), GameObject.Team.Enemy);
            GameObject Rebel3 = new HeavyShip("Federalist Grunt", new Vector3(3, 2, 3), GameObject.Team.Enemy);
            GameObject Rebel4 = new LightShip("Federalist Ace", new Vector3(1, 3, 1), GameObject.Team.Enemy);
            GameObject Rebel5 = new StandardShip("Federalist Grunt", new Vector3(2, 1, 2), GameObject.Team.Enemy);
            this.addGameObject(Rebel1, Rebel1.GridPosition);
            this.addGameObject(Rebel2, Rebel2.GridPosition);
            this.addGameObject(Rebel3, Rebel3.GridPosition);
            this.addGameObject(Rebel4, Rebel4.GridPosition);
            this.addGameObject(Rebel5, Rebel5.GridPosition);

            this.AddEnvObject(GridCube.TerrainType.wreck, 3, 2, 10);
            this.AddEnvObject(GridCube.TerrainType.wreck, 2, 2, 9);
            this.AddEnvObject(GridCube.TerrainType.wreck, 3, 3, 12);
            this.AddEnvObject(GridCube.TerrainType.wreck, 2, 4, 5);
            this.AddEnvObject(GridCube.TerrainType.wreck, 4, 2, 3);
            this.AddEnvObject(GridCube.TerrainType.wreck, 5, 3, 14);
            this.AddEnvObject(GridCube.TerrainType.wreck, 4, 2, 10);
            this.AddEnvObject(GridCube.TerrainType.wreck, 2, 4, 7);
            this.AddEnvObject(GridCube.TerrainType.wreck, 0, 2, 13);
            this.AddEnvObject(GridCube.TerrainType.wreck, 3, 3, 11);
            this.AddEnvObject(GridCube.TerrainType.wreck, 4, 2, 6);
            this.AddEnvObject(GridCube.TerrainType.wreck, 1, 1, 7);

            this.AddEnvObject(GridCube.TerrainType.asteroid, 1, 2, 10);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 4, 2, 9);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 0, 3, 11);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 3, 5);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 2, 2, 3);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 5, 5, 14);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 1, 2, 10);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 2, 3, 11);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 2, 13);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 5, 16);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 2, 4, 4);
            this.AddEnvObject(GridCube.TerrainType.asteroid, 3, 5, 3);

        }
    }
}
