using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceHaste.Controls;
using System;

namespace SpaceHaste.Cameras
{
    public class RotationCamera : Camera
    {
        GraphicsDevice device;

        float verticalAngle = 1.0f;
        float horizontalAngle = 0;
        float zoom = 10;
        float verticalAngleMin = .01f;
        float verticalAngleMax = (float) Math.PI - .01f;
        float zoomMin = 5000;
        float zoomMax = 1000000;
        public RotationCamera(GraphicsDeviceManager graphics)
            : base()
        {
            device = graphics.GraphicsDevice;


            //float time = 0;

            ControlManager.View = Matrix.CreateLookAt(
               new Vector3(1000f, 1f, 0f),
               new Vector3(0.0f, 0.0f, 0.0f),
               Vector3.Up
               );

            ControlManager.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                  device.Viewport.AspectRatio,
                                                                  10, 100000);

            
        }

        internal void MoveCameraUp()
        {
            verticalAngle -= .01F; 
        }
        internal void MoveCameraDown()
        {
            verticalAngle += .01F; 
        }
        internal void MoveCameraLeft()
        {
            horizontalAngle -= .01F;
        }
        internal void MoveCameraRight()
        {
            horizontalAngle += .01F;
        }
        internal void AnalogMove(Vector2 stick)
        {
            horizontalAngle += .01F * stick.X;
            verticalAngle   += .01F * stick.Y;
        }

        internal void ZoomIn()
        {
            zoom -= 20F; 
        }
        internal void ZoomOut()
        {
            zoom += 20F; 
        }

        /// <summary>
        /// Keeps horizontalAngle keeps between 0 and 2 PI
        /// Keeps verticalAngle between verticalAngleMin and verticalAngleMax
        /// </summary>
        /// <param name="gameTime"></param>
        public override void UpdateView(GameTime gameTime)
        {
            // Keep vertical angle within tolerances 
            verticalAngle = MathHelper.Clamp(verticalAngle, verticalAngleMin, verticalAngleMax); 
 
            // Keep vertical angle within PI 
            if (horizontalAngle > MathHelper.TwoPi) 
                horizontalAngle -= MathHelper.TwoPi; 
            else if (horizontalAngle < 0.0f) 
                horizontalAngle += MathHelper.TwoPi; 
 
            // Keep zoom within range 
            zoom = MathHelper.Clamp(zoom, zoomMin, zoomMax); 
 
            // Start with an initial offset 
            Vector3 cameraPosition = new Vector3(0.0f, zoom, 0.0f); 
 
            // Rotate vertically 
            cameraPosition = Vector3.Transform(cameraPosition, Matrix.CreateRotationX(verticalAngle)); 
 
            // Rotate horizontally 
            cameraPosition = Vector3.Transform(cameraPosition, Matrix.CreateRotationY(horizontalAngle)); 
 
            Vector3 targetPosition = Vector3.Zero; 
 
            Vector3 position = cameraPosition + targetPosition; 
           
 
            // Compute view matrix 
            ControlManager.View = Matrix.CreateLookAt(position, targetPosition, Vector3.Up); 
        }

        public float getVerticalAngle() { return verticalAngle; }
        public float getHorizontalAngle() { return horizontalAngle; }

        public override void UpdateProjection(GameTime gameTime)
        {

        }
    }
}
