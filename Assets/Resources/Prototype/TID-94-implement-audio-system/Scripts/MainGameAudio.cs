using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using LSH_Lib;
public class MainGameAudio : MonoBehaviour
{
    public AudioSource audioSource;
    int myidx;
    private void Start()
    {
        myidx = AudioManager.instance.AddAudio(audioSource);
        AudioManager.instance.Play(myidx, "BGM");
    }
}
