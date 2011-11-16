using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
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
             

            //int numMissiles, int lsrDmg, int missDmg, double[move laser shield] eff
            spriteBatch.Draw(backpaneLeft, left, Color.White);
            spriteBatch.DrawString(spriteFont, "Missiles:", new Vector2( 0*widthL / 6 + 15, (height / 20) * 19 + 5 ), Color.White); 
            spriteBatch.DrawString(spriteFont, "" + GameMechanicsManager.GameObjectList[0].MissileCount, new Vector2( 0*widthL / 6 + 15, (height / 20) * 19 + 25), Color.White);

            spriteBatch.DrawString(spriteFont, "Lasers:", new Vector2(1*widthL / 6, (height / 20) * 19 + 5), Color.White); 
            spriteBatch.DrawString(spriteFont, "" + GameMechanicsManager.GameObjectList[0].dmg[0], new Vector2(1*widthL / 6, (height / 20) * 19 + 25), Color.White);
            
            spriteBatch.DrawString(spriteFont, "Munitions:", new Vector2(2*widthL / 6 - 10, (height / 20) * 19 + 5), Color.White);
            spriteBatch.DrawString(spriteFont, "" + GameMechanicsManager.GameObjectList[0].dmg[1], new Vector2(2*widthL / 6 - 10, (height / 20) * 19 + 25), Color.White);

            spriteBatch.DrawString(spriteFont, "Move Cost:", new Vector2(3*widthL / 6 - 10, (height / 20) * 19 + 5), Color.White);
            spriteBatch.DrawString(spriteFont, "" + GameMechanicsManager.GameObjectList[0].efficiency[0], new Vector2(3*widthL / 6 - 10, (height / 20) * 19 + 25), Color.White);

            spriteBatch.DrawString(spriteFont, "Laser Cost:", new Vector2(4*widthL / 6 - 10, (height / 20) * 19 + 5), Color.White);
            spriteBatch.DrawString(spriteFont, "" + GameMechanicsManager.GameObjectList[0].efficiency[1], new Vector2(4*widthL / 6 - 10, (height / 20) * 19 + 25), Color.White);

            spriteBatch.DrawString(spriteFont, "Shield Cost:", new Vector2(5*widthL / 6 - 10, (height / 20) * 19 + 5), Color.White);
            spriteBatch.DrawString(spriteFont, "" + GameMechanicsManager.GameObjectList[0].efficiency[2], new Vector2(5*widthL / 6 - 10, (height / 20) * 19 + 25), Color.White);


            spriteBatch.Draw(backpaneRight, right, Color.White);

        }

    }
}
