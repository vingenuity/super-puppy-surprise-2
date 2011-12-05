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
using SpaceHaste.Controls;
using SpaceHaste.Sounds;
using GameStateManagement;
using SpaceHaste.GameMech.CutScenes;
using SpaceHaste.DPSFParticles;
using SpaceHaste.ClearScreen;
using AvatarElementalBash.SaveLoad;

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
        public static Game1 game;
        public static bool USEMENUS = true;
        public ScreenManager ScreenManager;
        public static CutScene CutScene;
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
            game = this;
            gameComponents = new List<GameComponent>();
            LoadSaveManager.Init();
        }
        List<GameComponent> gameComponents;
        public void LoadGameComponents()
        {

            ExitGameComponents();
            gameComponents = new List<GameComponent>(); 
            // TODO: Add your initialization logic here
            GraphicsManager GraphicsManager = new GraphicsManager(this, graphics);
            Components.Add(GraphicsManager);
            gameComponents.Add(GraphicsManager);

            SoundManager SoundManager = new SoundManager(this);
            Components.Add(SoundManager);
            SoundManager.Load();
            gameComponents.Add(SoundManager);

            Hud HudManager = new Hud(this, graphics);
            Components.Add(HudManager);
            gameComponents.Add(HudManager);

            ClearScreenManager ClearScreenManager = new ClearScreenManager(this, graphics);
            Components.Add(ClearScreenManager);
            gameComponents.Add(ClearScreenManager);

            ParticleManager ParticleManager = new ParticleManager(this);
            Components.Add(ParticleManager);
            gameComponents.Add(ParticleManager);

            GameMechanicsManager GameMechanicsManager = new GameMechanicsManager(this);
            Components.Add(GameMechanicsManager);
            gameComponents.Add(GameMechanicsManager);
            
            PrimitiveManager PrimitiveManager = new PrimitiveManager(this, graphics);
            Components.Add(PrimitiveManager);
            gameComponents.Add(PrimitiveManager);

            LineManager LineManager = new LineManager(this, graphics);
            Components.Add(LineManager);
            gameComponents.Add(LineManager);

            QuadManager QuadManager = new QuadManager(this, graphics);
            Components.Add(QuadManager);
            gameComponents.Add(QuadManager);

            MapManager MapManager = new MapManager(this);
            Components.Add(MapManager);
            gameComponents.Add(MapManager);

            ControlManager ControlManager = new ControlManager(this, graphics);
            Components.Add(ControlManager);
            gameComponents.Add(ControlManager);

            DrawSkyDomeManager SkyDomeManager = new DrawSkyDomeManager(this, graphics)
            Components.Add(SkyDomeManager);
            gameComponents.Add(SkyDomeManager);
        }
       
        public void ExitGameComponents()
        {
            GameMechanicsManager.gamestate = GameState.Exiting;
           while (gameComponents.Count > 0)
            {
                Components.Remove(gameComponents[0]);
                gameComponents.Remove(gameComponents[0]);
            }
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
            //if (!graphics.IsFullScreen)
                //   graphics.ToggleFullScreen();
            graphics.ApplyChanges();

            if (USEMENUS)
            {
                ScreenManager = new ScreenManager(this);
                ScreenManager.DrawOrder = 100;
                Components.Add(ScreenManager);
            }
            else
                LoadGameComponents();
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
            if (USEMENUS)
            {
                ScreenManager.AddScreen(new BackgroundScreen(), null);
                ScreenManager.AddScreen(new MainMenuScreen(), null);
            }
            
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
            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
           // GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
