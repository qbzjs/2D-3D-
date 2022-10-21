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
        public bool IsMine=false;
        ObjGenerator target;

        private void Start()
        {
            slider = sliderUI.GetComponent<Slider>();
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
                if(slider.value.Equals(1))
                {
                    SliderInvisible();
                    TextInvisible();
                    yield break;
                }
            }
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
            sliderUI.SetActive(false);
            interactText.enabled = false;
        }
        public void Slidervisible()
        {
            if (IsMine)
            {
                sliderUI.SetActive(true);
            }
        }

        public void SliderInvisible()
        {
            if (IsMine)
            {
                sliderUI.SetActive(false);
            }
        }
        public void TextInvisible()
        {
            if (IsMine)
            {
                interactText.enabled = false;
            }
        }
        public void TextVisible()
        {
            if (IsMine)
            {
                interactText.enabled = true;
            }
        }
     

    }
}