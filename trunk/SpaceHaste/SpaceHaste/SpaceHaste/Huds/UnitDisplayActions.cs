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
        private bool ShowShipActions;
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
            texture1 = Hud.Content.Load<Texture2D>("UI_backPane_blue");
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            if (!ShowShipActions)
                return;

            spriteBatch.Draw(texture1, new Rectangle(350, 20, 150, 150), Color.White);
            if(Selected == 0)
                spriteBatch.DrawString(spriteFont, "Move", MenuMoveStringPosition, Color.Yellow, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
            else if (CanMove)
                spriteBatch.DrawString(spriteFont, "Move", MenuMoveStringPosition, Color.White);
            else
                spriteBatch.DrawString(spriteFont, "Move", MenuMoveStringPosition, Color.Gray);
            if (Selected == 1)
                spriteBatch.DrawString(spriteFont, "Attack", MenuAttackStringPosition, Color.Yellow, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
            else if (CanAttack)
                spriteBatch.DrawString(spriteFont, "Attack", MenuAttackStringPosition, Color.White);
            else
                spriteBatch.DrawString(spriteFont, "Attack", MenuAttackStringPosition, Color.Gray);
            if (Selected == 2)
                spriteBatch.DrawString(spriteFont, "Wait", MenuWaitStringPosition, Color.Yellow, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
            else if (CanWait)
                spriteBatch.DrawString(spriteFont, "Wait", MenuWaitStringPosition, Color.White);
            else
                spriteBatch.DrawString(spriteFont, "Wait", MenuWaitStringPosition, Color.Gray);
            
        }
        public void Update(GameTime gameTime)
        {
            Selected = (int)BattleMechanicsManager.Instance.ShipModeSelection;
            if (BattleMechanicsManager.Instance.gamestate == GameState.SelectShipAction)
                ShowShipActions = true;
            else
                ShowShipActions = false;
            CanMove = BattleMechanicsManager.Instance.MoveEnabled;
            CanAttack = BattleMechanicsManager.Instance.AttackEnabled;
            CanWait = BattleMechanicsManager.Instance.WaitEnabled;
        }
        public void Attack()
        {
        }
    }
}
