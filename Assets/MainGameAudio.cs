using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSH_Lib;
public class MainGameAudio : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.Play("BGM");
    }
}
