﻿#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace DPSF.ParticleSystems
{
    /// <summary>
    /// Create a new Particle System class that inherits from a
    /// Default DPSF Particle System
    /// </summary>
#if (WINDOWS)
    [Serializable]
#endif
    class NebulaeParticleSystem : DefaultTexturedQuadParticleSystem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public NebulaeParticleSystem(Game cGame) : base(cGame) { }

        //===========================================================
        // Structures and Variables
        //===========================================================
        private Color[] msaColors = { Color.White, Color.Gray, Color.Black, Color.Green, Color.Yellow, Color.Red, Color.Pink, Color.Brown, Color.Blue };
        private int miCurrentColor = 0;

        public float mfColorBlendAmount = 0.5f;
        public Vector3 mcExternalObjectPosition = Vector3.Zero;
        public float mfAttractRepelForce = 3.0f;
        public float mfAttractRepelRange = 50.0f;

        //===========================================================
        // Overridden Particle System Functions
        //===========================================================
        protected override void SetEffectParameters()
        {
            base.SetEffectParameters();

            // Specify to not use the Color component when drawing (Texture color is not tinted)
            Effect.Parameters["xColorBlendAmount"].SetValue(mfColorBlendAmount);
        }

        //===========================================================
        // Initialization Functions
        //===========================================================
        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeTexturedQuadParticleSystem(cGraphicsDevice, cContentManager, 100, 500,
                                                    UpdateVertexProperties, "Textures/TestImage");
            LoadSmokeEvents();
            Emitter.ParticlesPerSecond = 10;
            Name = "Smoke";
        }

        public void LoadSmokeEvents()
        {
            ParticleInitializationFunction = InitializeParticleRisingSmoke;

            ParticleEvents.RemoveAllEvents();
            ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionAndVelocityUsingAcceleration, 500);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
            ParticleEvents.AddEveryTimeEvent(UpdateColor);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyWithQuickFadeInAndSlowFadeOut, 100);
            ParticleEvents.AddEveryTimeEvent(IncreaseSizeBasedOnLifetime);
            ParticleEvents.AddEveryTimeEvent(UpdateParticleToFaceTheCamera, 200);
        }

        // Used to generate a single smoke plume
        public void InitializeParticleRisingSmoke(DefaultTexturedQuadParticle cParticle)
        {
            cParticle.Lifetime = RandomNumber.Between(1.0f, 7.0f);
            int a = 250;
            cParticle.Position = Emitter.PositionData.Position + new Vector3(RandomNumber.Next(-a, a), RandomNumber.Next(-a, a), RandomNumber.Next(-a, a)); ;
           // cParticle.Position += new Vector3(0, 10, 0);
            cParticle.Size = RandomNumber.Next(110, 140);
            cParticle.Color = msaColors[miCurrentColor];
            cParticle.Color = msaColors[6];

            cParticle.Orientation = DPSF.Orientation3D.Rotate(Matrix.CreateRotationZ(RandomNumber.Between(0, MathHelper.TwoPi)), cParticle.Orientation);

            cParticle.Velocity = new Vector3(RandomNumber.Next(-45, 45), RandomNumber.Next(-45, 45), RandomNumber.Next(-45, 45));
            cParticle.Acceleration = Vector3.Zero;
            cParticle.RotationalVelocity.Z = RandomNumber.Between(-MathHelper.Pi, MathHelper.Pi);
            cParticle.Color = Color.Purple;
            cParticle.StartSize = cParticle.Size;

            mfColorBlendAmount = 0.5f;
        }

        // Used to generate random smoke particles around the floor
        public void InitializeParticleFoggySmoke(DefaultTexturedQuadParticle cParticle)
        {
            cParticle.Lifetime = RandomNumber.Between(1.0f, 3.0f);

            cParticle.Position = Emitter.PositionData.Position;
            cParticle.Position += new Vector3(RandomNumber.Next(-5000, 5000), 0, RandomNumber.Next(-5000, 5000));
            cParticle.Size = RandomNumber.Next(100, 175);
            cParticle.Color = msaColors[miCurrentColor];
            cParticle.Orientation = DPSF.Orientation3D.Rotate(Matrix.CreateRotationZ(RandomNumber.Between(0, MathHelper.TwoPi)), cParticle.Orientation);

            cParticle.Velocity = new Vector3(RandomNumber.Next(-180, 180), RandomNumber.Next(-180, 180), RandomNumber.Next(-180, 180));
            cParticle.Acceleration = Vector3.Zero;
            cParticle.RotationalVelocity.Z = RandomNumber.Between(-MathHelper.Pi, MathHelper.Pi);

            cParticle.StartSize = cParticle.Size;

            mfColorBlendAmount = 0.5f;
        }

        //===========================================================
        // Particle Update Functions
        //===========================================================
        protected void IncreaseSizeBasedOnLifetime(DefaultTexturedQuadParticle cParticle, float fElapsedTimeInSeconds)
        {
            cParticle.Size = ((1.0f + cParticle.NormalizedElapsedTime) / 1.0f) * cParticle.StartSize * 2;
        }

        protected void UpdateColor(DefaultTexturedQuadParticle cParticle, float fElapsedTimeInSeconds)
        {
            cParticle.Color = msaColors[miCurrentColor];
        }

        protected void RepelParticleFromExternalObject(DefaultTexturedQuadParticle cParticle, float fElapsedTimeInSeconds)
        {
            // Calculate Direction away from the Object and how far the Particle is from the Object
            Vector3 sDirectionAwayFromObject = cParticle.Position - mcExternalObjectPosition;
            float fDistance = sDirectionAwayFromObject.Length();

            // If the Particle is close enough to the Object to be affected by it
            if (fDistance < mfAttractRepelRange)
            {
                // Repel the Particle from the Object
                sDirectionAwayFromObject.Normalize();
                cParticle.Velocity += sDirectionAwayFromObject * (mfAttractRepelRange - fDistance) * mfAttractRepelForce;
                cParticle.RotationalVelocity.Z += 0.005f;
            }
        }

        protected void AttractParticleToExternalObject(DefaultTexturedQuadParticle cParticle, float fElapsedTimeInSeconds)
        {
            // Calculate Direction towards the Object and how far the Particle is from the Object
            Vector3 sDirectionTowardsObject = mcExternalObjectPosition - cParticle.Position;
            float fDistance = sDirectionTowardsObject.Length();

            // If the Particle is close enough to the Object to be affected by it
            if (fDistance < mfAttractRepelRange)
            {
                // Attract the Particle to the Object
                sDirectionTowardsObject.Normalize();
                cParticle.Velocity = sDirectionTowardsObject * (mfAttractRepelRange - fDistance) * mfAttractRepelForce;
            }
        }

        //===========================================================
        // Particle System Update Functions
        //===========================================================

        //===========================================================
        // Other Particle System Functions
        //===========================================================
        public void ChangeColor()
        {
            if (++miCurrentColor >= msaColors.Length)
            {
                miCurrentColor = 0;
            }
        }

        public void MakeParticlesAttractToExternalObject()
        {
            // Make sure we only apply the Attract function once by first removing the function if it already exists
            this.ParticleEvents.RemoveEveryTimeEvents(AttractParticleToExternalObject);
            this.ParticleEvents.AddEveryTimeEvent(AttractParticleToExternalObject);
        }

        public void MakeParticlesRepelFromExternalObject()
        {
            // Make sure we only apply the Repel function once by first removing the function if it already exists
            this.ParticleEvents.RemoveEveryTimeEvents(RepelParticleFromExternalObject);
            this.ParticleEvents.AddEveryTimeEvent(RepelParticleFromExternalObject);
        }

        public void StopParticleAttractionAndRepulsionToExternalObject()
        {
            this.ParticleEvents.RemoveEveryTimeEvents(RepelParticleFromExternalObject);
            this.ParticleEvents.RemoveEveryTimeEvents(AttractParticleToExternalObject);
        }
    }
}