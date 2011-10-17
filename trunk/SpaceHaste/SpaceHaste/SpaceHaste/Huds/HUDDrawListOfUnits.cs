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
        Texture2D texture1;

        public HUDDrawListOfUnits()
        {
        }
        public void Load()
        {
            texture1 = Hud.Content.Load<Texture2D>("SketchTexture");
        }
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < GameMechanicsManager.MoveableSceneGameObjectList.Count; i++)
                DrawUnitInformation(GameMechanicsManager.MoveableSceneGameObjectList[i], spriteBatch, i, GraphicsManager.graphics.PreferredBackBufferWidth, GraphicsManager.graphics.PreferredBackBufferHeight);
        }

        private void DrawUnitInformation(GameObject unit, SpriteBatch spriteBatch,int num, float ScreenWidth, float ScreenHeight)
        {
            float Width = ScreenWidth / 10;
            float Height = ScreenHeight / 30;
            //float multiply = ScreenHeight /30/50
            spriteBatch.Draw(texture1, new Rectangle(0, num * 50, (int)Width, (int) Height), Color.White);
        }

    }
}
