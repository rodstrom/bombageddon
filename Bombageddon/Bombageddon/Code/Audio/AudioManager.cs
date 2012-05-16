using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Bombageddon.Code.Audio
{
    public class AudioManager : GameComponent
    {
        List<KeyValuePair<String, SoundEffect>> effectList = new List<KeyValuePair<String, SoundEffect>>();
        Dictionary<String, SoundEffect> musicList = new Dictionary<String, SoundEffect>();

        SoundEffectInstance activeMusic = null;

        public float MusicVolume
        {
            get;
            set;
        }

        public float EffectVolume
        {
            get;
            set;
        }

        public AudioManager(Bombageddon game) : base(game)
        {
            MusicVolume = (float.Parse(game.config.getValue("Audio", "MusicVolume")) / 100);
            EffectVolume = (float.Parse(game.config.getValue("Audio", "EffectVolume")) / 100);
        }

        public void LoadNewEffect(String key, String effect)
        {
            if (!(key == null || effect == null))
            {
                SoundEffect tmpEffect = Game.Content.Load<SoundEffect>(effect);
                tmpEffect.Name = key;
                effectList.Add(new KeyValuePair<String, SoundEffect>(key, tmpEffect));
            }
        }

        //public void RemoveEffect(String key)
        //{
        //    if (key != null)
        //    {
        //        effectList[key].Dispose();
        //        effectList.Remove(key);
        //    }
        //}

        private SoundEffect getSoundEffect(String effectType)
        {
            List<SoundEffect> bgList = new List<SoundEffect>();

            foreach (KeyValuePair<String, SoundEffect> e in effectList)
            {
                if (e.Key == effectType)
                {
                    bgList.Add(e.Value);
                }
            }

            return bgList[new Random().Next(bgList.Count)];
        }

        public void PlayEffect(String key)
        {
            SoundEffectInstance tmpEffect = getSoundEffect(key).CreateInstance();
            if (tmpEffect != null)
            {
                tmpEffect.Volume = EffectVolume;
                tmpEffect.Play();
            }
        }

        public void LoadNewMusic(String key, String music)
        {
            if (!(key == null || music == null))
            {
                SoundEffect tmpEffect = Game.Content.Load<SoundEffect>(music);
                tmpEffect.Name = key;
                musicList.Add(key, tmpEffect);
            }
        }

        public void RemoveMusic(String key)
        {
            if (key != null)
            {
                musicList[key].Dispose();
                musicList.Remove(key);
            }
        }

        public void SetMusic(String key)
        {
            activeMusic = musicList[key].CreateInstance();
            activeMusic.Volume = MusicVolume;
            activeMusic.IsLooped = true;
        }

        public void StopMusic()
        {
            if (activeMusic != null)
            {
                activeMusic.Stop();
            }
        }

        public void PlayMusic()
        {
            SoundState State = activeMusic.State;
            if (State == SoundState.Paused || State == SoundState.Stopped)
            {
                activeMusic.Play();
            }
            if (State == SoundState.Playing)
            {
                activeMusic.Pause();
            }
        }

        public void FadeMusic(String nextTrack, float speed)
        {
        }
    }
}
