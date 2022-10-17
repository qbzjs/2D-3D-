using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace LSH_Lib{
    public class PurificationBoxUI : MonoBehaviour
    {
        public GameObject sliderUI;
        public TMP_Text interactText;
        public Slider slider;

        bool isclick;
        ObjGenerator target;

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
            float time = speed * Time.deltaTime;
            while (value <= 1.0f)
            {
                yield return new WaitForSeconds(0.1f);
                slider.value += time;
                value += time;
            }

            Initialized();
            SliderInvisible();

        }
        public bool CheckValue()
        {
            if (slider.value.Equals(1.0f))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Initialized()
        {
            slider.value = 0.0f;
        }
        public void SetTarget(ObjGenerator target)
        {
            if (target == null)
            {
                Debug.LogError("BoxUIController's target is null");
                return;
            }
            this.target = target;
        }
        public void UIInvisible()
        {
            TextInvisible();
            SliderInvisible();
        }
        public void Slidervisible()
        {
            sliderUI.SetActive(true);
        }

        public void SliderInvisible()
        {
            sliderUI.SetActive(false);
        }
        public void TextInvisible()
        {
            interactText.enabled = false;
        }
        public void TextVisible()
        {
            interactText.enabled = true;
        }
    }
}
