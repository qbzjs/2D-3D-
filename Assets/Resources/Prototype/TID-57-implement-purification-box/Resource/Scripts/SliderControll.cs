using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LSH_Lib{
    public class SliderControll : MonoBehaviour
    {
        public Slider slider;
        private void Start()
        {
            slider = GetComponentInChildren<Slider>();
        }
        private void Update()
        {
            Casting();
        }
        public void initializationGauge()
        {
            slider.value = 0.0f;
        }
        public void addValue(float speed)
        {
            slider.value += speed * Time.deltaTime;
        }
        private void Casting()
        {
            if(Input.GetKey(KeyCode.Mouse0))
            {
                slider.value += 0.1f * Time.deltaTime;
            }
            if(Input.GetKeyUp(KeyCode.Mouse0))
            {
                slider.value = 0.0f;
            }
        }
        private void AutoCasting()
        {
            
        }
    }
}
