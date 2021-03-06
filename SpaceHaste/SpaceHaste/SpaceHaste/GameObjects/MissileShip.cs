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
    class MissileShip : GameObject
    {
        public MissileShip(String name, Vector3 location, Team side)
            : base(name, location, side, 100, 100, 8, 5, 8, 65, new double[] { 1.44, 1.44, 1.44 })
        {
        }
        public override void Load()
        {
            base.Load();
            if (team == Team.Player)
                Model = GraphicsManager.Content.Load<Model>("models/gatherer_ship_blue");
            else if (team == Team.Enemy)
                Model = GraphicsManager.Content.Load<Model>("models/gatherer_ship_red");
            ModelType = 2;
            Scale = 65f;
        }
    }
}
