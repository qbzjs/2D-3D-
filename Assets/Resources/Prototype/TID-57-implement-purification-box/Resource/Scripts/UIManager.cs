using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace LSH_Lib{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance { get { return _instance; } }
        public Slider interactSlider;
        private static UIManager _instance;
        private void Start()
        {
            if (_instance != null || this != _instance)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
            interactSlider = GetComponentInChildren<Slider>();
        }
        public void Autocasting(float speed)
        {
            interactSlider.value += speed * Time.deltaTime;
        }
        public void Casting()
        { }
    }
}
