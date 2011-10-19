using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SpaceHaste.Graphics;
using SpaceHaste.Cameras;
using SpaceHaste.Grids;
using SpaceHaste.Primitives;
using SpaceHaste.Maps;
using SpaceHaste.Huds;
using SpaceHaste.GameMech;

namespace SpaceHaste
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Camera Camera;
        double seconds = 1000;
        public double Hours
        {
            get { return seconds / 3600; }
            set { seconds = value * 3600; }
        }


        public double Hours2;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

     //       if (!graphics.IsFullScreen)
             //   graphics.ToggleFullScreen();
            graphics.ApplyChanges();

            // TODO: Add your initialization logic here
            GraphicsManager GraphicsManager = new GraphicsManager(this, graphics);
            Components.Add(GraphicsManager);

            GameMechanicsManager GameMechanicsManager = new GameMechanicsManager(this);
            Components.Add(GameMechanicsManager);

            Hud HudManager = new Hud(this, graphics);
            Components.Add(HudManager);

            CameraManager CameraManager = new CameraManager(this, graphics);
            Components.Add(CameraManager);

            PrimitiveManager PrimitiveManager = new PrimitiveManager(this, graphics);
            Components.Add(PrimitiveManager);

            LineManager LineManager = new LineManager(this, graphics);
            Components.Add(LineManager);

            MapManager MapManager = new MapManager(this);
            Components.Add(MapManager);

           

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
