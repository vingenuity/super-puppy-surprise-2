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
    class StandardShip : GameObject
    {
        public StandardShip(String name, Vector3 location, Team side)
            : base(name, location, side, 100, 100, 13, 1, 10, 50, new double[] { 1, 1, 1 })
        {
        }
        public override void Load()
        {
            base.Load();
            
            Model = GraphicsManager.Content.Load<Model>("models/light_ship_blue");

            Scale = 6f;

        }
    }
}
