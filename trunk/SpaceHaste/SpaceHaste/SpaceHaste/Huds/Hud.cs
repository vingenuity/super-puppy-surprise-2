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
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        HUDDrawListOfUnits DrawUnitLists;
        UnitDisplayActions UnitActions;
        GraphicsDeviceManager graphics;
        public static ContentManager Content;

        public Hud(Game game, GraphicsDeviceManager graphics) : base (game)
        {
            this.graphics = graphics;
            
            DrawUnitLists = new HUDDrawListOfUnits();
            UnitActions = new UnitDisplayActions();
            Content = new ContentManager(game.Services);
            Content.RootDirectory = "Content";
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
            //DrawUnitLists.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
                DrawUnitLists.Draw(gameTime,spriteBatch, spriteFont);
                UnitActions.Draw(gameTime, spriteBatch, spriteFont);
            spriteBatch.End();
        }
    }
}
