using UnityEngine.Audio;
using System.Collections.Generic;
using System;
using UnityEngine;

using KSH_Lib;
using KSH_Lib.Data;

namespace LSH_Lib
{
    public class AudioManager : MonoBehaviour
    {
        public enum PlayTarget
        {
            All,
            Exorcist,
            Doll
        }

        [SerializeField]
        //AudioSource audiosource;
        public Sound[] sounds;
        [SerializeField]
        List<AudioSource> audioSources = new List<AudioSource>();

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

            //foreach (Sound s in sounds)
            //{
            //    if (audiosource == null)
            //    {
            //        s.source = gameObject.AddComponent<AudioSource>();
            //    }
            //    else
            //    {
            //        s.source = audiosource;
            //    }
            //    s.source.clip = s.clip;
            //    s.source.volume = s.volume;
            //    s.source.pitch = s.pitch;
            //    s.source.outputAudioMixerGroup = s.mixer;
            //    s.source.loop = s.loop;
            //}
        }
      
        public int AddAudio(AudioSource audioSource)
        {
            audioSources.Add(audioSource);
            return audioSources.Count-1;
        }
        public void Play(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            s.source.Play();
        }
        public void Play(int index, string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            audioSources[index].clip = s.clip;
            audioSources[index].outputAudioMixerGroup = s.mixer;
            audioSources[index].volume = s.volume;
            audioSources[index].Play();
        }

        public void PlayNew(string name, AudioSource audioSource)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            audioSource.clip = s.clip;
            audioSource.outputAudioMixerGroup = s.mixer;
            audioSource.volume = s.volume;
            audioSource.Play();
        }

        public void Play(string name, PlayTarget type)
        {
            var localData = DataManager.Instance.LocalPlayerData;

            if (localData == null || localData.roleData == null || localData.roleData.Group == RoleData.RoleGroup.Null)
            {
                Play(name);
                return;
            }

            switch (type)
            {
                case PlayTarget.All:
                {
                    Play(name);
                }
                break;
                case PlayTarget.Exorcist:
                {
                    if (localData.roleData.Group == RoleData.RoleGroup.Exorcist)
                    {
                        Play(name);
                    }
                }
                break;
                case PlayTarget.Doll:
                {
                    if(localData.roleData.Group == RoleData.RoleGroup.Doll)
                    {
                        Play(name);
                    }
                }
                break;
            }
        }
        public void Stop(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);

            s.source.Stop();
        }
        public void Stop(int index, string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            audioSources[index].clip = s.clip;
            audioSources[index].outputAudioMixerGroup = s.mixer;
            audioSources[index].volume = s.volume;
            audioSources[index].Stop();
        }
    }
}
