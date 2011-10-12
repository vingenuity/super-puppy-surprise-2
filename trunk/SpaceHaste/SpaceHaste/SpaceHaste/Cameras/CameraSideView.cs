using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceHaste.Cameras
{
    public class CameraSideView : Camera
    {
        GraphicsDevice device;
        public CameraSideView(GraphicsDeviceManager graphics)
            : base()
        {
            device = graphics.GraphicsDevice;


            float time = 0;

            CameraManager.View = Matrix.CreateLookAt(
               new Vector3(10000f, 0f, 0f),
               new Vector3(0.0f, 0.0f, 0.0f),
               Vector3.Up
               );

            CameraManager.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                  device.Viewport.AspectRatio,
                                                                  10, 10000);


        }
        public override void UpdateView(GameTime gameTime)
        {
        }
        public override void UpdateProjection(GameTime gameTime)
        {

        }
    }
}
