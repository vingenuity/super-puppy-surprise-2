﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceHaste.GameObjects;

namespace SpaceHaste.Maps
{
    public class Map1 : Map
    {
        public Map1()
            : base(8)
        {

        }
        protected override void InitMapGameObjects()
        {
            MapGameObjects = new GameObject[Size, Size, Size];
            AddGameObjectToGridSquare(new TestShip(), 4, 4, 4); 
            AddGameObjectToGridSquare(new TestShip(), 2, 2, 3);
            AddGameObjectToGridSquare(new TestShip(), 0, 0, 0);
            AddGridXYZ();
            base.InitMapGameObjects();
        }
    }
}
