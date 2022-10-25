using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;

namespace GHJ_Lib
{
	public class FinalAltar: Interaction
	{
		/*--- Public Fields ---*/
		public static FinalAltar Instance { get { return instance; } }
		public int DollCount
		{
			get { return dollCount; }
		}
		public int NeedAltarCount
		{
			get { return needAltarCount; }
		} 

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/
		static FinalAltar instance;
		int dollCount;
		int needAltarCount = 4;
		bool canOpenDoor = false;
		/*--- MonoBehaviour Callbacks ---*/
		void OnEnable()
		{
			instance = this;
			dollCount = PhotonNetwork.CurrentRoom.PlayerCount;
			curGauge = 0.0f;
			CanActiveToExorcist = false;
			CanActiveToDoll = false;
		}

        void Update()
        {
			if (!canOpenDoor)
			{
				return;
			}

			if (curGauge >= 1.0f)
			{
				CanActiveToDoll = false;
				OpenDoor();
			}
        }

		/*--- Interaction Methods ---*/

		public void DisableNormalAltar()
		{
			photonView.RPC("DecreaseAltarCount", RpcTarget.All);
		}

		[PunRPC]
        public void DecreaseAltarCount()
		{
			if (needAltarCount > 0)
			{
				needAltarCount--;
			}

			if (needAltarCount <= 0)
			{
				CanActiveToDoll = true;
				canOpenDoor = true;
			}
		}
		
		public override void Interact(BasePlayerController controller)
		{
			if (controller is DollController)
			{
				BarUI.Instance.SetTarget(this);
				Casting(controller);
			}
			if (controller is ExorcistController)
			{
				BarUI.Instance.SetTarget(null);
				AutoCasting(controller);
			}
		}

		
		protected override void Casting(BasePlayerController controller)
		{
			//controller에서 PlayerData 를 호출하고 interact Velocity를 받음.	
			float velocity = 10.0f;
			curGauge += velocity * Time.deltaTime;
		}
		protected override void AutoCasting(BasePlayerController controller)
		{
			//controller에서 PlayerData 를 호출하고 interact Velocity를 받음.	
			float velocity = 10.0f;
			BarUI.Instance.AutoCastingNull(maxGauge / velocity);
		}


		/*---EndManager---*/
		public void EndGameAlone()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			PhotonNetwork.LeaveRoom();
			GameManager.Instance.LoadScene("99_GameResultScene");
		}


		[PunRPC]
		public void DollCountDecrease()
		{
			if (dollCount > 0)
			{
				--dollCount;
			}

			if (dollCount == 1)
			{
				EndGame();
			}
		}

		public void EndGame()
		{
			photonView.RPC("ExitMapAlone", RpcTarget.All);
		}
		public void ExitDoll(GameObject other)
		{
			if (!other.CompareTag("Doll"))
			{
				return;
			}
			photonView.RPC("DollCountDecrease", RpcTarget.All);
			if (other.GetComponent<PhotonView>().IsMine)
			{
				EndGameAlone();
			}
			else
			{
				other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
			}
		}
		public void ExitDoll(Collider other)
		{
			ExitDoll(other.gameObject);
		}

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
		void OpenDoor()
		{
			if (this.transform.position.y < -this.transform.localScale.y)
			{
				return;
			}
			this.transform.position -= new Vector3(0, Time.deltaTime, 0);
		}
	}
}