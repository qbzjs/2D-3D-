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
            StartCoroutine(FadeIn(3.0f));
        }
        public void Stop()
        {
            StartCoroutine(FadeOut(3.0f));
        }

        IEnumerator FadeOut(float FadeTime)
        {
            float startVolume = audioSource.volume;
            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
                yield return null;
            }
            audioSource.Stop();
        }

        IEnumerator FadeIn(float FadeTime)
        {
            audioSource.Play();
            audioSource.volume = 0f;
            while (audioSource.volume < 1)
            {
                audioSource.volume += Time.deltaTime / FadeTime;
                yield return null;
            }
        }

    }
}