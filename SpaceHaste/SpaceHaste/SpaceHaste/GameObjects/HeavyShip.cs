﻿using System;
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
            : base(name, location, side, 100, 120, 13, 1, 12, 60, new double[] { 1.66, 1, .8 })
        {
        }
        public override void Load()
        {
            base.Load();
            Model = GraphicsManager.Content.Load<Model>("heavy_ship_combined");

            Scale = 2f;

        }
    } 
}
