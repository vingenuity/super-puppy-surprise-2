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
    public class ShipWreckageParticle : Particle
    {
        //ShipWreckageParticleSystem paricleSystem;
        public int counter = 0;
        public Vector3 Position;


        public static ShipWreckageParticle CreateParticle(Vector3 pos)
        {
            ShipWreckageParticle p = new ShipWreckageParticle(pos);
            ParticleManager.Instance.Add(p);
            return p;
        }
        public ShipWreckageParticle(Vector3 Position)
            : base()
        {
           // paricleSystem = new ShipWreckageParticleSystem(Game1.game);

          //  paricleSystem.AutoInitialize(Game1.game.GraphicsDevice, Game1.game.Content, Hud.spriteBatch);
           // paricleSystem.Emitter.PositionData.Position = Position;

            //deathParicleSystem.Emitter.BurstParticles = (10);

            //  deathParicleSystem.Emitter.Enabled = true;

            //deathParicleSystem.Emitter.BurstComplete += BurstFinished;
           // ParticleSystem = paricleSystem;
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
