using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LSH_Lib
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance;
        public GameObject UIPrefab;
        public bool IsCoroutine = false;
        
        private List<GameObject> objList;
        private InteractionUI playerUI;

        void Start()
        {
            Instance = this;
            if (UIPrefab == null)
            {
                Debug.LogError("Missing Prefab");
            }

            playerUI = Instantiate(UIPrefab).GetComponent<InteractionUI>();
        }

        public void EnableInteractionText()
        {
            playerUI.ActiveInteractionText();
        }

        public void DisableInteractionText()
        {
            playerUI.DeactiveInteractionText();
        }

        public void EnableCastingBar(GameObject obj)
        {
            playerUI.ActiveCastingBar(obj);
        }

        public void DisableCastingBar()
        {
            playerUI.DeactiveCastingBar();
        }

        public void EnableAutoCastingNullBar(float chargeTime)
        {
            playerUI.ActiveAutoCastingNullBar(chargeTime);
        }

        public void EnableAutoCastingBar(GameObject obj, float chargeTime)
        {
            playerUI.ActiveAutoCastingBar(obj,chargeTime);
        }

    }
}