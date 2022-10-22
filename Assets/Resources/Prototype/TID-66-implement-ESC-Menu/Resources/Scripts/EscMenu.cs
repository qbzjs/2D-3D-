using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
namespace LSH_Lib{
    public class EscMenu : MonoBehaviour
    {
        public GameObject ExitButton;
        public GameObject OptionUIPanel;
        public FinalAltarInteraction finalAltar;
        private bool isInputKey = false;

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
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;


                    TPV_PlayerInputManager.instance.enabled = false;
                   
                    FPV_InputManager.instance.enabled = false;

                    PlayerInputManager.instance.enabled = false;

                    isInputKey = true;
                    OnSetActive(isInputKey);

                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;

                    TPV_PlayerInputManager.instance.enabled = true;

                    FPV_InputManager.instance.enabled = true;

                    PlayerInputManager.instance.enabled = true;

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
            if (!finalAltar)
            {
                finalAltar = GameObject.FindObjectOfType<FinalAltarInteraction>();
            }
            finalAltar.ExitPlayer();
            PhotonNetwork.LeaveRoom();
            GameManager.Instance.LoadScene("99_GameResultScene");
        }
    }
}
