using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LSH_Lib{
    public class EscMenu : MonoBehaviour
    {
        public GameObject ExitButton;
        public GameObject OptionUIPanel;
        public bool isInputKey = false;
        private void Update()
        {
            EscButtonInput();
        }
        private void EscButtonInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isInputKey)
                {
                    isInputKey = true;
                    OnSetActive(isInputKey);

                }
                else
                {
                    isInputKey = false;
                    OnSetActive(isInputKey);
                }
            }
        }
        private void OnSetActive(bool isInput)
        {
            ExitButton.SetActive(isInput);
            OptionUIPanel.SetActive(isInput);
        }
        public void OnExitButton()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("01Game");
        }
    }
}
