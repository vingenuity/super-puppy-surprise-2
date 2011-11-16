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
	class Figure8ParticleSystem : DefaultTexturedQuadParticleSystem
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Figure8ParticleSystem(Game cGame) : base(cGame) { }

		//===========================================================
		// Structures and Variables
		//===========================================================

		//===========================================================
		// Overridden Particle System Functions
		//===========================================================
		protected override void InitializeRenderProperties()
		{
			base.InitializeRenderProperties();

			// Have to use the AlphaTest Effect in order to use alpha testing with depth sorting
			this.Effect = new AlphaTestEffect(this.GraphicsDevice);
			RenderProperties.DepthStencilState.DepthBufferWriteEnable = true; // Turn on depth (z-buffer) sorting
		}

		protected override void SetEffectParameters()
		{
			// Set the AlphaTest Effect's parameters
			AlphaTestEffect effect = this.Effect as AlphaTestEffect;
			effect.World = World;
			effect.View = View;
			effect.Projection = Projection;
			effect.FogEnabled = false;
			effect.Texture = Texture;
			effect.VertexColorEnabled = true;	// Use the particle's specified color
		}

		//===========================================================
		// Initialization Functions
		//===========================================================
		public override void AutoInitialize(GraphicsDevice cGraphicsDevice, ContentManager cContentManager, SpriteBatch cSpriteBatch)
		{
			InitializeTexturedQuadParticleSystem(cGraphicsDevice, cContentManager, 1000, 50000,
												UpdateVertexProperties, "Textures/Donut");
			LoadEvents();
			Emitter.ParticlesPerSecond = 8;
			Name = "Figure 8";
		}

		public void InitializeParticleFigure8(DefaultTexturedQuadParticle cParticle)
		{
			cParticle.Lifetime = 4.0f;

			cParticle.Position = Emitter.PositionData.Position;
			cParticle.Position += new Vector3(0, 100, 0);
			cParticle.Size = 10.0f;
			cParticle.Color = Color.Red;

			cParticle.Velocity = new Vector3(0, 0, 0);
			cParticle.Acceleration = Vector3.Zero;
		}

		public void LoadEvents()
		{
			ParticleInitializationFunction = InitializeParticleFigure8;

			ParticleEvents.RemoveAllEvents();
			ParticleEvents.AddEveryTimeEvent(UpdatedPositionOnFigure8);
			ParticleEvents.AddEveryTimeEvent(UpdateParticleToFaceTheCamera, 100);
		}

		//===========================================================
		// Particle Update Functions
		//===========================================================
		protected void UpdatedPositionOnFigure8(DefaultTexturedQuadParticle cParticle, float fElapsedTimeInSeconds)
		{
			float fRadius = 25;
			float fHeight1 = 75;
			float fHeight2 = 25;

			Vector3 sPosition = new Vector3();

			// If the Particle is on the first loop
			if (cParticle.NormalizedElapsedTime < 0.5f)
			{
				// Calculate the angle on the circle that the particle should be at
				float fAngle = (cParticle.NormalizedElapsedTime * 2 * MathHelper.TwoPi) - MathHelper.PiOver2;

				// Calculate where on the loop the Particle should be
				sPosition = DPSFHelper.PointOnSphere(-MathHelper.PiOver2, fAngle, fRadius) + new Vector3(0, fHeight1, 0);
			}
			// Else it is on the second loop
			else
			{
				// Calculate the angle on the circle that the particle should be at
				float fAngle = MathHelper.TwoPi - (((cParticle.NormalizedElapsedTime - 0.5f) * 2 * MathHelper.TwoPi) - MathHelper.PiOver2);

				// Calculate where on the loop the Particle should be
				sPosition = DPSFHelper.PointOnSphere(-MathHelper.PiOver2, fAngle, fRadius) + new Vector3(0, fHeight2, 0);
			}

			// Set the new Position of the Particle
			cParticle.Position = sPosition;
		}

		//===========================================================
		// Particle System Update Functions
		//===========================================================
		
		//===========================================================
		// Other Particle System Functions
		//===========================================================
	}
}