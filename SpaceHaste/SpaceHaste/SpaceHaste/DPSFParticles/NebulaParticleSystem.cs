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
    public class NebulaParticle : Particle
    {
        ExplosionParticleSystem paricleSystem;
        public int counter = 0;
        public Vector3 Position;


        public static void CreateNebulaParticle(Vector3 pos)
        {
            ParticleManager.Instance.Add(new DeathParticle(pos));
        }
        public NebulaParticle(Vector3 Position)
            : base()
        {
            paricleSystem = new ExplosionParticleSystem(Game1.game);

            paricleSystem.AutoInitialize(Game1.game.GraphicsDevice, Game1.game.Content, Hud.spriteBatch);
            paricleSystem.Emitter.PositionData.Position = Position;

            //deathParicleSystem.Emitter.BurstParticles = (10);

            //  deathParicleSystem.Emitter.Enabled = true;

            //deathParicleSystem.Emitter.BurstComplete += BurstFinished;
            ParticleSystem = paricleSystem;
        }
        void BurstFinished(object sender, EventArgs e)
        {
            paricleSystem.Emitter.Enabled = false;
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
                paricleSystem.Explode();
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
