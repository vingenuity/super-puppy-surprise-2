using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceHaste.GameMech;
using SpaceHaste.GameMech.BattleMechanicsManagers;

namespace SpaceHaste.Huds
{
    public class DisplayCutScenes
    {
        public bool DisplayCommands;

       
        Texture2D texture1;
        private bool ShowCutSceneText;
        Vector2 TextPosition;

        public DisplayCutScenes()
        {
            TextPosition = new Vector2(10, 600);
        }
        public void Load()
        {
            texture1 = Hud.Content.Load<Texture2D>("UI_backPane_blue");
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
        
            if (!ShowCutSceneText)
                return;

            spriteBatch.Draw(texture1, new Rectangle(10, 600, 150, 500), Color.White);

            spriteBatch.DrawString(spriteFont, "Move", TextPosition, Color.Yellow, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
           
            
        }
        public void Update(GameTime gameTime)
        {
            if (GameMechanicsManager.gamestate == GameState.CutScene)
                ShowCutSceneText = true;
            else
                ShowCutSceneText = false;
           
        }
        public void Attack()
        {
        }
    }
}
