using System;
using DPSF.ParticleSystems;
using Microsoft.Xna.Framework;

namespace SuperPuppySurprise.DPSFParticles
{
    public class TestParticle3 : Particle
    {
        SpriteParticleSystem mcSphereParticleSystem;
        public int counter = 0;
        public Vector3 Position;
        public TestParticle3()
            : base()
        {
            mcSphereParticleSystem = new SpriteParticleSystem(Game1.game);
            mcSphereParticleSystem.AutoInitialize(Game1.game.GraphicsDevice, Game1.game.Content, Game1.spriteBatch);
            mcSphereParticleSystem.Emitter.PositionData.Position = new Vector3(1000, 600, 0);
            ParticleSystem = mcSphereParticleSystem;

            //mcSphereParticleSystem.ChangeSphereRadius(.000000005f);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //mcSphereParticleSystem.Emitter.PositionData.Position = Position;
            base.Update(gameTime);
        }
    }
}