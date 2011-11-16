using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DPSF;

namespace SuperPuppySurprise.DPSFParticles
{
    public class ParticleManager
    {
        ParticleSystemManager particleSystemManager;
        public List<Particle> ParticleList;
        IDPSFParticleSystem mcCurrentParticleSystem;
        Vector3 CameraPosition;
        Matrix View;
        Matrix Projection;
        public ParticleManager()
        {
            particleSystemManager = new ParticleSystemManager();
            ParticleList = new List<Particle>();
            CameraPosition = new Vector3(0,0,-200);
            View = Matrix.CreateLookAt(CameraPosition, new Vector3(0, 0, 0), Vector3.Up);
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Game1.ScreenWidth / (float)Game1.ScreenHeight, 1, 10000);
           // CameraPosition = new Vector3(0, 50, 200);
            //orthographic not ready yet
           /* 
            View = new Matrix(
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, -1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, -1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);
            Projection = Matrix.CreateOrthographicOffCenter(
                0, Game1.ScreenWidth, -Game1.ScreenHeight, 0, 0, 1);*/
           // View = Matrix.CreateLookAt(CameraPosition, new Vector3(0, 50, 0), Vector3.Up);
            //Matrix.
            // Setup the Camera's Projection matrix by specifying the field of view (1/4 pi), aspect ratio, and the near and far clipping planes
            //Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, (float)Game1.ScreenWidth / (float)Game1.ScreenHeight, 1, 10000);
            /*View = new Matrix(
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, -1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, -1.0f, 0.0f,
                0.0f, 0.0f, 0.0f, 1.0f);*/
           // Projection = Matrix.CreateOrthographic(500, 500, 1, 100);
          //  Projection = Matrix.creat

           
            particleSystemManager.UpdatesPerSecond = 0;
        }
        public void Add(Particle p)
        {
            ParticleList.Add(p);
            particleSystemManager.AddParticleSystem(p.ParticleSystem);
        }
        public void Remove(Particle p)
        {
            ParticleList.Remove(p);
            particleSystemManager.RemoveParticleSystem(p.ParticleSystem);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < ParticleList.Count; i++)
            {
                ParticleList[i].Update(gameTime);
            }
            particleSystemManager.SetCameraPositionForAllParticleSystems(CameraPosition);

            particleSystemManager.UpdateAllParticleSystems((float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        public void Draw()
        {
            particleSystemManager.SetWorldViewProjectionMatricesForAllParticleSystems(Matrix.Identity, View, Projection);

            // Draw the Particle Systems manually
            particleSystemManager.DrawAllParticleSystems();
        }
        public static Vector3 To3D(Vector2 vector)
        {
            float x, y, z;
            z = 0;
            y = (vector.Y / Game1.ScreenHeight);
            y = 1 - y;
            y -= .5f;
            y *= 160;
         
           
            x = (vector.X / Game1.ScreenWidth);
            x = 1 - x;
            x -= .5f;
            x *= 250;
     
            return new Vector3(x,y,z);
        }
        public static Vector3 ToVector3(Vector2 vector)
        {
            return new Vector3(vector.X, vector.Y, -10);
        }
    }
}
