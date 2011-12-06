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
    public class ThrustersParticle : Particle
    {
        TrailParticleSystem paricleSystem;
        public int counter = 0;
        public Vector3 Position;
        public Vector3 Offset;

        public static ThrustersParticle CreateParticle(Vector3 pos, Vector3 offset)
        {
            ThrustersParticle p = new ThrustersParticle(pos, offset);
            ParticleManager.Instance.Add(p);
            return p;
        }
        public ThrustersParticle(Vector3 Position, Vector3 offset)
            : base()
        {
            Offset = offset;
            paricleSystem = new TrailParticleSystem(Game1.game);

            paricleSystem.AutoInitialize(Game1.game.GraphicsDevice, Game1.game.Content, Hud.spriteBatch);
            paricleSystem.Emitter.PositionData.Position = Position;

            //deathParicleSystem.Emitter.BurstParticles = (10);

            //  deathParicleSystem.Emitter.Enabled = true;

            //deathParicleSystem.Emitter.BurstComplete += BurstFinished;
            ParticleSystem = paricleSystem;
        }
        void BurstFinished(object sender, EventArgs e)
        {
            //paricleSystem.Emitter.Enabled = false;
        }
        //double timercd = 0;
        //bool exploded = false;
        //126... 42....-38
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            paricleSystem.Emitter.PositionData.Position = Position + Offset;
            base.Update(gameTime);
        }
    }
}
