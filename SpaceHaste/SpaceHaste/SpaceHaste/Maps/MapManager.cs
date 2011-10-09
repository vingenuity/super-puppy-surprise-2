using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceHaste.Maps
{
    public class MapManager : GameComponent
    {
        public static Map Map;
        public MapManager(Game game)
            : base(game)
        {
            Map = new Map1();
        }
    }
}
