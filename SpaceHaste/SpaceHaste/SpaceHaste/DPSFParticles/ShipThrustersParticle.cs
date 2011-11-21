
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
    public class ShipThrustersParticle : Particle
    {
       TrailParticleSystem laserParicleSystem;
        public int counter = 0;
        public Vector3 Position;

        public static void CreateLaserParticle(Vector3 pos1, Vector3 pos2)
        {
            ParticleManager.Instance.Add(new LaserParticle(pos1, pos2));
        }
        Vector3 Start;
        Vector3 End;
        Vector3 Length;
        double timeToEnd = .5;
        public ShipThrustersParticle(Vector3 Start, Vector3 End)
            : base()
        {
            this.Start = Start;
            this.End = End;
            Length = End - Start;
            laserParicleSystem = new TrailParticleSystem(Game1.game);

            laserParicleSystem.AutoInitialize(Game1.game.GraphicsDevice, Game1.game.Content, Hud.spriteBatch);
            laserParicleSystem.Emitter.PositionData.Position = Position;

            //deathParicleSystem.Emitter.BurstParticles = (10);

            //  deathParicleSystem.Emitter.Enabled = true;

            //deathParicleSystem.Emitter.BurstComplete += BurstFinished;
            ParticleSystem = laserParicleSystem;
        }
        void BurstFinished(object sender, EventArgs e)
        {
            laserParicleSystem.Emitter.Enabled = false;
        }
        double timercd = 0;
        bool exploded = false;
        //126... 42....-38
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            timercd += gameTime.ElapsedGameTime.TotalSeconds;

            if (timercd > timeToEnd)
            {
                ParticleManager.Instance.Remove(this);
                return;
            }
            laserParicleSystem.Emitter.PositionData.Position = Start + (float)(timercd / timeToEnd) * Length;

           

            //mcSphereParticleSystem.Emitter.PositionData.Position = Position;
            base.Update(gameTime);
        }
    }
}
