using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace LSH_Lib{
    public class BoxController : MonoBehaviour
    {
        public TMP_Text interactText;

        [SerializeField]
        bool isActive = false;
        [SerializeField]
        bool isFull = false;

        private void Start()
        {
            interactText.enabled = false;
        }
        private void OnTriggerStay(Collider other)
        {
            if (isActive)
            {
                if (other.CompareTag("Exorcist"))
                { 
                    DoInteraction(true, false);
                }
            }

            if (isFull)
            {
                if (other.CompareTag("Doll"))
                {
                    DoInteraction(false, true);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            interactText.enabled = false;
        }

        void DoInteraction(bool isFull, bool isActive)
        {
            interactText.enabled = true;
            if (Input.GetKey(KeyCode.Mouse0))
            {
                this.isFull = isFull;
                interactText.enabled = false;
                this.isActive = isActive;
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                interactText.enabled = true;
            }
        }

    }
}
