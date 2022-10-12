using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon;
namespace LSH_Lib{
    public class SliderControll : MonoBehaviourPun
    {
        public GameObject sliderUI;
        bool isclick;
        public Slider slider;
        private void Start()
        {
            slider = GetComponentInChildren<Slider>();
        }
        [PunRPC]
        public void Casting(float speed)
        {
            slider.value += speed * Time.deltaTime;
        }
        [PunRPC]
        public void AutoCasting(float speed)
        {
            StartCoroutine("Cast", speed);
            //Initialized();
            //Invisible();
        }
        [PunRPC]
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
        [PunRPC]
        public void Initialized()
        {
            slider.value = 0.0f;
        }
        [PunRPC]
        public void visible()
        {
            sliderUI.SetActive(true);
        }
        [PunRPC]
        public void Invisible()
        {
            sliderUI.SetActive(false);
        }
    }
}
