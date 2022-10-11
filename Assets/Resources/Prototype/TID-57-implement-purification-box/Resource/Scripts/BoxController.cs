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
        public GameObject sliderControll;
        [SerializeField]
        bool canInteract = true;
        [SerializeField]
        bool isEmpty = true;
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
                sliderControll.GetComponent<SliderControll>().Initialized();
                isclick = false;
                slider.SetActive(false);
            }
        }
        private void OnTriggerStay(Collider other)
        {

            if (other.CompareTag("Exorcist"))
            {
                if (isEmpty)
                {
                    DoInteraction(false);
                }
            }

            if (other.CompareTag("Doll"))
            {
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
            //interactText.enabled = true;
            if (Input.GetKey(KeyCode.Mouse0))
            {
                isclick = true;
                slider.SetActive(true);
                interactText.enabled = false;
                if (!hasDoll)
                {
                    sliderControll.GetComponent<SliderControll>().AutoCasting(2.0f);
                }
                else if(hasDoll)
                {
                    sliderControll.GetComponent<SliderControll>().Casting(1.0f);
                }
                if (sliderControll.GetComponentInChildren<Slider>().value == 1.0f)
                {
                    this.isEmpty = hasDoll;
                }
            }
            else
            {
                interactText.enabled = true;
            }
        }
    }
}
