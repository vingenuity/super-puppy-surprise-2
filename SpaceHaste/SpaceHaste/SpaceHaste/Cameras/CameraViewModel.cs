using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceHaste.Cameras
{
    public class CameraViewModel : Camera
    {
        GraphicsDevice device;
        public CameraViewModel(GraphicsDeviceManager graphics) : base()
        {
             device = graphics.GraphicsDevice;

            
             float time = 0;

             Matrix rotation = Matrix.CreateRotationY(time * 0.5f);

             Matrix view = Matrix.CreateLookAt(new Vector3(3000, 1500, 0),
                                              Vector3.Zero,
                                              Vector3.Up);
             Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                    device.Viewport.AspectRatio,
                                                                    1000, 10000);
             CameraManager.Projection = projection;
             CameraManager.View = view;
            
        }
        public void UpdateView(GameTime gameTime)
        {
        }
        public void UpdateProjection(GameTime gameTime)
        {
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                   device.Viewport.AspectRatio,
                                                                   1000, 10000);
            CameraManager.Projection = projection;
        }
    }
}
