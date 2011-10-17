using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceHaste.Primitives
{
    public class QuadManager : DrawableGameComponent
    {
        public static List<Quad> Quads;
        GraphicsDeviceManager graphics;
        public QuadManager(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            Quads = new List<Quad>();
            this.graphics = graphics;
        }

        public override void Draw(GameTime gameTime)
        {
            //for (int i = 0; i < Quads.Count; i++)
                //Quads[i].DrawLine(graphics);
            base.Draw(gameTime);
        }
        public static void AddLine(Quad quad)
        {
            Quads.Add(quad);
        }
        public static void Clear()
        {
            Quads = new List<Quad>();
        }
        public static void RemoveLine(Quad quad)
        {
            Quads.Remove(quad);
        }
    }
}