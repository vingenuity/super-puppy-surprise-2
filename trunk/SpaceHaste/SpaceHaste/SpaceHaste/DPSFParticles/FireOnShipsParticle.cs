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
    public class FireOnShipsParticle : Particle
    {
        ExplosionParticleSystem LaserHitParicleSystem;
        public int counter = 0;
        public Vector3 Position;


        public static void CreateFireOnShipsParticle(Vector3 pos)
        {
            ParticleManager.Instance.Add(new FireOnShipsParticle(pos));
        }
        public FireOnShipsParticle(Vector3 Position)
            : base()
        {
            LaserHitParicleSystem = new ExplosionParticleSystem(Game1.game);

            LaserHitParicleSystem.AutoInitialize(Game1.game.GraphicsDevice, Game1.game.Content, Hud.spriteBatch);
            LaserHitParicleSystem.Emitter.PositionData.Position = Position;

            //deathParicleSystem.Emitter.BurstParticles = (10);

            //  deathParicleSystem.Emitter.Enabled = true;

            //deathParicleSystem.Emitter.BurstComplete += BurstFinished;
            ParticleSystem = LaserHitParicleSystem;
        }
        void BurstFinished(object sender, EventArgs e)
        {
            LaserHitParicleSystem.Emitter.Enabled = false;
        }
        double timercd = 0;
        bool exploded = false;
        //126... 42....-38
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            timercd += gameTime.ElapsedGameTime.TotalSeconds;
            if (timercd > .01 && !exploded)
            {
                exploded = true;
                LaserHitParicleSystem.Explode();
            }
            if (timercd > 3)
            {
                ParticleManager.Instance.Remove(this);
            }


            //mcSphereParticleSystem.Emitter.PositionData.Position = Position;
            base.Update(gameTime);
        }
    }
}

