using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceHaste;

namespace GameStateManagement
{
    class CreditsScreen : MenuScreen
    {
        List<String> creditRoll;

        public CreditsScreen()
            : base("Credits")
        {
            MenuEntry backEntry = new MenuEntry("Back to Title");
            backEntry.Selected += BackSelected;
            MenuEntries.Add(backEntry);

            //List of credits
            creditRoll = new List<String>();
            creditRoll.Add("Engine Programming: Thomas Robbins");
            creditRoll.Add("AI and Controls Programming: Vincent Kocks");
            creditRoll.Add("Story and Gameplay Programming: Matthew Leary");
            creditRoll.Add("Music and UI Programming: Trevor Gray");
            creditRoll.Add("Modeling and Art: Jorge Cereijo");
        }

        void BackSelected(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            // Add variables for centering credits.
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = new Vector2(0, 0);
            Vector2 textPosition = (viewportSize - textSize) / 2;
            int startY = 200;

            //Print the credits
            spriteBatch.Begin();
            foreach (String person in creditRoll)
            {
                textSize = font.MeasureString(person);
                textPosition = (viewportSize - textSize) / 2;
                textPosition.Y = startY;
                spriteBatch.DrawString(font, person, textPosition, Color.White);
                startY += 40;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
