using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TID22
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
                bar.gameObject.SetActive(true);
                interactionText.gameObject.SetActive(false);
                bar.value += autoCastingTime * Time.deltaTime;
            }
        }
        #endregion

        #region Public Methods
        public void ActiveInteractionText()
        {
            interactionText.gameObject.SetActive(true);
        }

        public void DeactiveInteractionText()
        {
            interactionText.gameObject.SetActive(false);
        }

        public void ActiveCastingBar(GameObject _obj)
        {
            SetTargetObj(_obj);
            bar.gameObject.SetActive(true);
        }

        public void DeactiveCastingBar()
        {
            SetTargetObjToNull();
            bar.gameObject.SetActive(false);
        }

        public void ActiveAutoCastingBar(float chargeTime)
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
                yield return new WaitForSeconds(chargeTime);

                if (bar.value >= 1.0f)
                {
                    isAutoCasting = false;
                    autoCastingTime = 0;
                    SceneManager.Instance.IsCoroutine = false;
                    bar.gameObject.SetActive(false);
                    break;
                }
            }

        }
    }
}
