using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace LSH_Lib
{
	public class DoorAudio : MonoBehaviour
	{
        [SerializeField]
        AudioSource doorAudio;
		[SerializeField]
		Sound[] sounds;
        int myIdx;
        private void Start()
        {
            myIdx = AudioManager.instance.AddAudio(doorAudio);
        }
        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source = doorAudio;
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.outputAudioMixerGroup = s.mixer;
            s.source.loop = s.loop;
            s.source.Play();
        }
    }
}
