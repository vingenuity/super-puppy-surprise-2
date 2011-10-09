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
                new Vector3(0.0f, 0.0f, 1000.0f),
                new Vector3(0.0f, 0.0f, 0.0f),
                Vector3.Up
                );

             CameraManager.Projection = Matrix.CreateOrthographicOffCenter(
                             0,
                             (float)device.Viewport.Width,
                             (float)device.Viewport.Height,
                             0,
                             1.0f, 100000.0f);

            
        }
        float offsetx, offsety, offsetz = 0;
        public override void UpdateView(GameTime gameTime)
        {
            offsety = -30;
            CameraManager.View = Matrix.CreateLookAt(
                new Vector3(offsetx, offsety, 1000.0f+offsetz),
                new Vector3(offsetx, offsety, 0),
                Vector3.Up
                );
          //  offsetz--;
        }
        public override void UpdateProjection(GameTime gameTime)
        {

        }
    }
}
