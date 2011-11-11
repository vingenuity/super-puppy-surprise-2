using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceHaste.GameMech.CutScenes;

namespace SpaceHaste.Huds
{
    class TextBox
    {
        Vector2 TextPosition;
        Texture2D texture1;

        public TextBox() { 
            TextPosition = new Vector2(10, 600);
            texture1 = Hud.Content.Load<Texture2D>("UI_backPane_blue");

        }


        public void Draw(String text)
        {

            Hud.spriteBatch.Begin();
           
            
            Hud.spriteBatch.Draw(texture1, new Rectangle(10, 600, 150, 500), Color.White);
            Hud.spriteBatch.DrawString(Hud.spriteFont, text, TextPosition, Color.Yellow, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
            
            
            Hud.spriteBatch.End();

        }

    }
}
