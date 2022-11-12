using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace LSH_Lib
{
	public class OptionUIController : MonoBehaviour
	{
		[Header("Option UI")]
		[SerializeField]
		GameObject optionPanel;

        [SerializeField]
        GameObject settingWindow;
        [SerializeField]
        GameObject controllWindow;

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
        
        void EnableOptionPanel()
        {
            optionPanel.SetActive(true);
        }
        void DisableAllOptionPanel()
        {
            settingWindow.SetActive(false);
            controllWindow.SetActive(false);
        }
        void EnableSettingWindow()
        {
            DisableAllOptionPanel();
            settingWindow.SetActive(true);
        }
        void EnableControllWindow()
        {
            DisableAllOptionPanel();
            controllWindow.SetActive(true);
        }

        void SetMasterVolumeText()
        {
            int volumvalue = (int)mastervolume.value;
            masterVolumeText.text = volumvalue.ToString();
        }
        void SetSFXVolueText()
        {
            int volumvalue = (int)sfxVolume.value;
            sfxVolumeText.text = volumvalue.ToString();
        }
        void BackgroundVolumeText()
        {
            int volumvalue = (int)backgroundVolume.value;
            backgroundVolumeText.text = volumvalue.ToString();
        }
        void MouseSliderText()
        {
            int value = (int)mouseSlider.value;
            mouseSliderText.text = value.ToString();
        }
        void ResetAllVolume()
        {
            mastervolume.value = 80;
            masterVolumeText.text = mastervolume.value.ToString();
            sfxVolume.value = 80;
            sfxVolumeText.text = sfxVolume.value.ToString();
            backgroundVolume.value = 80;
            backgroundVolumeText.text = backgroundVolume.value.ToString();
        }
    }
}
