using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LSH_Lib{
    public class BoxController : MonoBehaviour
    {
        public TMP_Text interactText;
        public SliderControll sliderControll;
        public CylinderMove Exorcist;
        
        [SerializeField]
        bool canInteract = true;
        [SerializeField]
        bool isEmpty = true;
        bool isclick;
        string PlayerTag;

        private void Start()
        {
            interactText.enabled = false;
            sliderControll.Invisible();
        }
        private void Update()
        {
            KeyUp();
        }
        private void OnTriggerStay(Collider other)
        {
            
            if (other.CompareTag("Exorcist"))
            {
                PlayerTag = "Exorcist";
                //if (isEmpty && Exorcist.states.hasDoll)
                if (isEmpty)
                {
                    DoInteraction(false);
                }
            }

            if (other.CompareTag("Doll"))
            {
                PlayerTag = "Doll";
                if (!isEmpty)
                {
                    DoInteraction(true);
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
            GUI.Box(new Rect(0, 60, 150, 30), "BoxEmpty : " + isEmpty.ToString());
        }

        void DoInteraction(bool hasDoll)
        {
            
            if (Input.GetKey(KeyCode.Mouse0))
            {
                isclick = true; 

                sliderControll.visible();
                interactText.enabled = false;

                if (PlayerTag == "Exorcist")
                {
                    sliderControll.AutoCasting(10.0f);
                    this.isEmpty = hasDoll;
                    Exorcist.states.hasDoll = false;
                }
                if(PlayerTag == "Doll")
                {
                    sliderControll.Casting(1.0f);
                    if(sliderControll.slider.value.Equals(1.0f))
                    {
                        this.isEmpty = true;
                        sliderControll.Invisible();
                        interactText.enabled = false;
                    }
                }
            }
            else
            {
                interactText.enabled = true;
                sliderControll.Invisible();
            }
        }

        void KeyUp()
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if(PlayerTag == "Doll")
                {
                    isclick = false;
                    sliderControll.Initialized();
                }
                if(PlayerTag == "Exorcist")
                {   
                    sliderControll.visible();
                }
            }
        }
    }
}
