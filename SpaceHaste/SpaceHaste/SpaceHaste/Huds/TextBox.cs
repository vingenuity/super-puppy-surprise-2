using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceHaste.GameMech.CutScenes;
using SpaceHaste.Graphics;

namespace SpaceHaste.Huds
{
    class TextBox
    {
        Vector2 TextPosition;
        Texture2D texture1;
        float ScreenWidth = GraphicsManager.graphics.PreferredBackBufferWidth;
        float ScreenHeight = GraphicsManager.graphics.PreferredBackBufferHeight;

        public TextBox() {
            TextPosition = new Vector2((int)(ScreenWidth / 3) + 10, (int)(4 * (ScreenHeight / 5) - ScreenHeight / 20 - 5));
            texture1 = Hud.Content.Load<Texture2D>("UI_backPane_blue");

        }


        public void Draw(String text)
        {

            Hud.spriteBatch.Begin();
           
            
            Hud.spriteBatch.Draw(texture1, new Rectangle((int)(ScreenWidth / 3), (int)(4 * (ScreenHeight / 5) - ScreenHeight / 20 - 5), (int)(ScreenWidth/3), (int)(ScreenHeight/5)), Color.Plum);
            Hud.spriteBatch.DrawString(Hud.spriteFont, text, TextPosition, Color.Yellow, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
            
            
            Hud.spriteBatch.End();

        }

        public void endBox() {
            ScreenWidth = 0;
            ScreenHeight = 0;
        }

    }
}
