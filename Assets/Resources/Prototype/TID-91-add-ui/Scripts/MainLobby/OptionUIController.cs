using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
namespace LSH_Lib
{
	public class OptionUIController : MonoBehaviour
	{
        [Header("Canvas")]
        [SerializeField]
        GameObject optionCanvas;
        [SerializeField]
        GameObject buttonPanel;
        
		[Header("Option UI")]
		[SerializeField]
		GameObject optionPanel;

        [SerializeField]
        GameObject settingWindow;
        [SerializeField]
        GameObject controllWindow;

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


        [Header("Controll UI")]
        [SerializeField]
        Slider mouseSlider;
        [SerializeField]
        TextMeshProUGUI mouseSliderText;


        private void Start()
        {
            optionPanel.SetActive(false);
            settingWindow.SetActive(true);
            controllWindow.SetActive(false);
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                EnableSettingWindow();
                DisableAllOptionPanel();
                optionPanel.SetActive(false);
                optionCanvas.SetActive(false);
                buttonPanel.SetActive(true);

            }
        }
        void EnableOptionPanel()
        {
            optionPanel.SetActive(true);
            settingWindow.SetActive(true);
        }
        void DisableAllOptionPanel()
        {
            settingWindow.SetActive(false);
            controllWindow.SetActive(false);
        }
        void EnableSettingWindow()
        {
            DisableAllOptionPanel();
            optionPanel.SetActive(true);
            settingWindow.SetActive(true);
        }
        void EnableControllWindow()
        {
            DisableAllOptionPanel();
            controllWindow.SetActive(true);
        }

        void SetMasterVolume()
        {
            int volumvalue = (int)(mastervolume.value * 100);
            masterVolumeText.text = volumvalue.ToString();
            mixer.SetFloat("Master", Mathf.Log10(mastervolume.value)*20);
        }
        void SetSFXVolue()
        {
            int volumvalue = (int)(sfxVolume.value * 100);
            sfxVolumeText.text = volumvalue.ToString();
            mixer.SetFloat("SFX", Mathf.Log10(sfxVolume.value) *20);
        }
        void BackgroundVolume()
        {
            int volumvalue = (int)(backgroundVolume.value * 100);
            backgroundVolumeText.text = volumvalue.ToString();
            mixer.SetFloat("BGM", Mathf.Log10(backgroundVolume.value)*20);
        }
        void MouseSliderText()
        {
            int value = (int)mouseSlider.value;
            mouseSliderText.text = value.ToString();
        }
        void SaveValue()
        {
            float value = mouseSlider.value;
            float mouseSensitivityX = Input.GetAxis("Mouse X") * value;
            float mouseSensitivityY = Input.GetAxis("Mouse Y") * value;
        }

        void ResetAllVolume()
        {
            mastervolume.value = 0.8f;
            masterVolumeText.text = (mastervolume.value*100).ToString();
            sfxVolume.value = 0.8f;
            sfxVolumeText.text = (sfxVolume.value * 100).ToString();
            backgroundVolume.value = 0.8f;
            backgroundVolumeText.text = (backgroundVolume.value * 100).ToString();
        }
    }
}
