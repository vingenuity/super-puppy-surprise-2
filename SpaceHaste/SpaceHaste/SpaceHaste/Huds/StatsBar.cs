using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceHaste.GameMech;
using SpaceHaste.GameMech.BattleMechanicsManagers;

namespace SpaceHaste.Huds
{
    class StatsBar
    {

        Texture2D backpaneLeft = Graphics.GraphicsManager.Content.Load<Texture2D>("UI_backPane_blue");
        Texture2D backpaneRight = Graphics.GraphicsManager.Content.Load<Texture2D>("UI_backPane_red");

        Rectangle left = new Rectangle(0, (Graphics.GraphicsManager.graphics.PreferredBackBufferHeight / 20) * 19, Graphics.GraphicsManager.graphics.PreferredBackBufferWidth / 2, (Graphics.GraphicsManager.graphics.PreferredBackBufferHeight / 20));
        Rectangle right = new Rectangle(Graphics.GraphicsManager.graphics.PreferredBackBufferWidth / 2, (Graphics.GraphicsManager.graphics.PreferredBackBufferHeight / 20) * 19, Graphics.GraphicsManager.graphics.PreferredBackBufferWidth / 2, (Graphics.GraphicsManager.graphics.PreferredBackBufferHeight / 20));

        public StatsBar() { }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {

            spriteBatch.Draw(backpaneLeft, left, Color.White);
            //spriteBatch.DrawString(spriteFont, "HULL: " + BattleMechanicsManager.Instance.CurrentGameObjectSelected.getHull(), 

            spriteBatch.Draw(backpaneRight, right, Color.White);

        }

    }
}
