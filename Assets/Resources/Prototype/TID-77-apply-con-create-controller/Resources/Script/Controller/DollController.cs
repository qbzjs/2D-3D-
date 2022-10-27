using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;

namespace GHJ_Lib
{
	public class DollController : BasePlayerController,IPunObservable
	{

		/*--- Public Fields ---*/
		
		public BvIdle GetIdle
		{
			get { return idle; }
		}
		public int TypeIndex
		{
			get { return typeIndex; }
		}
		public int PlayerIndex
		{
			get { return playerIndex; }
		}
		public Behavior<BasePlayerController> CurCharacterCondition	= new Behavior<BasePlayerController>();
		public Behavior<BasePlayerController> CurCharacterAction		= new Behavior<BasePlayerController>();
		public Behavior<BasePlayerController> ActiveSkill = new Behavior<BasePlayerController>();
		public Behavior<BasePlayerController> PassiveSkill = new Behavior<BasePlayerController>();

		[SerializeField]
		protected GameObject GhostModel;
		/*--- Protected Fields ---*/


		protected PhotonTransformViewClassic photonTransformView;

		protected BvIdle idle					= new BvIdle();
		protected BvDown down = new BvDown();
		protected BvHit hit = new BvHit();
		protected BvCaught caught = new BvCaught();
		protected BvCharacterInteraction interaction	= new BvCharacterInteraction();
		protected BvPurified purified = new BvPurified();
		protected BvEscape escape = new BvEscape();



		protected KSH_Lib.FPV_CameraController	fpvCam;
		protected TPV_CameraController			tpvCam;

		protected bool canInteract = false;
		protected int typeIndex;
		protected int playerIndex;
		protected float initialSpeed;
		/*--- Private Fields ---*/
		Interaction interactObj;
		[PunRPC]
		public void SetPlayerIdx(int playerIdx)
		{
			playerIndex = playerIdx;
			
		}

		[PunRPC]
		public void SetTypeIdx( int typeIdx)
		{
			
			typeIndex = typeIdx;
		}

		/*--- MonoBehaviour Callbacks ---*/
		public override void OnEnable()
		{
			photonTransformView = GetComponent<PhotonTransformViewClassic>();
			//GhostModel.SetActive(false);
			// 스테이터스 받아오기

			//애니매이터 받기 -> behavior에서 할예정

			// 카메라 설정하기
			if (photonView.IsMine)
			{
				typeIndex = (int)DataManager.Instance.LocalPlayerData.roleData.TypeOrder;
				playerIndex = DataManager.Instance.PlayerIdx;
				photonView.RPC("SetPlayerIdx", RpcTarget.All, playerIndex);
				photonView.RPC("SetTypeIdx", RpcTarget.All, typeIndex);
				initialSpeed = DataManager.Instance.RoleInfos[typeIndex].MoveSpeed;



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
			base.Start();

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
			//ActiveSkill.PushSuccessorState();
			//PassiveSkill.PushSuccessorState();
				
		}
		protected override void Update()
		{
			//게임 대기
			//camTarget.transform.Rotate(Vector3.up, 30.0f, Space.World);


			//상태에 따른 행동조건 -> 업데이트에서 했었으나 이젠 behavior 에서 할것.

			if (photonView.IsMine)
			{
				//움직임 관련, 및 행동제한 부분
				if (CurCharacterAction is BvIdle)
				{
					SetDirection();
				}
				else
				{
					Stop();
				}
				
				PlayerInput();

				//Stop();
				var velocity = direction*DataManager.Instance.LocalPlayerData.roleData.MoveSpeed;
				var turnSpeed = rotateSpeed;
				photonTransformView.SetSynchronizedValues(velocity, turnSpeed);
				
			}
			RotateToDirection();
			MoveCharacter();


			CurCharacterAction.Update(this, ref CurCharacterAction);
			CurCharacterCondition.Update(this, ref CurCharacterCondition);
			//HP동기화

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

		//행동은 한번에 하나씩 존재
		public virtual void ChangeActionTo(string ActionName)
		{
			photonView.RPC("_ChangeActionTo", RpcTarget.AllViaServer, ActionName);
		}

		//상태는 중복존재가능
		public virtual void AddCondition(string ConditionName)
		{
			photonView.RPC("_AddCondition", RpcTarget.AllViaServer, ConditionName);
		}

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

	

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{

			if (stream.IsWriting)
			{

				stream.SendNext(direction.x);
				stream.SendNext(direction.y);
				stream.SendNext(direction.z);

				stream.SendNext(this.transform.position.x);
				stream.SendNext(this.transform.position.y);
				stream.SendNext(this.transform.position.z);

			}
			if (stream.IsReading)
			{
				this.direction.x = (float)stream.ReceiveNext();
				this.direction.y = (float)stream.ReceiveNext();
				this.direction.z = (float)stream.ReceiveNext();

				this.transform.position = new Vector3((float)stream.ReceiveNext(), (float)stream.ReceiveNext(), (float)stream.ReceiveNext());
			}
		}

		/*--- Protected Methods ---*/
		protected void PlayerInput()
		{

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
		protected void Stop()
		{
			direction = Vector3.zero;
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

		protected override IEnumerator AutoCasting()
		{
			if (IsAutoCasting)
			{
				yield break;
			}
			IsAutoCasting = true;
			BarUI.Instance.SetTarget(interactObj);
			while (true)
			{
				
				float ChargeVel = 3;//차지속도
				interactObj.AddGauge(ChargeVel * Time.deltaTime);
				yield return new WaitForEndOfFrame();
				if (interactObj.GetGaugeRate >= 1.0f)
				{
					IsAutoCasting = false;
					break;
				}
			}
		}
		protected override IEnumerator AutoCastingNull()
		{
			if (IsAutoCasting)
			{
				yield break;
			}
			IsAutoCasting = true;
			BarUI.Instance.SetTarget(null);
			while (true)
			{
				float ChargeVel = 3;
				BarUI.Instance.UpdateValue(ChargeVel * Time.deltaTime);
				yield return new WaitForEndOfFrame();
				if (BarUI.Instance.GetValue >= 1.0f)
				{
					IsAutoCasting = false;
					break;
				}
			}
		}

		[PunRPC]
		protected void _ChangeActionTo(string ActionName)
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

		/*
		[PunRPC]
		protected void _ChangeActionTo(Behavior<BasePlayerController> Action)
		{
			CurcharacterAction.PushSuccessorState(Action);
		}
		*/

		[PunRPC]
		protected void _AddCondition(string ConditionName)
		{
			switch (ConditionName)
			{ }
			
		}


		/*--- Private Methods ---*/


		/*--- IEumerator Methods ---*/
		
	}
}