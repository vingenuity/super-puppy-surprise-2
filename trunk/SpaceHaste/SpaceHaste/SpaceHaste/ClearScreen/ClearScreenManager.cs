using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceHaste.ClearScreen
{
    public class ClearScreenManager: DrawableGameComponent
    {
        GraphicsDeviceManager graphics;
        public ClearScreenManager(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            DrawOrder = -1;
            this.graphics = graphics;
        }
        public override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}
