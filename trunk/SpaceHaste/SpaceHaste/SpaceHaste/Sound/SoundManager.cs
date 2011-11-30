using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace SpaceHaste.Sounds
{
    public enum ConstantSounds
    {
        FightorFlight,
    };

    public enum SoundEffects
    {
        laser, explode, engines, missLaunch, missExpl
    };
    public class SoundManager : GameComponent
    {
        ContentManager soundContent;
        public static SoundManager Sounds;


        AudioEngine engine;
        WaveBank waveBank;
        SoundBank soundBank;
        static Cue laser, explode, engines, missLaunch, missExpl;
        Game gameref;


        
        //SoundEffect menuselect;
        //List<SoundEffect> hit;
        //List<SoundEffect> playerattack;
        //List<SoundEffect> monsterattack;
        //SoundEffect playerattacklong;
        //Random random;

        SoundEffect FightorFlight;
        SoundEffectInstance FoF;
        //SoundEffectInstance menubackground;
        public SoundManager(Game game) : base(game)
        {
            try
            {

                gameref = game;
                engine = new AudioEngine("Content/Sound/Audio.xgs");
                soundBank = new SoundBank(engine, "Content/Sound/Sounds.xsb");
                waveBank = new WaveBank(engine, "Content/Sound/Waves.xwb");
            }
            catch { }   
                Sounds = this;

            Reset();
        }
        void Reset()
        {
            
        }
        public void Load()
        {
            try
            {
                soundContent = gameref.Content;

                laser = soundBank.GetCue("laser");
                explode = soundBank.GetCue("explode");
                engines = soundBank.GetCue("Engines");
                FightorFlight = soundContent.Load<SoundEffect>("Sound/sounds/SpaceHasteFlight(v0.9)");
                missLaunch = soundBank.GetCue("RocketExpl");
                missExpl = soundBank.GetCue("RocketFire");
                FoF = FightorFlight.CreateInstance();
                FoF.IsLooped = true;
                FoF.Volume = 0.1F;
                
            }
            catch { }
        }




        public void StopHover(){
            try
            {
       //         ambientInstance.Stop();
            }
            catch { }
        }

        public void PlaySound(SoundEffects sound)
        {
            try
            {
                //SoundEffectInstance sb;
                switch (sound)
                {
                    case SoundEffects.laser:

                        Cue laser = soundBank.GetCue("laser");
                        laser.Play();
                        
                        break;

                    case SoundEffects.explode:
                        Cue explode = soundBank.GetCue("explode");
                        explode.Play();
                        break;

                    case SoundEffects.engines:
                        Cue engines = soundBank.GetCue("Engines");
                        engines.Play();
                        break;

                    case SoundEffects.missExpl:
                        Cue missExpl = soundBank.GetCue("RocketExpl");
                        missExpl.Play();
                        break;

                    case SoundEffects.missLaunch:
                        Cue missLaunch = soundBank.GetCue("RocketFire");
                        missLaunch.Play();
                        break;

 
                };
            }
            catch { }
        }


        public void Update() { 
          //  pitch = ambient.GetVariable
          //  ambient.
        }
        public void TurnSoundOn(ConstantSounds sound)
        {
            try
            {
                switch (sound)
                {
                    case ConstantSounds.FightorFlight:
                         if (FoF.State.Equals(2))
                             FoF.Play();
                         FoF.Resume();
                         break;

                };
            }
            catch { }
        }
        public void TurnSoundOff(ConstantSounds sound)
        {
            try
            {
                switch (sound)
                {
                    
                case ConstantSounds.FightorFlight:
                    if (FoF.State.Equals(0))
                        FoF.Stop();
                    
                    break;


                };
            }
            catch { }
        }
    }
}