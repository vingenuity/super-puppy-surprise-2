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
    class AttackShip : GameObject
    {
        public AttackShip(String name, Vector3 location, Team side)
            : base(name, location, side, 100, 100, 20, 2, 15, 60, new double[] { .7, .7, 1.44 })
        {
        }
        public override void Load()
        {
            base.Load();

            if (team == Team.Player)
                Model = GraphicsManager.Content.Load<Model>("models/heavy_ship_blue");
            else if (team == Team.Enemy)
                Model = GraphicsManager.Content.Load<Model>("models/heavy_ship_red");
            ModelType = 3;
            Scale = 25f;

        }
    }
}
