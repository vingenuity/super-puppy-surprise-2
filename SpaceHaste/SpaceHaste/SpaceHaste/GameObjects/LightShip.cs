using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;
using Microsoft.Xna.Framework;
using SpaceHaste.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceHaste.GameObjects
{
    public class LightShip : GameObject
    {
        public LightShip(String name, Vector3 location, Team side)
            : base(name, location, side, 100, 80, 20, 1, 8, 50, new double[] { .8, 1, 1.66 })
        {
           
        }
        public override void Load()
        {
            base.Load();
            Model = GraphicsManager.Content.Load<Model>("models/light_ship_red");
            Scale = 15f;

        }
    }

}
