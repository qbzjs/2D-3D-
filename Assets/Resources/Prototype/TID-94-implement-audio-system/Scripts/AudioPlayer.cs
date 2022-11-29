using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class AudioPlayer : MonoBehaviour
	{
        [SerializeField]
        AudioSource[] AudioSources;
        int myIdx;
        private void Start()
        {
            //myIdx = AudioManager.instance.AddAudio(AudioSources[0]);
        }
        public void Play(string name)
        {
            //AudioManager.instance.Play(myIdx, name);
            for (int i = 0; i < AudioSources.Length; ++i)
            {
                if (!AudioSources[i].isPlaying)
                {
                    AudioManager.instance.PlayNew(name, AudioSources[i]);
                    return;
                }
            }
        }
        public void Stop(string name)
        {
            AudioManager.instance.Stop(myIdx,name);
        }
    }
}
