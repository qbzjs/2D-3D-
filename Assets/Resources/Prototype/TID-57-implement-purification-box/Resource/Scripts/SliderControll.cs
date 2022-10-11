using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LSH_Lib{
    public class SliderControll : MonoBehaviour
    {
        public Slider slider;
        bool isclick;
        private void Start()
        {
            slider = GetComponentInChildren<Slider>();
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(0, 30, 150, 30), "casting"+isclick.ToString());
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
            slider.value += speed * Time.deltaTime;
            yield return new WaitForSeconds(1.0f);
            
        }
        public void Initialized()
        {
            slider.value = 0.0f;
        }
    }
}
