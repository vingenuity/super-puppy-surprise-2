using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceHaste.Graphics
{
    public class Missile
    {
        public static Vector3 GridPosition;
        public static Vector3 DrawPosition;
        public static bool shouldDraw = false;
        public static Model Model;
        public static Vector3 Direction;
    }
}
