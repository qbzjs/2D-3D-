using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
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

        private void OnGUI()
        {
            GUI.Box(new Rect(Screen.width - 30, 0, Screen.width, 30), PhotonNetwork.CurrentRoom.PlayerCount.ToString());
        }

    }
}
