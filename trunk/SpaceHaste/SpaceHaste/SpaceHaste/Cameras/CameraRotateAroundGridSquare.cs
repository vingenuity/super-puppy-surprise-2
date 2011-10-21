using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace SpaceHaste.Cameras
{
    public class CameraRotateAroundGridSquare : Camera
    {
        GraphicsDevice device;

        KeyboardState keyboardState;
        float verticalAngle = .4f;
        float horizontalAngle = 0;
        float zoom = 10;
        float verticalAngleMin = .01f;
        float verticalAngleMax = (float) Math.PI - .01f;
        float zoomMin = 10000;
        float zoomMax = 1000000;
        public CameraRotateAroundGridSquare(GraphicsDeviceManager graphics)
            : base()
        {
            device = graphics.GraphicsDevice;


            float time = 0;

            CameraManager.View = Matrix.CreateLookAt(
               new Vector3(1000f, 1f, 0f),
               new Vector3(0.0f, 0.0f, 0.0f),
               Vector3.Up
               );

            CameraManager.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                  device.Viewport.AspectRatio,
                                                                  10, 100000);
        }
        
        public override void UpdateView(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState(); 
 
            if (keyboardState.IsKeyDown(Keys.W)) 
            { 
                //Add phi 
                verticalAngle -= .01F; 
            } 
            if (keyboardState.IsKeyDown(Keys.A)) 
            { 
                //Subtract theta 
                //new_position.Y -= gameTime.ElapsedRealTime.Seconds; 
                horizontalAngle -= .01F; 
            } 
            if (keyboardState.IsKeyDown(Keys.S)) 
            { 
                //Subtract phi 
                verticalAngle += .01F; 
            } 
            if (keyboardState.IsKeyDown(Keys.D)) 
            { 
                //Add theta 
                //new_position.Y += gameTime.ElapsedRealTime.Seconds; 
                horizontalAngle += .01F; 
            } 
            if (keyboardState.IsKeyDown(Keys.Q)) 
            { 
                //Zoom in 
                zoom -= 20F; 
            } 
            if (keyboardState.IsKeyDown(Keys.E)) 
            { 
                //Zoom out 
                zoom += 20F; 
            }

            verticalAngle = MathHelper.Clamp(verticalAngle, verticalAngleMin, verticalAngleMax); 
 
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
            CameraManager.View = Matrix.CreateLookAt(position,
                                                  targetPosition, 
                                                  Vector3.Up); 
        }
        public override void UpdateProjection(GameTime gameTime)
        {

        }
    }
}
