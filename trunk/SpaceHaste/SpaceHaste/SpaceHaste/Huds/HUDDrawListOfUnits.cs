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
        Texture2D texture1, currentHealth, currentEnergy, maxEnergy;
        //float Size;
        //float Size2;
        //int here;


        public HUDDrawListOfUnits()
        {
        }
        public void Load()
        {
            texture1 = Hud.Content.Load<Texture2D>("SketchTexture");
            maxEnergy = Hud.Content.Load<Texture2D>("EnergyBarEmpty");
            currentHealth = Hud.Content.Load<Texture2D>("HealthBar");
            currentEnergy = Hud.Content.Load<Texture2D>("EnergyBar");

        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
         //   DrawGridInformation(GameMechanicsManager., spriteBatch, spriteFont, i, GraphicsManager.graphics.PreferredBackBufferWidth, GraphicsManager.graphics.PreferredBackBufferHeight);
            for (int i = 0; i < GameMechanicsManager.GameObjectList.Count; i++)
                DrawUnitInformation(GameMechanicsManager.GameObjectList[i], spriteBatch, spriteFont, i, GraphicsManager.graphics.PreferredBackBufferWidth, GraphicsManager.graphics.PreferredBackBufferHeight);

        }

        private void DrawGridInformation(GameObject unit, GameObject selected, SpriteBatch spriteBatch, SpriteFont spriteFont, float ScreenWidth, float ScreenHeight){

                    /* Under-Bar */
            spriteBatch.Draw(texture1, new Rectangle(0, (int)(ScreenHeight-((ScreenHeight*5)/100)), (int)ScreenWidth, (int)(ScreenHeight*5)/100), Color.White);
            spriteBatch.DrawString(spriteFont, "HP: " + unit.Health, new Vector2((float)((ScreenWidth * 1) / 100), (float)(ScreenHeight - ((ScreenHeight * 2.5) / 100))), Color.White);

        }

        private void DrawUnitInformation(GameObject unit, SpriteBatch spriteBatch, SpriteFont spriteFont, int num, float ScreenWidth, float ScreenHeight)
        {
           
            float Width = ScreenWidth / 5;
            float Height = ScreenHeight / 30 * 2;
            //float multiply = ScreenHeight /30/50

            /* Unit List */
            spriteBatch.Draw(texture1, new Rectangle(20, num * 100 + 20, (int)Width, (int) Height), Color.Gray);
            spriteBatch.Draw(currentEnergy, new Rectangle(23, num * 100 + 33,  (int)(Width - 6), (int)Height - 60), Color.White);
            spriteBatch.Draw(maxEnergy, new Rectangle((int)((unit.Energy / 100) * (Width - 6) + 26), num * 100 + 33, (int)((Width - 6) - ((unit.Energy / 100) * (Width - 6))-3), (int)Height - 60), Color.White);
            spriteBatch.Draw(currentHealth, new Rectangle(23, num * 100 + 45, (int)Width - 6, (int)Height - 50), Color.White);
            spriteBatch.DrawString(spriteFont, unit.Name, new Vector2(20, num * 100 + 10), Color.White);
            spriteBatch.DrawString(spriteFont,"" +unit.Energy, new Vector2((float)(unit.Energy/100)*(Width-6)+23, num * 100 + 33), Color.White);
            spriteBatch.DrawString(spriteFont, "" + unit.Health + " / " + unit.maxHealth, new Vector2(100, num * 100 + 52), Color.White);

        }

    }
}
