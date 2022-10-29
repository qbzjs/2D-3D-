using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;

namespace GHJ_Lib
{
	public class DollController : NetworkBaseController, IPunObservable
	{

		/*--- Public Fields ---*/
		
		public BvIdle GetIdle
		{
			get { return idle; }
		}
		public int CrossStack
		{
            get { return crossStack; }
		}
		[SerializeField]
		protected GameObject GhostModel;
		/*--- Protected Fields ---*/

		protected BvIdle idle					= new BvIdle();
		protected BvDown down = new BvDown();
		protected BvHit hit = new BvHit();
		protected BvCaught caught = new BvCaught();
		protected BvCharacterInteraction interaction	= new BvCharacterInteraction();
		protected BvPurified purified = new BvPurified();
		protected BvEscape escape = new BvEscape();

		protected int crossStack = 0;

		/*--- Private Fields ---*/



		/*--- MonoBehaviour Callbacks ---*/
		public override void OnEnable()
		{
			base.OnEnable();
			//GhostModel.SetActive(false);
			// 스테이터스 받아오기

			//애니매이터 받기 -> behavior에서 할예정

			// 카메라 설정하기
			if (photonView.IsMine)
			{
				//인형인지 퇴마사인지에 따라서 Setactive 를 해줄것.
				fpvCam = GameObject.Find( "FPV_Cam(Clone)" ).GetComponent<KSH_Lib.FPV_CameraController>();
				fpvCam.InitCam(camTarget);
				fpvCam.gameObject.SetActive(false);
				tpvCam = GameObject.Find( "TPV_Cam(Clone)" ).GetComponent<TPV_CameraController>();
				tpvCam.InitCam(camTarget);
				tpvCam.gameObject.SetActive(true);
			}
			// CurcharacterAction, CurcharacterCondition,  초기설정하기
			CurCharacterAction.PushSuccessorState(idle);
			//CurCharacterCondition.PushSuccessorState

			//처음 대기시간 주기( 이건 StageManger가 할일)

			switch (typeIndex) //5~9 일단 임시로 만들어 놓은것.
			{
				case 5:
					{ }
					break;
				case 6:
					{ }
					break;
				case 7:
					{ }
					break;
				case 8:
					{ }
					break;
				case 9:
					{ }
					break;
			}


			//아직 인형은 하나밖에없기 때문에 위 switch문은 보여주기만 할것
			//PassiveSkill.PushSuccessorState();

		}


        private void OnTriggerStay(Collider other)
        {
			

			if (other.CompareTag("interactObj"))
			{
				interactObj =  other.GetComponent<Interaction>();

				if (!photonView.IsMine)
				{
					return;
				}

				if (IsAutoCasting)
				{
					canInteract = false;
					BarUI.Instance.SliderVisible(true);
					BarUI.Instance.TextVisible(false);
					return;
				}
				else if(IsCasting)
				{ 
					canInteract = true;
					BarUI.Instance.SliderVisible(true);
					BarUI.Instance.TextVisible(false);
					if (!interactObj.CanActiveToDoll)
					{
						canInteract = false;
					}
					return;
				}
				else
				{
					BarUI.Instance.SliderVisible(false);
				}

				if (Vector3.Dot(forward, Vector3.ProjectOnPlane((other.transform.position - this.transform.position), Vector3.up))>0&&
					interactObj.CanActiveToDoll)
				{
					BarUI.Instance.TextVisible(true);
					canInteract = true;
					
				}
				else
				{
					BarUI.Instance.TextVisible(false);
					canInteract = false;
				}
				
			}
        }

