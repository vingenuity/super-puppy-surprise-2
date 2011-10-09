﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceHaste.Maps
{
    public class GridSquare
    {
        public int X, Y, Z;
        public static float GRIDSQUARELENGTH = 50;
        public Vector3 Center;
        public GridSquare(int X, int Y, int Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            Center = new Vector3(X + GRIDSQUARELENGTH / 2, Y + GRIDSQUARELENGTH / 2, + Z + GRIDSQUARELENGTH / 2);
        }
    }
}