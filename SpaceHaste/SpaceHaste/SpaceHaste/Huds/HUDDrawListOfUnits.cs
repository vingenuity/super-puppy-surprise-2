using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceHaste.GameObjects;
using SpaceHaste.GameMech;
using SpaceHaste.Graphics;

namespace SpaceHaste.Huds
{
    public class HUDDrawListOfUnits
    {
        Texture2D texture1, currentHealth, currentEnergy;

        public HUDDrawListOfUnits()
        {
        }
        public void Load()
        {
            texture1 = Hud.Content.Load<Texture2D>("SketchTexture");
            currentHealth = Hud.Content.Load<Texture2D>("HealthBar");
            currentEnergy = Hud.Content.Load<Texture2D>("EnergyBar");

        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            for (int i = 0; i < GameMechanicsManager.GameObjectList.Count; i++)
                DrawUnitInformation(GameMechanicsManager.GameObjectList[i], spriteBatch, spriteFont, i, GraphicsManager.graphics.PreferredBackBufferWidth, GraphicsManager.graphics.PreferredBackBufferHeight);
        }

        private void DrawUnitInformation(GameObject unit, SpriteBatch spriteBatch, SpriteFont spriteFont, int num, float ScreenWidth, float ScreenHeight)
        {
            float Width = ScreenWidth / 5;
            float Height = ScreenHeight / 30 * 2;
            //float multiply = ScreenHeight /30/50
            spriteBatch.Draw(texture1, new Rectangle(20, num * 100 + 20, (int)Width, (int) Height), Color.Gray);
            spriteBatch.Draw(currentEnergy, new Rectangle(23, num * 100 + 33,  (int)Width - 6, (int)Height - 60), Color.White);
            spriteBatch.Draw(currentHealth, new Rectangle(23, num * 100 + 40, (int)Width - 6, (int)Height - 40), Color.White);
            spriteBatch.DrawString(spriteFont, unit.Name, new Vector2(20, num * 100 + 20), Color.White);
            spriteBatch.DrawString(spriteFont,"" +unit.Energy, new Vector2(60, num * 100 + 33), Color.White);
        }

    }
}
