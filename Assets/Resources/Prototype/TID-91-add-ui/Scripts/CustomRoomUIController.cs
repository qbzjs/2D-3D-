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
        [Header("Popup Menu")]
        [SerializeField]
        TextMeshProUGUI roomcode;
        [SerializeField]
        GameObject copysuccess;
        string roomName;

        [SerializeField]
        GameObject customRoom;
        [SerializeField]
        GameObject dollSelect;
        [SerializeField]
        GameObject ExorcistSelect;

        [Header("In Room")]
        [SerializeField]
        GameObject invite;
        
        
        void DisalbeAll()
        {
            dollSelect.SetActive(false);
            ExorcistSelect.SetActive(false);
        }
        void EnableCustomRoom()
        {
            customRoom.SetActive(true);
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
        IEnumerator DestroyText()
        {
            yield return new WaitForSeconds(1.0f);
            copysuccess.SetActive(false);

        }
        void CreateRoom()
        {
            roomName = roomcode.text;
            
        }
    }
}
