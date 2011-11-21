using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using DPSF;
using DPSF.ParticleSystems;


namespace SpaceHaste.DPSFParticles
{
    public class Particle
    {
        public IDPSFParticleSystem ParticleSystem;
        public bool started = false;
        public bool loaded = false;
        public Particle()
        {
        }
        public virtual void Update(GameTime gameTime)
        {

        }
       
    }
}
