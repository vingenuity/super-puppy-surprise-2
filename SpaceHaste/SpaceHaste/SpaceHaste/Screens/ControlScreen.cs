using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameStateManagement
{
    class ControlScreen : MenuScreen
    {
        bool keyboardOn;
        ContentManager content;
        Texture2D GPPic;
        Texture2D keyPic;

        public ControlScreen()
            : base("Controls")
        {
            keyboardOn = true;

            MenuEntry keyboard = new MenuEntry("Keyboard");
            keyboard.Selected += keyboardSelected;

            MenuEntries.Add(keyboard);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            keyPic = content.Load<Texture2D>("keyMaps");
            GPPic = content.Load<Texture2D>("PadMapComp");
        }

        void keyboardSelected(object sender, PlayerIndexEventArgs e)
        {
            keyboardOn = false;
            MenuEntries.Clear();
            MenuEntry gamepad = new MenuEntry("Gamepad");
            gamepad.Selected += gamepadSelected;
            MenuEntries.Add(gamepad);

        }

        void gamepadSelected(object sender, PlayerIndexEventArgs e)
        {
            keyboardOn = true;
            MenuEntries.Clear();
            MenuEntry keyboard = new MenuEntry("Keyboard");
            keyboard.Selected += keyboardSelected;
            MenuEntries.Add(keyboard);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            base.OnCancel(playerIndex);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle size; 

            spriteBatch.Begin();

            if (keyboardOn)
            {
                size = new Rectangle((viewport.Width / 2) - (keyPic.Width / 2), (viewport.Height / 2) - (keyPic.Height / 2),
                                        keyPic.Width, keyPic.Height);
                spriteBatch.Draw(keyPic, size, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            }
            else
            {
                size = new Rectangle((viewport.Width / 2) - (GPPic.Width / 2), (viewport.Height / 2) - 200,
                                        GPPic.Width, GPPic.Height);
                spriteBatch.Draw(GPPic, size, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
