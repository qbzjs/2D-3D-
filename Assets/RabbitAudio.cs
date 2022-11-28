using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

namespace LSH_Lib
{
	public class RabbitAudio : MonoBehaviour
	{
		[SerializeField]
		AudioSource rabbitAudio;
        [SerializeField]
        Sound[] sounds;
        int myIdx;
        private void Start()
        {
            myIdx = AudioManager.instance.AddAudio(rabbitAudio);
            //foreach(Sound s in sounds)
            //{
            //    s.source = rabbitAudio;
            //    s.source.clip = s.clip;
            //    s.source.volume = s.volume;
            //    s.source.pitch = s.pitch;
            //    s.source.outputAudioMixerGroup = s.mixer;
            //    s.source.loop = s.loop;
            //}
        }
        public void PlaySound(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source = rabbitAudio;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = s.mixer;
            s.source.loop = s.loop;
            s.source.Play();
        }
        public void Stop(string name)
        {
            AudioManager.instance.Stop(name);
        }
    }
}
