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
            //Initialized();
            //Invisible();
        }
        IEnumerator Cast(float speed)
        {
           slider.value = 0.0f;
           float value = 0.0f;
            float time = speed * Time.deltaTime;
            while (value <= 1.0f)
            {
                yield return new WaitForSeconds(0.1f);
                slider.value += time;
                value += time;
            }
            //yield return new WaitForSeconds(time);
            Initialized();
            Invisible();
            
            //Casting(speed);
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
