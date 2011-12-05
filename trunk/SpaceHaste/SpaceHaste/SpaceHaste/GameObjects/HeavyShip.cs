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
    public class HeavyShip : GameObject
    {
        public HeavyShip(String name, Vector3 location, Team side)
            : base(name, location, side, 100, 120, 13, 1, 12, 100, new double[] { 1.66, 1, .8 })
        {
            MissileRange = 8;
        }
        public override void Load()
        {
            base.Load(); 
            if (team == Team.Player)
                Model = GraphicsManager.Content.Load<Model>("models/heavy_ship_blue_swap");
            else if (team == Team.Enemy)
                Model = GraphicsManager.Content.Load<Model>("models/heavy_ship_red_swap");
            Scale = 30f;
        }
    } 
}
