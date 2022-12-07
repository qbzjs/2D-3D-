using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace LSH_Lib
{
	public class UIAudioManager : MonoBehaviour
	{
        public Sound[] sounds;
        [SerializeField]
        List<AudioSource> audioSources = new List<AudioSource>();

        public static UIAudioManager instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }

            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.outputAudioMixerGroup = s.mixer;
                s.source.loop = s.loop;
            }
        }
        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Play();
        }
        
        
        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Stop();
        }
        
    }
}
