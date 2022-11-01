using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib.Data;

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
		[SerializeField]
		protected Material GhostMaterial;
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
				//fpvCam.InitCam(camTarget);
				//tpvCam.InitCam(camTarget);
				//인형인지 퇴마사인지에 따라서 Setactive 를 해줄것.
				//fpvCam.gameObject.SetActive(false);
				//tpvCam.gameObject.SetActive(true);
				//StartCoroutine("CameraActive");
				tpvCam.gameObject.SetActive(true);
				curCam = tpvCam;
			}
			else
			{
				//fpvCam.InitCam(camTarget);
				//tpvCam.InitCam(camTarget);
				//fpvCam.gameObject.SetActive(false);
				//tpvCam.gameObject.SetActive(false);
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





		/*--- Public Methods ---*/


		public void CaughtDoll(BaseCameraController cam)
		{
			characterModel.gameObject.SetActive(false);
			ChangeCamera(cam);
			ChangeActionTo("Caught");
		}
		

		public void HitFrom()
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
			characterModel.gameObject.SetActive(true);
			puriBox.PurifyDoll(this);


			//characterObj.transform.rotation = puriBox.CharacterPos.rotation;



			BaseAnimator.Play("Fear");
			CharacterLayerChange(characterObj, 0);
			ChangeCamera(tpvCam);
			if (photonView.IsMine)
			{

				//characterObj.transform.position = puriBox.CharacterPos.position;
				float x = puriBox.CharacterPos.position.x;
				float z = puriBox.CharacterPos.position.z;
				photonView.RPC("ChangeTransform", RpcTarget.AllViaServer, x, z);
				ChangeActionTo("Purified");
			}
				characterModel.transform.rotation = puriBox.CharacterPos.rotation;
		}

		[PunRPC]
		public void ChangeTransform(float x, float z)
		{
			characterObj.transform.position = new Vector3(x, characterObj.transform.position.y, z);
		}

		public override void EscapeFrom(Transform transform, int layer)
		{
			
			this.transform.position = transform.position;
			this.transform.rotation = transform.rotation;
			characterModel.gameObject.SetActive(true);
			CharacterLayerChange(characterObj, layer);
			ChangeCamera(tpvCam);
		}
		public override void BecomeGhost()
		{
			//UI 바뀐다
			photonView.RPC("_BecomeGhost", RpcTarget.All);
			photonView.RPC("DecreaseDollCount", RpcTarget.All);
			photonView.RPC("DisappearPurificationBox",RpcTarget.All);
			ChangeActionTo("Idle");
			
		}
		
		

		[PunRPC]
		public void _BecomeGhost()
		{
			//에셋이 바뀐다
			characterModel.SetActive(false);
			GhostModel.SetActive(true);
			//Layer가 바뀐다
			CharacterLayerChange(GhostModel, 8);//8 : Ghost Layer
			GhostModel.GetComponent<Animator>().Play("GhostIdle");

			if (DataManager.Instance.LocalPlayerData.roleData.Type == RoleData.RoleType.Exorcist)
			{
				GhostModel.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = GhostMaterial;
			}


		}

		[PunRPC]
		public void DecreaseDollCount()
		{
			StageManager.Instance.DollCountDecrease();

		}
        public override bool DoResist()
        {
			if (Input.GetKeyDown(KeyCode.LeftArrow)
				|| Input.GetKeyDown(KeyCode.LeftArrow))
			{
				return true;
			}
			return false;

        }

		[PunRPC]
		public void DisappearPurificationBox()
		{
			if (interactObj is not PurificationBox)
			{
				Debug.LogError("the nearest interactObj is not Purification Box");
				return;
			}
			StageManager.Instance.Disappear(interactObj.gameObject);
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
						//interaction.SetInteractObj(interactObj);
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
				case "Escape":
					{
						CurCharacterAction.PushSuccessorState(escape);
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