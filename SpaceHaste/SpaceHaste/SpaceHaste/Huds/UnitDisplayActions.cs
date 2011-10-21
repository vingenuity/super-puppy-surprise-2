﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceHaste.Huds
{
    
    public class UnitDisplayActions
    {
        public bool DisplayCommands;
        public bool CanAttack;
        public bool CanWait;
        public bool CanMove;
        int Selected = 0;
        Vector2 MenuAttackStringPosition;
        Vector2 MenuMoveStringPosition;
        Vector2 MenuWaitStringPosition;
        Texture2D texture1 ;
        public UnitDisplayActions()
        {
            MenuMoveStringPosition = new Vector2(360, 40);
            MenuAttackStringPosition = MenuMoveStringPosition;
            MenuAttackStringPosition.Y += 40;
            MenuWaitStringPosition = MenuAttackStringPosition;
            MenuWaitStringPosition.Y += 40;
        }
        public void Load()
        {
            texture1 = Hud.Content.Load<Texture2D>("SketchTexture");
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Draw(texture1, new Rectangle(350, 20, 150, 150), Color.White);
            if(Selected == 0)
                spriteBatch.DrawString(spriteFont, "Move", MenuMoveStringPosition, Color.Yellow, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
            else
                spriteBatch.DrawString(spriteFont, "Move", MenuMoveStringPosition, Color.White);
            if (Selected == 1)
                spriteBatch.DrawString(spriteFont, "Attack", MenuAttackStringPosition, Color.Yellow, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
            else
                spriteBatch.DrawString(spriteFont, "Attack", MenuAttackStringPosition, Color.White);
            if (Selected == 2)
                spriteBatch.DrawString(spriteFont, "Wait", MenuWaitStringPosition, Color.Yellow, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
            else
                spriteBatch.DrawString(spriteFont, "Wait", MenuWaitStringPosition, Color.White);
            
        }
        public void Update(GameTime gameTime)
        {
        }
        public void Attack()
        {
        }
    }
}
