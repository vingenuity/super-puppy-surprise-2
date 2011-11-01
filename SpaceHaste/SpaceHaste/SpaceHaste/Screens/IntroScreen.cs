using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperPuppySurprise;
using Microsoft.Xna.Framework.Input;
namespace GameStateManagement
    {
        /// <summary>
        /// This screen implements the actual game logic. It is just a
        /// placeholder to get the idea across: you'll probably want to
        /// put some more interesting gameplay in here!
        /// </summary>
        class IntroScreen : GameScreen
        {
            public class SmartText
            {
                public List<string> list;
                public double Seconds;
                public Side side;
                public enum Side
                {
                    Left,
                    Right,
                    Center,
                };
                public SmartText(List<string> List, double seconds, Side side)
                {
                    list = List;
                    Seconds = seconds;
                    this.side = side;
                }

            }
            #region Fields


            SpriteFont gameFont;

            double currentTime;
            int textNumber;

            float pauseAlpha;

            #endregion

            #region Initialization
            public static bool loaded = false;

            /// <summary>
            /// Constructor.
            /// </summary>
            public IntroScreen()
            {
            }
            #endregion
            Texture2D text;
            /// <summary>
            /// Load graphics content for the game.
            /// </summary>
            public override void LoadContent()
            {

             

                text = Game1.game.Content.Load<Texture2D>("asset_storyBoard");
                Game1.spriteBatch = new SpriteBatch(Game1.game.GraphicsDevice);
            }


            /// <summary>
            /// Unload graphics content used by the game.
            /// </summary>
            public override void UnloadContent()
            {
               
            }

            #region Update and Draw

            double time=5;
            /// <summary>
            /// Updates the state of the game. This method checks the GameScreen.IsActive
            /// property, so the game will stop updating when the pause menu is active,
            /// or if you tab away to a different application.
            /// </summary>
            public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                           bool coveredByOtherScreen)
            {
                base.Update(gameTime, otherScreenHasFocus, false);

                time -= gameTime.ElapsedGameTime.TotalSeconds;


                keyboardState = Keyboard.GetState();
                gamePadState = GamePad.GetState(PlayerIndex.One);

                if (keyboardState.IsKeyDown(Keys.Enter) || gamePadState.IsButtonDown(Buttons.Start) || gamePadState.IsButtonDown(Buttons.A) || time < 0)
                {
                    //Game1.gameplayScreen = new GameplayScreen();
                    //LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, Game1.gameplayScreen);
                    LoadingScreen.Load(ScreenManager, true, PlayerIndex.One, new GameplayScreen());
                }

            }
            KeyboardState keyboardState; GamePadState gamePadState;

            /// <summary>
            /// Lets the game respond to player input. Unlike the Update method,
            /// this will only be called when the gameplay screen is active.
            /// </summary>
            public override void HandleInput(InputState input)
            {

               

                // Look up inputs for the active player profile.
                int playerIndex = (int)ControllingPlayer.Value;

                keyboardState = input.CurrentKeyboardStates[playerIndex];
                gamePadState = input.CurrentGamePadStates[playerIndex];

                // The game pauses either if the user presses the pause button, or if
                // they unplug the active gamepad. This requires us to keep track of
                // whether a gamepad was ever plugged in, because we don't want to pause
                // on PC if they are playing with a keyboard and have no gamepad at all!
                bool gamePadDisconnected = !gamePadState.IsConnected &&
                                           input.GamePadWasConnected[playerIndex];

                if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
                {
                    ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
                }
                else
                {

                }
            }

            Texture2D UN, Virus, Ninja, Ninja1, Money, Comic;
            /// <summary>
            /// Draws the gameplay screen.
            /// </summary>
            public override void Draw(GameTime gameTime)
            {
                // This game has a blue background. Why? Because!
                ScreenManager.GraphicsDevice.Clear(Color.Black);
                Game1.spriteBatch.Begin();
                Game1.spriteBatch.Draw(text, new Rectangle(0, 0,750,500), Color.White);
                Game1.spriteBatch.End();

            }
            #endregion
        }
    }
