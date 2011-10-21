using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceHaste.Cameras
{
    public class CameraManager : GameComponent, CameraControls
    {
        public static CameraManager cameraManager;

        public static Matrix View;
        public static Matrix Projection;
        public static Camera Camera;
        bool Disabled;
        GraphicsDeviceManager graphics;
        int CameraNum = 0;

        CameraControls CameraControls;

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
                Camera = new CameraRotateAroundGridSquare(graphics);
            if (CameraNum == 1)
                Camera = new CameraSideView(graphics);
            if (CameraNum == 2)
                Camera = new CameraBackView(graphics);
            if (CameraNum == 3)
                Camera = new CameraViewModel(graphics);
        }
        void CameraControls.ChangeCamera()
        {
            CameraNum = (CameraNum + 1) % 4;
            ChangeCamera();
        }
    }
}