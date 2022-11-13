using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KSH_Lib;
using KSH_Lib.UI;

using Photon;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib
{
    public class SearchCustomUIController : MonoBehaviour
    {
        [SerializeField] LobbyUI_Manager uiManager;

        [SerializeField]
        TMP_InputField inputField;
        void JoinRoom()
        {
            uiManager.roomType = LobbyUI_Manager.RoomType.Custom;
            PhotonNetwork.JoinRoom(inputField.text);
        }
    }
}

