using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SpaceHaste.Huds
{
    public class Hud : GameComponent
    {
        public static Hud Instance;

        public static SpriteBatch spriteBatch;
        public static SpriteFont spriteFont;
        HUDDrawListOfUnits DrawUnitLists;
        UnitDisplayActions UnitActions;
        AttackUnitDisplayActions AttackUnitActions;
        StatsBar Stats;
        GraphicsDeviceManager graphics;
        public static ContentManager Content;
        DisplayCutScenes scene;

        public Hud(Game game, GraphicsDeviceManager graphics) : base (game)
        {
           
            this.graphics = graphics;
            
            DrawUnitLists = new HUDDrawListOfUnits();
            UnitActions = new UnitDisplayActions();
            AttackUnitActions = new AttackUnitDisplayActions();
            Stats = new StatsBar();
            Content = new ContentManager(game.Services);
            Content.RootDirectory = "Content";

            scene = new DisplayCutScenes();
            Instance = this;
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            spriteFont = Hud.Content.Load<SpriteFont>("hudFont");
            DrawUnitLists.Load();
            UnitActions.Load();
            AttackUnitActions.Load();
        }
        public override void Update(GameTime gameTime)
        {
            UnitActions.Update(gameTime);
            AttackUnitActions.Update(gameTime);
        }

        public void showCutscene(GameTime gameTime) {
            spriteBatch.Begin();
            scene.Draw();
            spriteBatch.End();
                
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

                Stats.Draw(gameTime, spriteBatch, spriteFont);

                DrawUnitLists.Draw(gameTime,spriteBatch, spriteFont);

                UnitActions.Draw(gameTime, spriteBatch, spriteFont);
                AttackUnitActions.Draw(gameTime, spriteBatch, spriteFont);
            spriteBatch.End();
            
            scene.Draw();


        }
    }
}
