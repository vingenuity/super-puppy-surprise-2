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
    public class NebulaParticle2 : Particle
    {
        ParticleSystem2 paricleSystem;
        public int counter = 0;
        public Vector3 Position;


        public static NebulaParticle2 CreateParticle(Vector3 pos)
        {
            NebulaParticle2 p = new NebulaParticle2(pos);
            ParticleManager.Instance.Add(p);
            return p;
        }
        public NebulaParticle2(Vector3 Position)
            : base()
        {
            paricleSystem = new ParticleSystem2(Game1.game);

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
        //double timercd = 0;
        //bool exploded = false;
        //126... 42....-38
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
