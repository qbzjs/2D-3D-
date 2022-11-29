using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
namespace LSH_Lib
{
	public class optionUI : MonoBehaviour
	{
        [Header("Audio Mixer")]
        [SerializeField]
        AudioMixer mixer;
        [Header("Setting UI")]
        [SerializeField]
        Slider mastervolume;
        [SerializeField]
        Slider sfxVolume;
        [SerializeField]
        Slider backgroundVolume;
        [SerializeField]
        TextMeshProUGUI masterVolumeText;
        [SerializeField]
        TextMeshProUGUI sfxVolumeText;
        [SerializeField]
        TextMeshProUGUI backgroundVolumeText;

        void SetMasterVolume()
        {
            int volumvalue = (int)(mastervolume.value * 100);
            masterVolumeText.text = volumvalue.ToString();
            mixer.SetFloat("Master", Mathf.Log10(mastervolume.value) * 20);
        }
        void SetSFXVolue()
        {
            int volumvalue = (int)(sfxVolume.value * 100);
            sfxVolumeText.text = volumvalue.ToString();
            mixer.SetFloat("SFX", Mathf.Log10(sfxVolume.value) * 20);
        }
        void BackgroundVolume()
        {
            int volumvalue = (int)(backgroundVolume.value * 100);
            backgroundVolumeText.text = volumvalue.ToString();
            mixer.SetFloat("BGM", Mathf.Log10(backgroundVolume.value) * 20);
        }
        void ResetAllVolume()
        {
            mastervolume.value = 0.8f;
            masterVolumeText.text = (mastervolume.value * 100).ToString();
            sfxVolume.value = 0.8f;
            sfxVolumeText.text = (sfxVolume.value * 100).ToString();
            backgroundVolume.value = 0.8f;
            backgroundVolumeText.text = (backgroundVolume.value * 100).ToString();
        }
        
    }
}
