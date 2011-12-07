using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;
using SpaceHaste.DPSFParticles;

namespace SpaceHaste.Maps
{
    class Map_Act3Scene1 : Map
    {
        public Map_Act3Scene1()
            : base(new Vector3(18, 18, 18))
        {
            Act = 3;
            Scene = 3;
        }


        protected override void InitMapGameObjects()
        {
            GameObject Ceruleo = new StandardShip("Ceruleo", new Vector3(3, 3, 08), GameObject.Team.Player);
            GameObject Grau = new AttackShip("Grau", new Vector3(4, 4, 09), GameObject.Team.Player);
            GameObject Reubber = new HeavyShip("Reubber", new Vector3(2, 3, 10), GameObject.Team.Player);
            GameObject Viridis = new LightShip("Viridis", new Vector3(4, 3, 10), GameObject.Team.Player);
            GameObject Katie = new MissileShip("Katie", new Vector3(9, 7, 4), GameObject.Team.Player);
            this.addGameObject(Ceruleo, Ceruleo.GridPosition);
            this.addGameObject(Grau, Grau.GridPosition);
            this.addGameObject(Reubber, Reubber.GridPosition);
            this.addGameObject(Viridis, Viridis.GridPosition);
            this.addGameObject(Katie, Katie.GridPosition);

            GameObject Albid = new MissileShip("Albid", new Vector3(2, 2, 2), GameObject.Team.Enemy);
            this.addGameObject(Albid, Albid.GridPosition);
        }
    }
}
