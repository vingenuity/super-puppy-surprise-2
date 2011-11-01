using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;

namespace SpaceHaste.Maps
{
    public class Map1 : Map
    {
        public Map1()
            : base(12)
        {

        }
        protected override void InitMapGameObjects()
        {
            this.addGameObject(new Ship("Player Ship 1", new Vector3(1, 4, 7), GameObject.Team.Player));
            this.addGameObject(new Ship("Player Ship 2", new Vector3(4, 3, 7), GameObject.Team.Player));
            this.addGameObject(new Ship("Player Ship 3", new Vector3(7, 4, 7), GameObject.Team.Player));

            this.addGameObject(new Ship("Enemy Ship 1", new Vector3(1, 4, 3), GameObject.Team.Enemy));
            this.addGameObject(new Ship("Enemy Ship 2", new Vector3(4, 3, 3), GameObject.Team.Enemy));
            //this.addGameObject(new Ship("Enemy Ship 3", new Vector3(7, 4, 3), GameObject.Team.Enemy));

            AddGridIsometric();
        }
    }
}
