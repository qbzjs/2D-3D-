using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using Photon.Pun;
using Photon.Realtime;


using KSH_Lib;
using KSH_Lib.Data;
using KSH_Lib.UI;

using DEM;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace LSH_Lib
{
    public class CustomRoomCanvasController : MonoBehaviourPunCallbacks
    {
        [Header("Popup Menu")]
        [SerializeField]
        TextMeshProUGUI roomcode;
        [SerializeField]
        GameObject copysuccess;
        string roomName;

        void CopyButton()
        {
            GUIUtility.systemCopyBuffer = roomcode.text;
            copysuccess.SetActive(true);
            StartCoroutine(DestroyText());
        }
        IEnumerator DestroyText()
        {
            yield return new WaitForSeconds(1.0f);
            copysuccess.SetActive(false);
        }
        void CreateRoom()
        {
            roomName = roomcode.text;
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = GameManager.Instance.MaxPlayerCount });
        }
        
    } 
}

