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
    public class LightShip : GameObject
    {
        public LightShip(String name, Vector3 location, Team side, int maxHull, int maxShield, double regeneration, int numMissiles, int lsrDmg, int missDmg, double[] eff)
            : base(name, location, side, maxHull, maxShield, regeneration, numMissiles, lsrDmg, missDmg, eff)
        {
        }
        public override void Load()
        {
            base.Load();
            Model = GraphicsManager.Content.Load<Model>("light_ship_combined");

            Scale = 15f;

        }
    }

}
