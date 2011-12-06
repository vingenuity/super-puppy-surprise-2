
#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

namespace DPSF.ParticleSystems
{
    /// <summary>
    /// Create a new Particle System class that inherits from a Default DPSF Particle System.
    /// </summary>
#if (WINDOWS)
    [Serializable]
#endif
    class ShipWreckageParticleSystem : DefaultTexturedQuadTextureCoordinatesParticleSystem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipWreckageParticleSystem(Game cGame) : base(cGame) { }

        //===========================================================
        // Structures and Variables
        //===========================================================
        private Color[] msaColors = { Color.White, Color.Gray, Color.Black, Color.Green, Color.Yellow, Color.Red, Color.Pink, Color.Brown, Color.Blue };
        private int miCurrentColor = 0;

        public float mfColorBlendAmount = 0.5f;
        public Vector3 mcExternalObjectPosition = Vector3.Zero;
        public float mfAttractRepelForce = 3.0f;
        public float mfAttractRepelRange = 50.0f;


        Rectangle _debris1TextureCoordinates = new Rectangle(256, 256, 39, 44);
        Rectangle _debris2TextureCoordinates = new Rectangle(300, 261, 35, 33);
        Rectangle _debris3TextureCoordinates = new Rectangle(344, 263, 38, 30);
        Rectangle _debris4TextureCoordinates = new Rectangle(259, 302, 37, 35);
        Rectangle _debris5TextureCoordinates = new Rectangle(298, 299, 42, 41);
        Rectangle _debris6TextureCoordinates = new Rectangle(342, 306, 40, 32);
        Rectangle _debris7TextureCoordinates = new Rectangle(257, 345, 39, 36);
        Rectangle _debris8TextureCoordinates = new Rectangle(299, 349, 41, 25);
        Rectangle _debris9TextureCoordinates = new Rectangle(343, 342, 36, 40);

        //===========================================================
        // Overridden Particle System Functions
        //===========================================================

        //===========================================================
        // Initialization Functions
        //===========================================================
        public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
        {
            InitializeTexturedQuadParticleSystem(cGraphicsDevice, cContentManager, 50,50,
                                                UpdateVertexProperties, "Textures/ExplosionParticles");
           // InitializeSpriteParticleSystem(cGraphicsDevice, cContentManager, 20, 20, "Textures/Particle");
            LoadSmokeEvents();
            Emitter.ParticlesPerSecond = 400;
            Name = "Smoke";
        }

