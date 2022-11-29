using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class AudioPlayer : MonoBehaviour
	{
        [SerializeField]
        AudioSource AudioSource;
        int myIdx;
        private void Start()
        {
            myIdx = AudioManager.instance.AddAudio(AudioSource);
        }
        public void Play(string name)
        {
            AudioManager.instance.Play(myIdx, name);
        }
        public void Stop(string name)
        {
            AudioManager.instance.Stop(myIdx,name);
        }
    }
}
