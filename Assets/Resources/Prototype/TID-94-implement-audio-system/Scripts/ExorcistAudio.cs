using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace LSH_Lib
{
	public class ExorcistAudio : MonoBehaviour
	{
        [SerializeField]
        AudioSource audioSource;
        [SerializeField]
        Sound[] sounds;
        int myIdx;
        private void Start()
        {
            myIdx = AudioManager.instance.AddAudio(audioSource);
        }
        public void PlaySound(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source = audioSource;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = s.mixer;
            s.source.loop = s.loop;
            s.source.Play();
        }
        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Stop();
        }
    }
}
