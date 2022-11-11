using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class OptionUIController : MonoBehaviour
	{
		[Header("Setting UI")]
		[SerializeField]
		GameObject optionPanel;

        [SerializeField]
        GameObject settingWindow;
        [SerializeField]
        GameObject controllWindow;

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
        void EnableSettingWindow()
        {
            controllWindow.SetActive(false);
            settingWindow.SetActive(true);
        }
        void EnableControllWindow()
        {
            settingWindow.SetActive(false);
            controllWindow.SetActive(true);
        }
    }
}
