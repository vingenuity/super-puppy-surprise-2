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
            : base(8)
        {

        }
        protected override void InitMapGameObjects()
        {
            this.addGameObject(new Ship(new Vector3(4, 4, 4)));
            this.addGameObject(new Ship(new Vector3(2, 2, 4)));
            this.addGameObject(new Ship(new Vector3(3, 3, 3)));
            //AddGridXYZ();
            AddGridIsometric();
        }
    }
}