        public void LoadSmokeEvents()
        {
            ParticleInitializationFunction = InitializeParticleRisingSmoke;

            ParticleEvents.RemoveAllEvents();
            //ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionAndVelocityUsingAcceleration, 500);
           // ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity);
          //  ParticleEvents.AddEveryTimeEvent(UpdateColor);
          //  ParticleEvents.AddEveryTimeEvent(UpdateParticleTransparencyWithQuickFadeInAndSlowFadeOut, 100);
           // ParticleEvents.AddEveryTimeEvent(IncreaseSizeBasedOnLifetime);
        }
        // Used to generate a single smoke plume
        public void InitializeParticleRisingSmoke(DefaultTextureQuadTextureCoordinatesParticle cParticle)
        {
            //cParticle.Lifetime = RandomNumber.Between(1.0f, 7.0f);
            int a = 80;
            cParticle.Position = Emitter.PositionData.Position + new Vector3(RandomNumber.Next(-a, a), RandomNumber.Next(-a, a), RandomNumber.Next(-a, a)); ;
            // cParticle.Position += new Vector3(0, 10, 0);
            cParticle.Size = RandomNumber.Next(10, 40);
            cParticle.Color = msaColors[miCurrentColor];
            cParticle.Color = msaColors[6];

         //   cParticle.Orientation = DPSF.Orientation3D.Rotate(Matrix.CreateRotationZ(RandomNumber.Between(0, MathHelper.TwoPi)), cParticle.Orientation);
            cParticle.Orientation = DPSF.Orientation3D.Rotate(Matrix.CreateRotationY(RandomNumber.Between(-(MathHelper.TwoPi), MathHelper.TwoPi))*
                Matrix.CreateRotationX(RandomNumber.Between(-(MathHelper.TwoPi), MathHelper.TwoPi))*
                Matrix.CreateRotationZ(RandomNumber.Between(-(MathHelper.TwoPi), MathHelper.TwoPi)), cParticle.Orientation);
            cParticle.Velocity = new Vector3(RandomNumber.Next(-45, 45), RandomNumber.Next(-45, 45), RandomNumber.Next(-45, 45));
            cParticle.Acceleration = Vector3.Zero;
          //  cParticle.Orientation 
           // cParticle.RotationalVelocity = RandomNumber.Between(-MathHelper.Pi, MathHelper.Pi);
            //cParticle.Color = Color.Purple;
            cParticle.StartSize = cParticle.Size;

            mfColorBlendAmount = 0.5f;

            Rectangle textureCoordinates;
            switch (RandomNumber.Next(0, 9))
            {
                default:
                case 0: textureCoordinates = _debris1TextureCoordinates; break;
                case 1: textureCoordinates = _debris2TextureCoordinates; break;
                case 2: textureCoordinates = _debris3TextureCoordinates; break;
                case 3: textureCoordinates = _debris4TextureCoordinates; break;
                case 4: textureCoordinates = _debris5TextureCoordinates; break;
                case 5: textureCoordinates = _debris6TextureCoordinates; break;
                case 6: textureCoordinates = _debris7TextureCoordinates; break;
                case 7: textureCoordinates = _debris8TextureCoordinates; break;
                case 8: textureCoordinates = _debris9TextureCoordinates; break;
            }

            cParticle.SetTextureCoordinates(textureCoordinates, Texture.Width, Texture.Height);
        }

        // Used to generate random smoke particles around the floor
        public void InitializeParticleFoggySmoke(DefaultSprite3DBillboardParticle cParticle)
        {
            cParticle.Lifetime = RandomNumber.Between(1.0f, 3.0f);

            cParticle.Position = Emitter.PositionData.Position;
            cParticle.Position += new Vector3(RandomNumber.Next(-5000, 5000), 0, RandomNumber.Next(-5000, 5000));
            cParticle.Size = RandomNumber.Next(1, 2);
            cParticle.Color = msaColors[miCurrentColor];
            // cParticle.Orientation = DPSF.Orientation3D.Rotate(Matrix.CreateRotationZ(RandomNumber.Between(0, MathHelper.TwoPi)), cParticle.Orientation);

            cParticle.Velocity = new Vector3(RandomNumber.Next(-180, 180), RandomNumber.Next(-180, 180), RandomNumber.Next(-180, 180));
            cParticle.Acceleration = Vector3.Zero;
            cParticle.RotationalVelocity = RandomNumber.Between(-MathHelper.Pi, MathHelper.Pi);

            cParticle.StartSize = cParticle.Size;

            mfColorBlendAmount = 0.5f;
        }
        //===========================================================
        // Particle Update Functions
        //===========================================================
        protected void IncreaseSizeBasedOnLifetime(DefaultSprite3DBillboardParticle cParticle, float fElapsedTimeInSeconds)
        {
            cParticle.Size = ((1.0f + cParticle.NormalizedElapsedTime) / 1.0f) * cParticle.StartSize;
        }

        protected void UpdateColor(DefaultSprite3DBillboardParticle cParticle, float fElapsedTimeInSeconds)
        {
            cParticle.Color = new Color(111,111,111);
        }

        protected void RepelParticleFromExternalObject(DefaultSprite3DBillboardParticle cParticle, float fElapsedTimeInSeconds)
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
                cParticle.RotationalVelocity += 0.005f;
            }
        }

        protected void AttractParticleToExternalObject(DefaultSprite3DBillboardParticle cParticle, float fElapsedTimeInSeconds)
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

      
    }
}
