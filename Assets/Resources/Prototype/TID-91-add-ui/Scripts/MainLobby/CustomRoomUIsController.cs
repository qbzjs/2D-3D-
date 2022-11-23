using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using KSH_Lib;
using KSH_Lib.Data;
using KSH_Lib.UI;

using Photon;
using Photon.Pun;
using Photon.Realtime;

namespace LSH_Lib
{
	public class CustomRoomUIsController : MonoBehaviour
	{
		[System.Serializable]
		public struct PlayerUI
        {
			public Image StateImage;
			public Image UnReadyImage;
			public TextMeshProUGUI PlayerName;
			public Image RoleType;

        }

		[Header("Player UIs")]
		[SerializeField]
		PlayerUI[] playerUIs;

		[Header("Sprites")]
		[SerializeField]
		Sprite[] rollSprites;
		[SerializeField]
		Sprite[] stateSprites;


        private void Start()
        {
			SetRoll();
			ResetImage();
        }
        private void Update()
        {
			ChangeImage();
        }
		void ResetImage()
        {
			for(int i = 0; i<playerUIs.Length; ++i)
            {
				playerUIs[i].StateImage.enabled = false;
				playerUIs[i].UnReadyImage.enabled = false;
				playerUIs[i].PlayerName.text = "기다리는 중...";
				playerUIs[i].RoleType.sprite = rollSprites[0];
            }
        }
        void SetRoll()
		{
			if (PhotonNetwork.IsMasterClient)
			{
				DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Exorcist;
			}
			else
			{
				DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Doll;
			}
		}
		void ChangeImage()
        {
			if(PhotonNetwork.IsMasterClient)
            {
				playerUIs[0].StateImage.sprite = stateSprites[0];
				playerUIs[0].StateImage.enabled = true;
				playerUIs[0].PlayerName.text = "Test";
				playerUIs[0].RoleType.sprite = rollSprites[2];
            }
			else
            {
				for(int i = 1; i<PhotonNetwork.CurrentRoom.PlayerCount-1; ++i)
                {
					playerUIs[i].StateImage.enabled = false;
					playerUIs[i].UnReadyImage.enabled = true;
					playerUIs[i].PlayerName.text = "Test";
					playerUIs[i].RoleType.sprite = rollSprites[2];
				}
            }
        }
	}
}
