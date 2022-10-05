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
        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            
        }

        void Update()
        {
            if (dollStatus == null)
            {
                Debug.LogError("Missing Status");
                dollStatus = GetComponent<NetworkTPV_CharacterController>().GetStatus();
                return;
            }

            Debug.Log(dollStatus.DollHitPoint);
        }

        public void HitDoll(int offensePower)
        {
            dollStatus.HitDollHP(offensePower);
        }
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
