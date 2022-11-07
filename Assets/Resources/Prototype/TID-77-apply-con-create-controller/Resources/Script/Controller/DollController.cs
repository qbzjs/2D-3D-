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
		public int CrossStack { get { return crossStack; } }
		[SerializeField]
		protected GameObject GhostModel;
		[SerializeField]
		protected Material GhostMaterial;


		/*--- Protected Fields ---*/
		protected BvCollapse down = new BvCollapse();
		protected BvGetHit hit = new BvGetHit();
		protected BvBeCaught caught = new BvBeCaught();
		protected BvBePurifying purified = new BvBePurifying();
		protected BvEscape escape = new BvEscape();

		protected int crossStack = 0;

		/*--- MonoBehaviour Callbacks ---*/
		public override void OnEnable()
		{
			base.OnEnable();
			if (photonView.IsMine)
			{
				tpvCam.gameObject.SetActive(true);
				curCam = tpvCam;
			}
			CurBehavior.PushSuccessorState(idle);
		}


		/*--- Public Methods ---*/
		public void ChangeBvToBeCaught(BaseCameraController cam)
		{
			characterModel.gameObject.SetActive(false);
			ChangeCamera(cam);
			ChangeBehaviorTo(BehaviorType.BeCaught);
		}
		public void ChangeBvToGetHit()
		{
			if (CurBehavior is not BvGetHit)
			{
				if (photonView.IsMine)
				{
					ChangeBehaviorTo(BehaviorType.GetHit);
				}
			}
		}
		public void ChangeBvToBePurifying(KSH_Lib.Object.PurificationBox puriBox)
		{
			characterModel.gameObject.SetActive(true);
			puriBox.SetDoll(this);

			if ( photonView.IsMine )
			{
				float x = puriBox.CharacterPos.position.x;
				float z = puriBox.CharacterPos.position.z;
				photonView.RPC( "ChangeTransform", RpcTarget.AllViaServer, x, z );
				ChangeBehaviorTo( BehaviorType.BePurifying );
			}
			characterModel.transform.rotation = puriBox.CharacterPos.rotation;

			StageManager.CharacterLayerChange(characterObj, LayerMask.NameToLayer( "Player" ) );
			ChangeCamera(tpvCam);
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
			StageManager.CharacterLayerChange(characterObj, layer);
			ChangeCamera(tpvCam);
		}
		public override void BecomeGhost()
		{
			//UI 바뀐다
			photonView.RPC("_BecomeGhost", RpcTarget.All);
			photonView.RPC("DecreaseDollCount", RpcTarget.All);
			photonView.RPC("DisappearPurificationBox",RpcTarget.All);
			ChangeBehaviorTo(BehaviorType.Idle);
		}
		
		

		[PunRPC]
		public void _BecomeGhost()
		{
			//에셋이 바뀐다
			characterModel.SetActive(false);
			GhostModel.SetActive(true);
			//Layer가 바뀐다
			StageManager.CharacterLayerChange(GhostModel, 8);//8 : Ghost Layer
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
			//if (interactObj is not PurificationBox)
			//{
			//	Debug.LogError("the nearest interactObj is not Purification Box");
			//	return;
			//}
			//StageManager.Instance.Disappear(interactObj.gameObject);
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
			if (photonView.IsMine)
			{ 
				if (flag)
				{
					DataManager.Instance.LocalPlayerData.roleData.InteractionSpeed += DataManager.Instance.RoleInfos[TypeIndex].InteractionSpeed * 0.05f;
				}
				else
				{
					DataManager.Instance.LocalPlayerData.roleData.InteractionSpeed-= DataManager.Instance.RoleInfos[TypeIndex].InteractionSpeed * 0.05f;
				}
				DataManager.Instance.ShareRoleData();
			}
		}



		/*--- Protected Methods ---*/
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
				CannotMove();
				return;
			}

			if(DataManager.Instance.PlayerDatas[PlayerIndex].roleData == null)
            {
				return;
            }

			controller.SimpleMove(direction * DataManager.Instance.PlayerDatas[PlayerIndex].roleData.MoveSpeed);

			if (direction.sqrMagnitude <= 0)
			{
				BaseAnimator.SetFloat("Move", 0);
			}
			else
			{
				BaseAnimator.SetFloat("Move", DataManager.Instance.PlayerDatas[PlayerIndex].roleData.MoveSpeed);
			}
		}


		[PunRPC]
		protected override void ChangeBehaviorTo_RPC( BehaviorType type )
		{
			switch ( type )
			{
				case BehaviorType.Idle:
				{
					CurBehavior.PushSuccessorState( idle );
				}
				break;
				case BehaviorType.Interact:
				{
					CurBehavior.PushSuccessorState(interact);
				}
				break;
				case BehaviorType.Collapse:
				{
					CurBehavior.PushSuccessorState( down );
				}
				break;
				case BehaviorType.GetHit:
				{
					CurBehavior.PushSuccessorState( hit );
				}
				break;
				case BehaviorType.BeCaught:
				{
					CurBehavior.PushSuccessorState( caught );
				}
				break;
				case BehaviorType.BePurifying:
				{
					CurBehavior.PushSuccessorState( purified );
				}
				break;
				case BehaviorType.Escape:
				{
					CurBehavior.PushSuccessorState( escape );
				}
				break;
			}
		}
	}
}