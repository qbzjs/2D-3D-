using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LSH_Lib
{
    public class InteractionUI : MonoBehaviour
    {
        #region Public Fields
        public Text InteractText
        {
            get { return interactionText; }
            set { interactionText = value; }
        }
        #endregion

        #region Private Fields
        [SerializeField]
        Slider bar;
        [SerializeField]
        Text interactionText;
        private GameObject targetObj;
        private bool isAutoCasting = false;
        private float autoCastingTime = 0.0f;
        #endregion

        #region MonoBehaviour CallBacks
         void Start()
        {
            Debug.Log("Start");
            bar = GetComponentInChildren<Slider>();
            bar.gameObject.SetActive(false);
            interactionText = GetComponentInChildren<Text>();
            interactionText.gameObject.SetActive(false);
            if (bar == null)
            {
                Debug.LogError("Missing Slider");
            }

        }

        void Update()
        {
            if (targetObj)
            {
                bar.value = targetObj.GetComponent<interaction>().GetGaugeRate;
            }

            if (isAutoCasting)
            {
                bar.value += autoCastingTime * Time.deltaTime;
            }

        }
        #endregion

        #region Public Methods
        public void ActiveInteractionText()
        {
            if (interactionText.gameObject.activeInHierarchy)
            {
                return;
            }
            interactionText.gameObject.SetActive(true);
        }

        public void DeactiveInteractionText()
        {
            if (!interactionText.gameObject.activeInHierarchy)
            {
                return;
            }
            interactionText.gameObject.SetActive(false);
        }

        public void ActiveCastingBar(GameObject _obj)
        {
            if (bar.gameObject.activeInHierarchy)
            {
                return;
            }
            SetTargetObj(_obj);
            bar.gameObject.SetActive(true);
        }

        public void DeactiveCastingBar()
        {
            if (!bar.gameObject.activeInHierarchy)
            {
                return;
            }
            SetTargetObjToNull();
            bar.gameObject.SetActive(false);
        }

        public void ActiveAutoCastingBar(GameObject _obj, float chargeTime)
        {
            if (!isAutoCasting)
            {
                SetTargetObj(_obj);
                bar.value = 0;
                StartCoroutine("AutoCasting", chargeTime);
            }
        }
        
        public void ActiveAutoCastingNullBar(float chargeTime)
        {
            if (!isAutoCasting)
            {
                SetTargetObjToNull();
                bar.value = 0;
                StartCoroutine("AutoCasting", chargeTime);
            }
        }


        #endregion

        #region Private Methods
        private void SetTargetObj(GameObject _obj)
        {
            if (targetObj == _obj)
            {
                return;
            }
            targetObj = _obj;
        }

        private void SetTargetObjToNull()
        {
            targetObj = null;
        }

        #endregion

        IEnumerator AutoCasting(float chargeTime)
        {
            while (true)
            {
                isAutoCasting = true;
                autoCastingTime = 1 / chargeTime;
                SceneManager.Instance.IsCoroutine = true;
                bar.gameObject.SetActive(true);
                interactionText.gameObject.SetActive(false);
                yield return new WaitForSeconds(chargeTime);

                if (bar.value >= 1.0f)
                {
                    isAutoCasting = false;
                    autoCastingTime = 0;
                    SceneManager.Instance.IsCoroutine = false;
                    bar.gameObject.SetActive(false);
                    interactionText.gameObject.SetActive(true);
                    break;
                }
            }
        }

    }
}
