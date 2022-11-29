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

using DEM;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace LSH_Lib
{
	public class CustomRoomUIController : MonoBehaviourPunCallbacks
	{
        [SerializeField] string loadSceneName;
        [SerializeField] LobbyUI_Manager uiManager;

        [Header("Popup Menu")]
        [SerializeField]
        TextMeshProUGUI roomcode;
        [SerializeField]
        int CustomRoomNameLength;
        [SerializeField]
        GameObject copysuccess;
        string roomName;

        [SerializeField]
        GameObject dollSelect;
        [SerializeField]
        GameObject ExorcistSelect;

        [Header("In Room")]
        [SerializeField]
        GameObject invite;

        void InstanceRoomCode()
        {
            roomcode.text = uiManager.CreateRandomRoomName(CustomRoomNameLength);
        }
        
        void DisalbeAll()
        {
            dollSelect.SetActive(false);
            ExorcistSelect.SetActive(false);
        }
        void EnableDollSelect()
        {
            DisalbeAll();
            dollSelect.SetActive(true);
        }
        void EnableExorcistSelect()
        {
            DisalbeAll();
            ExorcistSelect.SetActive(true);
        }
        void EnableInvite()
        {
            invite.SetActive(true);
        }

        void DisableInvite()
        {
            invite.SetActive(false);
        }
        void CopyButton()
        {
            GUIUtility.systemCopyBuffer = roomcode.text;
            copysuccess.SetActive(true);
            StartCoroutine(DestroyText());
        }

        void CreateCustomRoom()
        {
            uiManager.roomType = LobbyUI_Manager.RoomType.Custom;
            PhotonNetwork.CreateRoom(roomcode.text, new RoomOptions { MaxPlayers = GameManager.Instance.MaxPlayerCount });
        }

        IEnumerator DestroyText()
        {
            yield return new WaitForSeconds(1.0f);
            copysuccess.SetActive(false);

        }
        
    }
}
