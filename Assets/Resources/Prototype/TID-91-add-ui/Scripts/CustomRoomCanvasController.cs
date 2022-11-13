using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

using KSH_Lib;
using KSH_Lib.Data;
using KSH_Lib.UI;

namespace LSH_Lib
{
    public class CustomRoomCanvasController : MonoBehaviour
    {
        [SerializeField]
        Image[] player;
        [SerializeField]
        Sprite exorcistSprite;
        [SerializeField]
        Sprite dollSprite;

        private void Update()
        {
            for(var i = 0; i<PhotonNetwork.CurrentRoom.PlayerCount;++i)
            {
                player[i].sprite = dollSprite;
            }
        }
    } 
}

