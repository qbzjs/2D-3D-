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
        int myIdx;
        private void Start()
        {
            myIdx = AudioManager.instance.AddAudio(rabbitAudio);
        }
        public void Play(string name)
        {
            AudioManager.instance.Play(myIdx, name);
        }
        public void Stop(string name)
        {
            AudioManager.instance.Stop(name);
        }
    }
}
