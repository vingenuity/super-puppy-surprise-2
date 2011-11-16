using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
            for (int i = 0; i < Quads.Count; i++)
                Quads[i].DrawQuad();
            base.Draw(gameTime);
        }
        public static void AddQuad(Quad quad)
        {
            Quads.Add(quad);
        }
        public static void Clear()
        {
            Quads = new List<Quad>();
        }
        public static void RemoveQuad(Quad quad)
        {
            Quads.Remove(quad);
        }
    }
}