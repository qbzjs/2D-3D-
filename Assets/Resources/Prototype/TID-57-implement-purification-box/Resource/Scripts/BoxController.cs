using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LSH_Lib{
    public class BoxController : MonoBehaviour
    {
        public TMP_Text interactText;
        public GameObject slider;
        [SerializeField]
        bool canInteract = true;
        [SerializeField]
        bool hasDoll = false;
        bool isclick;

        private void Start()
        {
            interactText.enabled = false;
            slider.SetActive(false);
        }
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                isclick = false;
                slider.SetActive(false);
                interactText.enabled = true;
            }
        }
        private void OnTriggerStay(Collider other)
        {

            if (other.CompareTag("Exorcist"))
            {
                if (canInteract && !hasDoll)
                {
                    DoInteraction(true, false);
                }
            }

            if (other.CompareTag("Doll"))
            {
                if (hasDoll && !canInteract)
                {
                    DoInteraction(false, true);
                }
            } 
            
        }
        private void OnTriggerExit(Collider other)
        {
            interactText.enabled = false;
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(0, 0, 150, 30), isclick.ToString());
        }

        void DoInteraction(bool hasDoll, bool canInteract)
        {
            interactText.enabled = true;
            if (Input.GetKey(KeyCode.Mouse0))
            {
                isclick = true;
                slider.SetActive(true);
                interactText.enabled = false;
                this.hasDoll = hasDoll;
                this.canInteract = canInteract;
            }
        }
    }
}
