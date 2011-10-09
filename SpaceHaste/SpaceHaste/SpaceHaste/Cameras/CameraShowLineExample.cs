using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceHaste.Cameras
{
    public class CameraShowLineExample : Camera
    {
        GraphicsDevice device;
        public CameraShowLineExample(GraphicsDeviceManager graphics)
            : base()
        {
             device = graphics.GraphicsDevice;

            
             float time = 0;

             CameraManager.View = Matrix.CreateLookAt(
                new Vector3(0.0f, 0f, 10000f),
                new Vector3(0.0f, 0.0f, 0.0f),
                Vector3.Up
                );

             CameraManager.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                   device.Viewport.AspectRatio,
                                                                   10, 10000);

            
        }
        float offsetx, offsety, offsetz = 0;
        public override void UpdateView(GameTime gameTime)
        {
            offsety = 0;
            CameraManager.View = Matrix.CreateLookAt(
                new Vector3(0, 0, 10000),
                new Vector3(0, 0, 0),
                Vector3.Up
                );
          //  offsetz--;
        }
        public override void UpdateProjection(GameTime gameTime)
        {

        }
    }
}
