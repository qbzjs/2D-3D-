using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class DollHit : MonoBehaviour
    {
        #region Public Fields
        #endregion

        #region Private Fields
        private DollStatus dollStatus = null;
        private DollAnimationController dollanimator;
        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            dollanimator = GetComponent<DollAnimationController>();
            dollStatus = GetComponent<NetworkTPV_CharacterController>().GetStatus();
            if (dollStatus == null)
            {
                Debug.LogError("Missing Status");
                return;
            }

        }

        void Update()
        {
            Debug.Log("Doll HP: " + dollStatus.DollHitPoint);
            
        }


        public void HitDoll(int offensePower)
        {

            dollStatus.HitDollHP(offensePower);
            dollanimator.PlayHitAnimation();

        }
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
