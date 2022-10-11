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
        public void OnEnable()
        {
            dollanimator = GetComponent<DollAnimationController>();
            dollStatus = GetComponent<DollStatus>();
            if (dollStatus == null)
            {
                Debug.LogError("Missing Status");
                return;
            }

        }
        /*
        public void HitDoll(int offensePower)
        {

            dollStatus.HitDollHP(offensePower);
            dollanimator.PlayHitAnimation();

        }
        */
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
