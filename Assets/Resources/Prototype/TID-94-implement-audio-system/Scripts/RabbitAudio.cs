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
		AudioSource[] rabbitAudios;
        //int[] indices;
        //[SerializeField] AudioSource rabbitAudio;
        int myIdx;
        private void Start()
        {
            //indices = new int[rabbitAudios.Length];
            //myIdx = AudioManager.instance.AddAudio(rabbitAudio);
            // AddAudios();
            myIdx = AudioManager.instance.AddAudio(rabbitAudios[0]);
        }
        public void Play(string name)
        {
            //AudioManager.instance.Play(myIdx, name);
            for (int i = 0; i < rabbitAudios.Length; ++i)
            {
                if (!rabbitAudios[i].isPlaying)
                {
                    AudioManager.instance.PlayNew(name, rabbitAudios[i]);
                    return;
                }
            }
        }
        public void Play(string name, AudioManager.PlayTarget target)
        {
            //AudioManager.instance.Play(myIdx, name);
            for (int i = 0; i < rabbitAudios.Length; ++i)
            {
                if (!rabbitAudios[i].isPlaying)
                {
                    AudioManager.instance.PlayNew(name, rabbitAudios[i], target);
                    return;
                }
            }
        }
        public void Stop(string name)
        {
            AudioManager.instance.Stop(name);
        }
    }
}
