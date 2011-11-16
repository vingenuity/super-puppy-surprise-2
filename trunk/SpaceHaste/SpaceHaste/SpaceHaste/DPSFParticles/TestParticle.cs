
using DPSF.ParticleSystems;
using System;
using Microsoft.Xna.Framework;

namespace SuperPuppySurprise.DPSFParticles
{
    public class TestParticle : Particle
    {
        SphereParticleSystem mcSphereParticleSystem;
        public int counter = 0;
        public Vector3 Position;
        public TestParticle() : base()
        {
            mcSphereParticleSystem = new SphereParticleSystem(Game1.game);
            mcSphereParticleSystem.AutoInitialize(Game1.game.GraphicsDevice, Game1.game.Content, Game1.spriteBatch);
            ParticleSystem = mcSphereParticleSystem;
            
            //mcSphereParticleSystem.ChangeSphereRadius(.000000005f);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            mcSphereParticleSystem.Emitter.PositionData.Position = Position; 
            base.Update(gameTime);
        }
    }
}