using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class CustomRoomUIController : MonoBehaviour
	{
		[Header("CustomRoomCanvas")]
		Canvas customRoomCanvas;
		
		[Header("Create Custom Room Popup Window")]
		[SerializeField]
		GameObject createCustomRoom;
		[SerializeField]
		GameObject copy;

		[Header("Custom Room UI")]
		[SerializeField]
		GameObject customRoomLobby;
		[SerializeField]
		GameObject SelectDollCharacter;
		

		void EnableAllCanvas()
        {
			customRoomCanvas.gameObject.SetActive(true);
        }
		void DisalbeAllCanvas()
        {
			customRoomCanvas.enabled = false;
        }
		void EnableAllPanel()
        {
			createCustomRoom.SetActive(true);
        }
		void DisableAllPanel()
		{
			createCustomRoom.SetActive(false);
		}
		void EnableMainLobbyCanvas()
        {

        }
        void EnableCustomRoom()
		{ 
        }

    }
}
