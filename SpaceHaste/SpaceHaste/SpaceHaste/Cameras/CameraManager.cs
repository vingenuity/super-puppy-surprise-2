using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceHaste.Cameras
{
    public class CameraManager : GameComponent
    {
        public static Matrix View;
        public static Matrix Projection;
        public static Camera Camera;
        bool Disabled;
        GraphicsDeviceManager graphics;
        int CameraNum = 0;
        public CameraManager(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            //Camera = new CameraViewModel(graphics);
            //Camera = new CameraShowLineExample(graphics);
            this.graphics = graphics;
            ChangeCamera();
            Disabled = false;
        }
        public override void Update(GameTime gameTime)
        {
            
            Camera.UpdateView(gameTime);
            Camera.UpdateProjection(gameTime);
            HandleInput();
            base.Update(gameTime);
        }
        void ChangeCamera()
        {
            if(CameraNum == 0)
                Camera = new CameraShowLineExample(graphics);
            if (CameraNum == 1)
                Camera = new CameraViewModel(graphics);
        }
        KeyboardState currentKeyboardState;
        KeyboardState lastKeyboardState;
        void HandleInput()
        {
            lastKeyboardState = currentKeyboardState;
            

            currentKeyboardState = Keyboard.GetState();



            // Switch cameras
            if (currentKeyboardState.IsKeyDown(Keys.B) &&
                 lastKeyboardState.IsKeyUp(Keys.B))
            {
                CameraNum = (CameraNum + 1) % 2;
                ChangeCamera();
            }
        }
    }
}