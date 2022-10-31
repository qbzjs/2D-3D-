using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;

namespace GHJ_Lib
{
	public class FinalAltar: InteractionObj
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
		ExitAltar exitAltar;
		/*--- MonoBehaviour Callbacks ---*/
		void OnEnable()
		{
			exitAltar = GameObject.Find("ExitAltar(Clone)").GetComponent<ExitAltar>();
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

        public override CastingType GetCastingType(NetworkBaseController player)
		{
			if (player is DollController)
			{
				if (curGauge >= 1.0f)
				{
					return CastingType.NotCasting;
				}

				if (canOpenDoor)
				{ 
					return CastingType.ManualCasting;
				}

				
			}		
			return CastingType.NotCasting;
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

			if (dollCount == 2)
			{
				exitAltar.OpenExitAltar();
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