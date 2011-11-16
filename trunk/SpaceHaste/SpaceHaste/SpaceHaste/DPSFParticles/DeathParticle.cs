using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DPSF.ParticleSystems;

namespace SuperPuppySurprise.DPSFParticles
{
    public class DeathParticle : Particle
    {
        Random2DParticleSystem deathParicleSystem;
        public int counter = 0;
        public Vector2 Position;
       
        public void ChangeStatus(bool on)
        {
            deathParicleSystem.on = on;
        }
        public static void CreateDeathParticle(Vector2 pos)
        {
            Game1.ParticleEngine.Add(new DeathParticle(pos));
        }
        public DeathParticle(Vector2 Position)
            : base()
        {
            this.Position = Position;
            deathParicleSystem = new Random2DParticleSystem(Game1.game);
            deathParicleSystem.AutoInitialize(Game1.game.GraphicsDevice, Game1.game.Content, Game1.spriteBatch);
            deathParicleSystem.Emitter.EmitParticlesAutomatically = false;

            deathParicleSystem.Emitter.BurstParticles = (10);

            deathParicleSystem.Emitter.Enabled = true;

            deathParicleSystem.Emitter.BurstComplete += BurstFinished;
            ParticleSystem = deathParicleSystem;
        }
        void BurstFinished(object sender, EventArgs e)
        {
            deathParicleSystem.Emitter.Enabled = false;
        }
        double timercd;
        //126... 42....-38
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
           timercd += gameTime.ElapsedGameTime.TotalSeconds;

           if (timercd > 5)
           {
               Game1.ParticleEngine.Remove(this);
           }

           deathParicleSystem.Pos = ParticleManager.To3D(Position);
            //mcSphereParticleSystem.Emitter.PositionData.Position = Position;
            base.Update(gameTime);
        }
    }
}
