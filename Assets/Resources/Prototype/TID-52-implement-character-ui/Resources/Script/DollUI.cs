using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GHJ_Lib
{ 
    public class DollUI : MonoBehaviour
    {
        #region Public Fields
        public Image DollHP;
        public Image DevilHP;
        #endregion	

        #region Private Fields
        private DollStatus dollStatus = null;
        private float maxDollHP;
        private float maxDevilHP;
        #endregion	

        #region MonoBehaviour CallBacks
        void Start()
        {
        }

        void Update()
        {
            if (dollStatus == null)
            {
                return;
            }

            DollHP.fillAmount = dollStatus.DollHitPoint / maxDollHP;
            DevilHP.fillAmount = dollStatus.DevilHitPoint / maxDevilHP;
        }
        #endregion	

        #region Public Methods
        public void SetStatus(DollStatus dollStatus)
        {
            this.dollStatus = dollStatus;
            maxDollHP = dollStatus.DollHitPoint;
            maxDevilHP = dollStatus.DevilHitPoint;
        }
        #endregion	

        #region Private Methods
        #endregion	
    }
}
