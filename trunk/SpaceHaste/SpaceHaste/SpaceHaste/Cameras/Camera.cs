using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceHaste.Cameras
{
    public class Camera
    {
        public Matrix View;
        public Matrix Projection;
        public Camera()
        {

        }
        public virtual void UpdateView(GameTime gameTime)
        {
        }
        public virtual void UpdateProjection(GameTime gameTime)
        {
        }
    }
}
