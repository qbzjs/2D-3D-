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

        void EnableCustomRoom()
        {
            customRoom.SetActive(true);
        }
        void EnableDollSelect()
        {
            dollSelect.SetActive(true);
        }

    }
}
