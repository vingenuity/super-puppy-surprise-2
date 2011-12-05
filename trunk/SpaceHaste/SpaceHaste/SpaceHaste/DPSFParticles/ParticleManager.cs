using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DPSF;
using SpaceHaste.Controls;

namespace SpaceHaste.DPSFParticles
{
    public class ParticleManager : DrawableGameComponent
    {
        ParticleSystemManager particleSystemManager;
        public List<Particle> ParticleList;
        //IDPSFParticleSystem mcCurrentParticleSystem;
        public static ParticleManager Instance;
        public ParticleManager(Game game): base(game)
        {
            particleSystemManager = new ParticleSystemManager();
            ParticleList = new List<Particle>();
            DrawOrder = 5;
            //CameraPosition = new Vector3(0,0,-200);
          
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
            Instance = this;
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
        public override void  Update(GameTime gameTime)   
        {
            for (int i = 0; i < ParticleList.Count; i++)
            {
                ParticleList[i].Update(gameTime);
            }
            particleSystemManager.SetCameraPositionForAllParticleSystems(ControlManager.CameraPosition);

            particleSystemManager.UpdateAllParticleSystems((float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        public override void  Draw(GameTime gameTime)
        {
            particleSystemManager.SetWorldViewProjectionMatricesForAllParticleSystems(Matrix.Identity, ControlManager.View, ControlManager.Projection);

            // Draw the Particle Systems manually
            particleSystemManager.DrawAllParticleSystems();
        }
    }
}
