using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib
{
    public class ChasingSound : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        public void Play()
        {
            audioSource.volume = 0.0f;

            audioSource.Play();
            StartCoroutine( ChangeVolume( ChangeValue, 0.0f, 1.0f, 1.0f ) );
        }
        public void Stop()
        {
            audioSource.volume = 1.0f;
            StartCoroutine( StopSound( ChangeValue, 0.0f, 1.0f, 1.0f ) );
        }


        void ChangeValue(float input)
        {
            audioSource.volume = input;
        }

        IEnumerator StopSound( System.Action<float> ChangeValue, float from, float to, float time )
        {
            yield return ChangeVolume( ChangeValue, from, to, time );
            yield return new WaitForSeconds( time );
            audioSource.Stop();
        }

        IEnumerator ChangeVolume(System.Action<float> ChangeValue, float from, float to, float time)
        {
            float delta = (to - from) / time;
            float desire = 0.0f;

            while ( true )
            {
                if(desire >= to)
                {
                    break;
                }
                desire += delta;

                ChangeValue( desire );

                yield return false;
            }
            yield return true;
        }
    }
}