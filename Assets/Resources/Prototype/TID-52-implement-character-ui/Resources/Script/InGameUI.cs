using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using LSH_Lib;
namespace GHJ_Lib
{
    public class InGameUI : MonoBehaviour
    {
        protected void ApplyStatusHPToHPUI(DollStatus dollStatus, Image DollHPImage, Image DevilHPImage)
        {
            if (dollStatus == null)
            {
                Debug.LogWarning("Missing status");
                return;
            }

            DollHPImage.fillAmount = dollStatus.CurrentRateOfDollHP;
            DevilHPImage.fillAmount = dollStatus.CurrentRateOfDevilHP;
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(Screen.width/2 , 0, 180, 30), "Player Number(Not Ghost) : " +  GameEndManager.Instance.DollCount);
        }
    }
}
