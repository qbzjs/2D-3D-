using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GHJ_Lib
{
    public class SceneManger : MonoBehaviour
    {
        #region Public Fields
        public static SceneManger Instance;
        public GameObject UIPrefab;
        public bool IsCoroutine = false;
        #endregion

        #region Private Fields
        private List<GameObject> objList;
        private InteractionUI playerUI;
        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            Instance = this;
            if (UIPrefab == null)
            {
                Debug.LogError("Missing Prefab");
            }

            playerUI = Instantiate(UIPrefab).GetComponent<InteractionUI>();
        }

        void Update()
        {

        }
        #endregion

        #region Public Methods
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

        public void EnableAutoCastingBar(float chargeTime)
        {
            playerUI.ActiveAutoCastingBar(chargeTime);
        }



        #endregion

        #region Private Methods
        #endregion

        #region Interface Interaction





        #endregion
    }
}