		private void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("interactObj"))
			{
				BarUI.Instance.TextVisible(false);
				BarUI.Instance.SliderVisible(false);
			}
		}


		/*--- Public Methods ---*/


		public void CaughtDoll(GameObject ExorcistCamTarget)
		{
			characterModel.gameObject.SetActive(false);
			ChangeCamera(ExorcistCamTarget);
			ChangeActionTo("Caught");
		}
		public void Escape(Transform transform, int layer)
		{
			this.transform.position = transform.position;
			this.transform.rotation = transform.rotation;
			characterModel.gameObject.SetActive(true);
			CharacterLayerChange(characterObj, layer);
			ChangeCamera(camTarget);
		}

		public void ChangeCamera(GameObject camTarget)
		{
			if (photonView.IsMine)
			{
				tpvCam.InitCam(camTarget);
			}
		}
		public void HitDamage()
		{
			if (CurCharacterAction is not BvHit)
			{
				if (photonView.IsMine)
				{ 
					ChangeActionTo("Hit");
				}
			}
			
		}
		public void Imprisoned(PurificationBox puriBox)
		{
			purified.SetPuriBox(puriBox);
			if (photonView.IsMine)
			{
				ChangeActionTo("Purified");
			}
		}
		public void BecomeGhost()
		{
			//에셋이 바뀐다
			characterModel.SetActive(false);
			GhostModel.SetActive(true);
			CharacterLayerChange(GhostModel, 8);
			ChangeActionTo("Idle");
			BaseAnimator.enabled = false;

			GhostModel.GetComponent<Animator>().Play("GhostIdle");
			

		}


		/*--HitByExorcistSkill--*/
		public void AprrochCrossArea()
		{
			crossStack++;
			if (crossStack > 5)
			{
				crossStack = 5;
				return;
			}

			switch (crossStack)
			{
				case 1:
					{
						//흔적 짙어짐
						//흔적 유지시간 길어짐 (데이터테이블에 있을지) 없다면 코루틴으로 ...
					}
					break;
				case 2:
					{
						//Hit Damageup
					}
					break;
				case 3:
					{
						//접근시 이동속도증가
					}
					break;
				case 4:
					{
						//범위내에 있을경우 위치표시
					}
					break;
				case 5:
					{
						//이동속도증가 한번더 
					}
					break;

			}
			Log.Instance.WriteLog("crossStack" + crossStack.ToString(), 2);
		}

		public void HitWolfPasSkill(bool flag)
		{
			if (flag)
			{
				DataManager.Instance.LocalPlayerData.roleData.InteractionSpeed += initialInteractSpeed * 0.05f;
			}
			else
			{
				DataManager.Instance.LocalPlayerData.roleData.InteractionSpeed-= initialInteractSpeed * 0.05f;
			}
		}



		/*--- Protected Methods ---*/
		protected override void PlayerInput()
		{

			if (Input.GetKeyDown(KeyCode.Mouse1))
			{
				if (!useActiveSkill)
				{ 
					photonView.RPC("DoActiveSkill", RpcTarget.AllViaServer);
				}
			}
			


				if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				DataManager.Instance.LocalPlayerData.roleData.MoveSpeed = initialSpeed * 2;
				DataManager.Instance.ShareRoleData();
			}
			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				DataManager.Instance.LocalPlayerData.roleData.MoveSpeed = initialSpeed;
				DataManager.Instance.ShareRoleData();
			}


			if (canInteract)
			{
				if (Input.GetKeyDown(KeyCode.Mouse0))
				{
					ChangeActionTo("Interact");

				}
				if (Input.GetKeyUp(KeyCode.Mouse0))
				{
					ChangeActionTo("Idle");
				}
			}
			else
			{
				if (!(CurCharacterAction is BvIdle))
				{
					ChangeActionTo("Idle");

				}
			}

		}

        protected override void SetDirection()
        {
			inputDir = BasePlayerInputManager.Instance.GetPlayerMove();
			
			camForward = camTarget.transform.forward;
			camProjToPlane = Vector3.ProjectOnPlane(camForward, Vector3.up);
			camRight = camTarget.transform.right;
			direction = (inputDir.x * camRight + inputDir.y * camProjToPlane).normalized;
		
		}
		protected override void RotateToDirection()
        {
			if (direction.sqrMagnitude > 0.01f)
			{
				
				forward = Vector3.Slerp(characterModel.transform.forward, direction,
					rotateSpeed * Time.deltaTime / Vector3.Angle(characterModel.transform.forward, direction));
				characterModel.transform.LookAt(characterModel.transform.position + forward);
			}

			
		}
        protected override void MoveCharacter()
		{
			if (controller.enabled == false)
			{
				BaseAnimator.SetFloat("Move", 0);
				Stop();
				return;
			}

			if(DataManager.Instance.PlayerDatas[playerIndex].roleData == null)
            {
				return;
            }

			controller.SimpleMove(direction * DataManager.Instance.PlayerDatas[playerIndex].roleData.MoveSpeed);

			if (direction.sqrMagnitude <= 0)
			{
				BaseAnimator.SetFloat("Move", 0);
			}
			else
			{
				BaseAnimator.SetFloat("Move", DataManager.Instance.PlayerDatas[playerIndex].roleData.MoveSpeed);
			}

		}



		[PunRPC]
		protected override void _ChangeActionTo(string ActionName)
		{
			switch (ActionName)
			{
				case "Idle":
					{
						CurCharacterAction.PushSuccessorState(idle);
					}
					break;
				case "Interact":
					{
						interaction.SetInteractObj(interactObj);
						CurCharacterAction.PushSuccessorState(interaction);
					}
					break;
				case "Down":
					{
						CurCharacterAction.PushSuccessorState(down);
					}
					break;
				case "Hit":
					{
						hit.SetPlayerIdx(playerIndex);
						CurCharacterAction.PushSuccessorState(hit);
					}
					break;
				case "Caught":
					{
						CurCharacterAction.PushSuccessorState(caught);
					}
					break;
				case "Purified":
					{
						CurCharacterAction.PushSuccessorState(purified);
					}
					break;
					
			}

			
		}



		[PunRPC]
		protected override void _AddCondition(string ConditionName)
		{
			switch (ConditionName)
			{ }
			
		}


		/*--- Private Methods ---*/


		/*--- IEumerator Methods ---*/
		
	}
}