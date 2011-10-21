using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using SpaceHaste.Cameras;

namespace SpaceHaste.Controls
{
    public class ControlManager : GameComponent
    {
        CameraControls CameraControls;
        bool CameraControlsLoaded = false;

        KeyboardState thisKeyState;
        KeyboardState lastKeyState;

        public ControlManager(Game game)
            : base(game)
        {
            thisKeyState = Keyboard.GetState();
            lastKeyState = Keyboard.GetState();
            LoadCameraControls();
        }
        void LoadCameraControls()
        {
            try
            {
                CameraControls = CameraManager.cameraManager;
                CameraControlsLoaded = true;
            }
            catch
            {
            }
        }
        public override void Update(GameTime gameTime)
        {
            CheckCameraControls(gameTime);

            UpdateKeyboardStates();

            base.Update(gameTime);
        }

        private void UpdateKeyboardStates()
        {
            lastKeyState = thisKeyState;
            thisKeyState = Keyboard.GetState();
        }

        private void CheckCameraControls(GameTime gameTime)
        {
            if (!CameraControlsLoaded)
                return;
            if (KeyPushed(Keys.B))
                CameraControls.ChangeCamera();
        }

        private bool KeyPushed(Keys keys)
        {
            return thisKeyState.IsKeyDown(keys) && lastKeyState.IsKeyUp(keys);
        }
    }
}