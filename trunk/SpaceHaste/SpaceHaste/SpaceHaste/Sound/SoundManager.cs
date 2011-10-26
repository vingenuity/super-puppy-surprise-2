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

    };

    public enum SoundEffects
    {
        laser
    };
    public class SoundManager
    {
        ContentManager soundContent;
        public static SoundManager Sounds;


        AudioEngine engine;
        WaveBank waveBank;
        SoundBank soundBank;
        static Cue laser; // menu;
        Game gameref;


        
        //SoundEffect menuselect;
        //List<SoundEffect> hit;
        //List<SoundEffect> playerattack;
        //List<SoundEffect> monsterattack;
        //SoundEffect playerattacklong;
        //Random random;

        //SoundEffect MenuBackground;
        //SoundEffectInstance menubackground;
        public SoundManager(Game game)
        {
            try
            {
                gameref = game;
                engine = new AudioEngine("Content/Sound/Audio.xgs");
                soundBank = new SoundBank(engine, "Content/Sound/Sounds.xsb");
                waveBank = new WaveBank(engine, "Content/Sound/Waves.xwb");

                
                Sounds = this;
            }
            catch { }
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
                /*switch (sound)
                {
                    case ConstantSounds.MenuBackground:
                         if (!menuback.IsPlaying)
                             menuback.Play();
                         menuback.Resume();
                         break;
                     case ConstantSounds.Ambient:
                        if (!ambient.IsPlaying)
                            ambient.Play();
                        ambient.Resume();
                        break;
                };*/
            }
            catch { }
        }
        public void TurnSoundOff(ConstantSounds sound)
        {
            try
            {
                /*switch (sound)
                {
                    
                case ConstantSounds.MenuBackground:
                    if (menuback.IsPlaying)
                        menuback.Pause();
                    
                    break;
                     

                    case ConstantSounds.Ambient:
                        if (ambient.IsPlaying)
                            ambient.Pause();
                        break;

                };*/
            }
            catch { }
        }
    }
}