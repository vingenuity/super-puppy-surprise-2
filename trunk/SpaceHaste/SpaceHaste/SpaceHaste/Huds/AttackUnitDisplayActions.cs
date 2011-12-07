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

    public class AttackUnitDisplayActions
    {
        public bool DisplayCommands;
        public bool CanMissiles;
        public bool CanLasers;
        public bool CanTargetLasers;
        public bool CanDrainEnergy;
        int Selected = 0;
        Vector2 MenuAttackStringPosition;
        Vector2 MenuMoveStringPosition;
        Vector2 MenuWaitStringPosition;
        Vector2 MenutargetWeaponStringPosition;
        Texture2D texture1;
        private bool ShowShipAttackActions;
        public AttackUnitDisplayActions()
        {
            MenuMoveStringPosition = new Vector2(560, 29);
            MenuAttackStringPosition = MenuMoveStringPosition;
            MenuAttackStringPosition.Y += 35;
            MenuWaitStringPosition = MenuAttackStringPosition;
            MenuWaitStringPosition.Y += 35;
            MenutargetWeaponStringPosition = MenuWaitStringPosition;
            MenutargetWeaponStringPosition.Y += 35;
        }
        public void Load()
        {
            texture1 = Hud.Content.Load<Texture2D>("UI_backPane_blue");
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont spriteFont)
        {

            if (!ShowShipAttackActions)
                return;

            spriteBatch.Draw(texture1, new Rectangle(550, 20, 150, 150), Color.White);
            if (Selected == 0)
                spriteBatch.DrawString(spriteFont, "Laser", MenuMoveStringPosition, Color.Yellow, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
            else if (!CanLasers)
                spriteBatch.DrawString(spriteFont, "Laser", MenuMoveStringPosition, Color.Gray);
            else
                spriteBatch.DrawString(spriteFont, "Laser", MenuMoveStringPosition, Color.White);
           // else
           //     spriteBatch.DrawString(spriteFont, "Laser", MenuMoveStringPosition, Color.Gray);
            if (Selected == 1)
                spriteBatch.DrawString(spriteFont, "Missile", MenuAttackStringPosition, Color.Yellow, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
           // else if (CanAttack)
            else if (!CanMissiles)
                spriteBatch.DrawString(spriteFont, "Missile", MenuAttackStringPosition, Color.Gray);
            else
                spriteBatch.DrawString(spriteFont, "Missile", MenuAttackStringPosition, Color.White);
            if (Selected == 2)
                spriteBatch.DrawString(spriteFont, "Drain Energy", MenuWaitStringPosition, Color.Yellow, 0, Vector2.Zero, 1.1f, SpriteEffects.None, 0);
            else if (!CanDrainEnergy)
                spriteBatch.DrawString(spriteFont, "Drain Energy", MenuWaitStringPosition, Color.Gray);
            else
                spriteBatch.DrawString(spriteFont, "Drain Energy", MenuWaitStringPosition, Color.White);
          //  else
          //      spriteBatch.DrawString(spriteFont, "Missile", MenuAttackStringPosition, Color.Gray);
        }

        public void Update(GameTime gameTime)
        {
            Selected = (int)BattleMechanicsManager.Instance.ShipAttackModeSelection;
            if (GameMechanicsManager.gamestate == GameState.SelectShipAttackAction)
                ShowShipAttackActions = true;
            else
                ShowShipAttackActions = false;
            CanLasers = BattleMechanicsManager.Instance.AttackLasers;
            CanMissiles = BattleMechanicsManager.Instance.AttackMissiles;
            CanDrainEnergy = BattleMechanicsManager.Instance.AttackDrainEnergy;
        }
        public void Attack()
        {
        }
    }
}
