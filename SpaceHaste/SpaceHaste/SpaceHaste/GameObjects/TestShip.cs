using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SpaceHaste.Graphics;
using Microsoft.Xna.Framework.Graphics;
using SpaceHaste.Maps;

namespace SpaceHaste.GameObjects
{
    public class TestShip : GameObject
    {
        public TestShip()
        {
            Model = GraphicsManager.Content.Load<Model>("Ship");
            GraphicsManager.GraphicsGameObjects.Add(this);
            Scale = .5f;
        }
    }
}
