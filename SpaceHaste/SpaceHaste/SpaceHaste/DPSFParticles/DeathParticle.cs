using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DPSF.ParticleSystems;
using SpaceHaste.Graphics;
using SpaceHaste.Huds;

namespace SpaceHaste.DPSFParticles
{
    public class DeathParticle : Particle
    {
        ExplosionParticleSystem deathParicleSystem;
        public int counter = 0;
        public Vector3 Position;
       
       
        public static void CreateDeathParticle(Vector3 pos)
        {
            ParticleManager.Instance.Add(new DeathParticle(pos));
        }
        public DeathParticle(Vector3 Position)
            : base()
        {
            deathParicleSystem = new ExplosionParticleSystem(Game1.game);
            deathParicleSystem.AutoInitialize(Game1.game.GraphicsDevice, Game1.game.Content, Hud.spriteBatch);
            deathParicleSystem.Emitter.PositionData.Position = Position;
           
            //deathParicleSystem.Emitter.BurstParticles = (10);

          //  deathParicleSystem.Emitter.Enabled = true;

            //deathParicleSystem.Emitter.BurstComplete += BurstFinished;
            ParticleSystem = deathParicleSystem;
        }
        void BurstFinished(object sender, EventArgs e)
        {
            deathParicleSystem.Emitter.Enabled = false;
        }
        double timercd = 0;
        bool exploded = false;
        //126... 42....-38
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
           timercd += gameTime.ElapsedGameTime.TotalSeconds;
            if(timercd > .01 && !exploded)
            {
                exploded = true;
                deathParicleSystem.Explode();
            }
           if (timercd > 25)
           {
               ParticleManager.Instance.Remove(this);
           }

           
            //mcSphereParticleSystem.Emitter.PositionData.Position = Position;
            base.Update(gameTime);
        }
    }
}
