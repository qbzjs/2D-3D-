using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LSH_Lib{
    public class SliderControll : MonoBehaviour
    {
        public GameObject sliderUI;
        bool isclick;
        public Slider slider;
        private void Start()
        {
            slider = GetComponentInChildren<Slider>();
        }

        public void Casting(float speed)
        {
            slider.value += speed * Time.deltaTime;
        }
        public void AutoCasting(float speed)
        {
            StartCoroutine("Cast", speed);
            
        }
        IEnumerator Cast(float speed)
        {
           slider.value = 0.0f;
           float value = 0.0f;

            while (value <= 100.0f)
            {
                yield return new WaitForSeconds(speed * Time.deltaTime);
                Casting(speed);
                value += 10.0f;
            }
            Initialized();
            Invisible();
        }
        public void Initialized()
        {
            slider.value = 0.0f;
        }
        public void visible()
        {
            sliderUI.SetActive(true);
        }
        public void Invisible()
        {
            sliderUI.SetActive(false);
        }
    }
}
