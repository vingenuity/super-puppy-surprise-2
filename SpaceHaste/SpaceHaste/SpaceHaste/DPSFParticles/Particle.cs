﻿using System;
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
        public virtual void Start()
        {
            if (!started)
            {
                started = true;
                if (ParticleSystem != null)
                {
                    ParticleSystem.Destroy();
                }
                ParticleSystem.AutoInitialize(Game1.game.GraphicsDevice, Game1.game.Content, null);
                
                
                ParticleManager.Instance.Add(this);
            }
        }

        public virtual void End()
        {
            started = false;
            if (ParticleSystem != null)
            {
                ParticleSystem.Destroy();
            }
            ParticleManager.Instance.Remove(this);
        }
    }
}