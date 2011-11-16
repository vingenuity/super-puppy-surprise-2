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
	/// Create a new Particle System class that inherits from a
	/// Default DPSF Particle System
	/// </summary>
#if (WINDOWS)
	[Serializable]
#endif
	class SnowParticleSystem : DefaultTexturedQuadParticleSystem
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SnowParticleSystem(Game cGame) : base(cGame) { }

		//===========================================================
		// Structures and Variables
		//===========================================================
		// Define the Max Wind Force to apply
		public Vector3 mcMaxWindForce = new Vector3(50, 10, 0);

		//===========================================================
		// Overridden Particle System Functions
		//===========================================================
		protected override void SetEffectParameters()
		{
			base.SetEffectParameters();

			// Specify to not use the Color component when drawing (Texture color is not tinted)
			Effect.Parameters["xColorBlendAmount"].SetValue(1.0f);
		}

		//===========================================================
		// Initialization Functions
		//===========================================================
		public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
		{
			InitializeTexturedQuadParticleSystem(cGraphicsDevice, cContentManager, 1000, 50000,
												UpdateVertexProperties, "Textures/Cloud");
			LoadSnowEvents();
			Emitter.ParticlesPerSecond = 300;
			Name = "Snow";
		}

		public void InitializeParticleSnow(DefaultTexturedQuadParticle cParticle)
		{
			// Position the Snow within 500 units of the emitter
			Vector3 sPosition = Emitter.PositionData.Position;
			sPosition.Y = 200;
			sPosition.X += RandomNumber.Next(-500, 500);
			sPosition.Z += RandomNumber.Next(-500, 500);

			cParticle.Lifetime = 0.0f;

			cParticle.Position = sPosition;
			cParticle.Size = RandomNumber.Next(2, 5);
			cParticle.Color = DPSFHelper.LerpColor(new Color(255, 255, 255, 50), new Color(255, 255, 255, 255), RandomNumber.NextFloat());
			cParticle.Orientation = Orientation3D.Rotate(Matrix.CreateRotationZ(RandomNumber.Between(0, MathHelper.TwoPi)), cParticle.Orientation);

			cParticle.Velocity = new Vector3(RandomNumber.Next(-10, 3), RandomNumber.Next(-15, -5), RandomNumber.Next(-10, 10));
			cParticle.Acceleration = Vector3.Zero;
			cParticle.RotationalVelocity.Z = RandomNumber.Between(-MathHelper.PiOver2, MathHelper.PiOver2);
		}

		public void LoadSnowEvents()
		{
			ParticleInitializationFunction = InitializeParticleSnow;

			ParticleEvents.RemoveAllEvents();
			ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionUsingExternalForce, 100);
			ParticleEvents.AddEveryTimeEvent(UpdateParticlePositionAndVelocityUsingAcceleration, 500);
			ParticleEvents.AddEveryTimeEvent(UpdateParticleRotationUsingRotationalVelocity, 500);
			ParticleEvents.AddEveryTimeEvent(UpdateParticleDieOnceGroundIsHit, 1000);
			ParticleEvents.AddEveryTimeEvent(UpdateParticleChangeVelocityAndRotation);
			ParticleEvents.AddEveryTimeEvent(UpdateParticleToFaceTheCamera, 600);
		}

		//===========================================================
		// Particle Update Functions
		//===========================================================
		protected void UpdateParticleDieOnceGroundIsHit(DefaultTexturedQuadParticle cParticle, float fElapsedTimeInSeconds)
		{
			// If the Particle hits the ground
			if (cParticle.Position.Y < -5)
			{
				// Kill it
				cParticle.Lifetime = 1.0f;
			}
		}

		protected void UpdateParticleChangeVelocityAndRotation(DefaultTexturedQuadParticle cParticle, float fElapsedTimeInSeconds)
		{
			if (RandomNumber.Next(0, 100) == 50)
			{
				Vector3 sChange = new Vector3(RandomNumber.Next(-3, 3), RandomNumber.Next(-3, 3), RandomNumber.Next(-3, 3));
				cParticle.Velocity += sChange;
				cParticle.RotationalVelocity.Z += RandomNumber.Between(-MathHelper.PiOver4, MathHelper.PiOver4);
			}
		}

		protected void UpdateParticleAddWindForce(DefaultTexturedQuadParticle cParticle, float fElapsedTimeInSeconds)
		{
			cParticle.ExternalForce = Vector3.Lerp(Vector3.Zero, mcMaxWindForce, RandomNumber.Between(0.1f, 1.0f));
		}

		protected void UpdateParticleRemoveWindForce(DefaultTexturedQuadParticle cParticle, float fElapsedTimeInSeconds)
		{
			cParticle.ExternalForce = Vector3.Zero;
		}

		//===========================================================
		// Particle System Update Functions
		//===========================================================
		
		//===========================================================
		// Other Particle System Functions
		//===========================================================

		public void AddWindForce()
		{
			this.ParticleEvents.AddOneTimeEvent(UpdateParticleAddWindForce);
		}

		public void RemoveWindForce()
		{
			this.ParticleEvents.AddOneTimeEvent(UpdateParticleRemoveWindForce);
		}
	}
}