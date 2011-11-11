using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SpaceHaste.Huds
{
    public class Hud : DrawableGameComponent
    {
        public static SpriteBatch spriteBatch;
        public static SpriteFont spriteFont;
        HUDDrawListOfUnits DrawUnitLists;
        UnitDisplayActions UnitActions;
        GraphicsDeviceManager graphics;
        public static ContentManager Content;
        DisplayCutScenes scene;

        public Hud(Game game, GraphicsDeviceManager graphics) : base (game)
        {
            this.graphics = graphics;
            
            DrawUnitLists = new HUDDrawListOfUnits();
            UnitActions = new UnitDisplayActions();
            Content = new ContentManager(game.Services);
            Content.RootDirectory = "Content";

            scene = new DisplayCutScenes();

        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            spriteFont = Hud.Content.Load<SpriteFont>("hudFont");
            DrawUnitLists.Load();
            UnitActions.Load();
        }
        public override void Update(GameTime gameTime)
        {
            UnitActions.Update(gameTime);
        }

        public void showCutscene(GameTime gameTime) {
            spriteBatch.Begin();
            scene.Draw();
            spriteBatch.End();
                
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
                
                DrawUnitLists.Draw(gameTime,spriteBatch, spriteFont);

                UnitActions.Draw(gameTime, spriteBatch, spriteFont);
            spriteBatch.End();
            
            scene.Draw();


        }
    }
}
