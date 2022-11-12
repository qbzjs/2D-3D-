using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class CustomRoomUIController : MonoBehaviour
	{
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
    }
}
