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
        public static CameraManager cameraManager;

        public static Matrix View;
        public static Matrix Projection;
        public static Camera Camera;
        bool Disabled;
        GraphicsDeviceManager graphics;
        int CameraNum = 0;

        public CameraManager(Game game, GraphicsDeviceManager graphics)
            : base(game)
        {
            cameraManager = this;
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
            base.Update(gameTime);
        }
        void ChangeCamera()
        {
            if(CameraNum == 0)
                Camera = new RotationCamera(graphics);
        }
    }
}