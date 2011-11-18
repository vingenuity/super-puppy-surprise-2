using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SpaceHaste.Maps;
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
            int height = Graphics.GraphicsManager.graphics.PreferredBackBufferHeight;
            int widthL = Graphics.GraphicsManager.graphics.PreferredBackBufferWidth / 2;
            int widthR = Graphics.GraphicsManager.graphics.PreferredBackBufferWidth;
             

            //int numMissiles, int lsrDmg, int missDmg, double[move laser shield] eff
            spriteBatch.Draw(backpaneLeft, left, Color.White);
            spriteBatch.DrawString(spriteFont, "Missiles:", new Vector2( 0*widthL / 6 + 15, (height / 20) * 19 + 5 ), Color.White); 
            spriteBatch.DrawString(spriteFont, "" + GameMechanicsManager.GameObjectList[0].MissileCount, new Vector2( 0*widthL / 6 + 15, (height / 20) * 19 + 25), Color.White);
            
            spriteBatch.DrawString(spriteFont, "Mssl Pow:", new Vector2(1*widthL / 6, (height / 20) * 19 + 5), Color.White);
            spriteBatch.DrawString(spriteFont, "" + GameMechanicsManager.GameObjectList[0].dmg[1], new Vector2(1*widthL / 6, (height / 20) * 19 + 25), Color.White);

            spriteBatch.DrawString(spriteFont, "Max Move:", new Vector2(2*widthL / 6, (height / 20) * 19 + 5), Color.White);
            spriteBatch.DrawString(spriteFont, "" + GameMechanicsManager.GameObjectList[0].MovementRange, new Vector2(2*widthL / 6, (height / 20) * 19 + 25), Color.White);

            spriteBatch.DrawString(spriteFont, "Range:", new Vector2(3*widthL / 6, (height / 20) * 19 + 5), Color.White);
            spriteBatch.DrawString(spriteFont, "" + GameMechanicsManager.GameObjectList[0].LaserRange, new Vector2(3*widthL / 6, (height / 20) * 19 + 25), Color.White);

            spriteBatch.DrawString(spriteFont, "E. Regen:", new Vector2(4*widthL / 6 - 10, (height / 20) * 19 + 5), Color.White);
            spriteBatch.DrawString(spriteFont, "" + GameMechanicsManager.GameObjectList[0].regen, new Vector2(4*widthL / 6 - 10, (height / 20) * 19 + 25), Color.White);

            spriteBatch.DrawString(spriteFont, "Dmg Out:", new Vector2(5*widthL / 6, (height / 20) * 19 + 5), Color.White); 
            spriteBatch.DrawString(spriteFont, "" /*+  Damage Output */, new Vector2(5*widthL / 6, (height / 20) * 19 + 25), Color.White);



            spriteBatch.Draw(backpaneRight, right, Color.White);

            if (Map.map.GetCubeAt(GameMechanicsManager.MechMan.BattleManager.getSelectedCube()).GetObject() != null)
            {
                spriteBatch.DrawString(spriteFont, "Missiles:", new Vector2(0 * (widthL / 6) + widthL, (height / 20) * 19 + 5), Color.White);
                spriteBatch.DrawString(spriteFont, "" + Map.map.GetCubeAt(GameMechanicsManager.MechMan.BattleManager.getSelectedCube()).GetObject().MissileCount, new Vector2(0 * widthL / 6 + widthL + 15, (height / 20) * 19 + 25), Color.White);

                spriteBatch.DrawString(spriteFont, "Mssl Pow:", new Vector2((1 * widthL / 6) + widthL, (height / 20) * 19 + 5), Color.White);
                spriteBatch.DrawString(spriteFont, "" + Map.map.GetCubeAt(GameMechanicsManager.MechMan.BattleManager.getSelectedCube()).GetObject().dmg[1], new Vector2(1 * widthL / 6 + widthL, (height / 20) * 19 + 25), Color.White);

                spriteBatch.DrawString(spriteFont, "Max Move:", new Vector2(2 * widthL / 6 + widthL, (height / 20) * 19 + 5), Color.White);
                spriteBatch.DrawString(spriteFont, "" + Map.map.GetCubeAt(GameMechanicsManager.MechMan.BattleManager.getSelectedCube()).GetObject().MovementRange, new Vector2(2 * widthL / 6 + widthL, (height / 20) * 19 + 25), Color.White);

                spriteBatch.DrawString(spriteFont, "Range:", new Vector2(3 * widthL / 6 + widthL, (height / 20) * 19 + 5), Color.White);
                spriteBatch.DrawString(spriteFont, "" + Map.map.GetCubeAt(GameMechanicsManager.MechMan.BattleManager.getSelectedCube()).GetObject().LaserRange, new Vector2(3 * widthL / 6 + widthL, (height / 20) * 19 + 25), Color.White);

                spriteBatch.DrawString(spriteFont, "E. Regen:", new Vector2(4 * widthL / 6 + widthL - 10, (height / 20) * 19 + 5), Color.White);
                spriteBatch.DrawString(spriteFont, "" + Map.map.GetCubeAt(GameMechanicsManager.MechMan.BattleManager.getSelectedCube()).GetObject().regen, new Vector2(4 * widthL / 6 + widthL - 10, (height / 20) * 19 + 25), Color.White);

                spriteBatch.DrawString(spriteFont, "Dmg Out:", new Vector2(5 * widthL / 6 + widthL, (height / 20) * 19 + 5), Color.White);
                spriteBatch.DrawString(spriteFont, "" /*+  Damage Output */, new Vector2(5 * widthL / 6 + widthL, (height / 20) * 19 + 25), Color.White);
            }


        }

    }
}
