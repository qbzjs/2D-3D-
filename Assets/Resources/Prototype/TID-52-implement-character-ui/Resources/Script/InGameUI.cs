using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GHJ_Lib
{ 
    public class InGameUI : MonoBehaviour
    {

        protected void ApplyStatusHPToHPUI(DollStatus dollStatus, Image DollHPImage, Image DevilHPImage)
        {
            if (dollStatus == null)
            {
                Debug.LogError("Missing status");
                return;
            }

            DollHPImage.fillAmount = dollStatus.CurrentRateOfDollHP;
            DevilHPImage.fillAmount = dollStatus.CurrentRateOfDevilHP;
        }



    }
}
