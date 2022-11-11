using UnityEngine.Audio;
using System;
using UnityEngine;

namespace LSH_Lib
{
	public class AudioManager : MonoBehaviour
	{
		public Sound[] sounds;

        public static AudioManager instance;
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
            foreach(Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                //s.source.volume = s.volume;
                //s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        }
        private void Update()
        {
            foreach(Sound s in sounds)
            {
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
            }
        }
        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if(s == null)
            {
                Debug.LogError("can not find audio" + name + " source file");
            }
            s.source.Play();
        }
        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Stop();
        }
    }
}
