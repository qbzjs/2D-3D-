using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;
namespace GHJ_Lib
{
	public class ExorcistController: BasePlayerController ,IPunObservable
	{
		/*--- Public Fields ---*/
	
		public ParticleSystem Ayra;
		public GameObject[] CatchObj;

		public PickUpBox pickUpBox;
		public AttackBox attackBox;

		public Behavior<BasePlayerController> CurcharacterCondition = new Behavior<BasePlayerController>();
		public Behavior<BasePlayerController> CurcharacterAction = new Behavior<BasePlayerController>();

		/*--- Protected Fields ---*/
		protected PhotonTransformViewClassic photonTransformView;

		protected BvIdle idle = new BvIdle();
		protected BvAttack attack = new BvAttack();
		protected BvCharacterInteraction interact = new BvCharacterInteraction();
		protected BvCatch catchDoll = new BvCatch();
		protected BvImprison imprison = new BvImprison();

		protected KSH_Lib.FPV_CameraController fpvCam;
		protected TPV_CameraController tpvCam;

		protected GameObject caughtDoll;
		protected bool canInteract = false;
		protected int typeIndex;
		protected float initialSpeed;

		/*--- Private Fields ---*/
		Interaction interactObj;

		/*--- MonoBehaviour Callbacks ---*/
		public override void OnEnable()
		{
			photonTransformView = GetComponent<PhotonTransformViewClassic>();
			animator = GetComponent<Animator>();
			// 스테이터스 받아오기

			//애니매이터 받기 -> behavior에서 할예정

			// 카메라 설정하기
			if (photonView.IsMine)
			{
				typeIndex = (int)DataManager.Instance.LocalPlayerData.roleData.TypeOrder;
				Debug.Log($"TypeIndex: {typeIndex}");
				initialSpeed = DataManager.Instance.RoleInfos[typeIndex].MoveSpeed;
				//인형인지 퇴마사인지에 따라서 Setactive 를 해줄것.
				fpvCam = GameObject.Find( "FPV_Cam(Clone)" ).GetComponent<KSH_Lib.FPV_CameraController>();
				if(fpvCam == null)
                {
					Debug.LogError( "ExorcistController.OnEnable: Can not find FPVCamController" );
					return;
                }

				fpvCam.InitCam(camTarget);
				fpvCam.gameObject.SetActive(true);

				tpvCam = GameObject.Find( "TPV_Cam(Clone)" ).GetComponent<TPV_CameraController>();
				if ( tpvCam == null )
				{
					Debug.LogError( "ExorcistController.OnEnable: Can not find TPVCamController" );
					return;
				}
				tpvCam.InitCam(camTarget);
				tpvCam.gameObject.SetActive(false);
			}
			// CurcharacterAction, CurcharacterCondition,  초기설정하기
			CurcharacterAction.PushSuccessorState(idle);


			//처음 대기시간 주기( 이건 StageManger가 할일)
			base.Start();
		}
		protected override void Update()
		{
			//상태에 따른 행동조건 -> 업데이트에서 했었으나 이젠 behavior 에서 할것.

			if (photonView.IsMine)
			{
				//움직임 관련, 및 행동제한 부분
				PlayerInput();
				if (CurcharacterAction is BvIdle)
				{
					SetDirection();
				}
				else
				{
					Stop();
				}
				var velocity = controller.velocity;
				var turnSpeed = rotateSpeed;
				photonTransformView.SetSynchronizedValues(velocity, turnSpeed);
			}

			RotateToDirection();
			MoveCharacter();
			

			CurcharacterCondition.Update(this, ref CurcharacterCondition);
			CurcharacterAction.Update(this, ref CurcharacterAction);
			//HP동기화
		}

		private void OnTriggerStay(Collider other)
		{

			if (other.CompareTag("interactObj"))
			{
				interactObj = other.GetComponent<Interaction>();
				if (!photonView.IsMine)
				{
					return;
				}
				if (IsAutoCasting)
				{
					canInteract = false;
					BarUI.Instance.SliderVisible(true);
					BarUI.Instance.TextVisible(false);
				}
				else if (IsCasting)
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
					if (CurcharacterAction is BvCharacterInteraction)
					{
						ChangeActionTo("Idle");
					}
					BarUI.Instance.SliderVisible(false);
				}

				if (Vector3.Dot(forward, Vector3.ProjectOnPlane((other.transform.position - this.transform.position), Vector3.up)) < 90 &&
					interactObj.CanActiveToExorcist)
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
		protected void Stop()
		{
			direction = Vector3.zero;
		}
		protected void PlayerInput()
		{

			if (pickUpBox.CanPickUp()&&(CurcharacterAction is not BvCatch))
			{
				if (Input.GetKeyDown(KeyCode.Mouse0))
				{
					ChangeActionTo("Catch");
					return;
				}
			}

			if (canInteract)
			{
				if (Input.GetKeyDown(KeyCode.Mouse0))
				{

					ChangeActionTo("Interact");
					return;
				}
				if (Input.GetKeyUp(KeyCode.Mouse0))
				{

					ChangeActionTo("Idle");
					return;
				}
			}
			else
			{ 
				if (Input.GetKeyDown(KeyCode.Mouse0))
				{
					if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
					{
						ChangeActionTo("Attack");
						return;
					}
				
				}
			}

		}
		

		protected override void RotateToDirection()
		{
			if (direction.sqrMagnitude > 0.01f)
			{
				animator.SetFloat("MoveSpeed", direction.magnitude);
				forward = Vector3.Slerp(characterModel.transform.forward, direction,
					rotateSpeed * Time.deltaTime / Vector3.Angle(characterModel.transform.forward, direction));
				characterModel.transform.LookAt(characterModel.transform.position + forward);
			}
			else
			{
				animator.SetFloat("MoveSpeed", 0);
			}
		}
		protected override void MoveCharacter()
		{
			if (controller.enabled == false)
			{
				return;
			}

			if(DataManager.Instance.PlayerDatas[0].roleData != null)
			{
				controller.SimpleMove(direction * DataManager.Instance.PlayerDatas[0].roleData.MoveSpeed);
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
						CurcharacterAction.PushSuccessorState(idle);
					}
					break;
				case "Attack":
					{
						CurcharacterAction.PushSuccessorState(attack);
					}
					break;
				case "Interact":
					{
						if (CurcharacterAction is BvIdle)
						{
							interact.SetInteractObj(interactObj);
							CurcharacterAction.PushSuccessorState(interact);
						}
						if (CurcharacterAction is BvCatch)
						{
							imprison.SetCaughtDoll(caughtDoll);
							imprison.SetInteractObj(interactObj);
							CurcharacterAction.PushSuccessorState(imprison);
						}
					}
					break; 
				case "Catch":
					{
						caughtDoll = pickUpBox.FindNearestFallDownDoll();
						CurcharacterAction.PushSuccessorState(catchDoll);
					}
					break;
			}


		}

		[PunRPC]
		protected void _AddCondition(string ConditionName)
		{
			switch (ConditionName)
			{ }

		}


		/*--- Private Methods ---*/














		/*--- AnimationCallbacks Methods ---*/
		public void ImprisonDoll(GameObject camTarget)
        {
			DollController doll = caughtDoll.GetComponent<DollController>();

			CatchObj[doll.TypeIndex-5].gameObject.SetActive(false);
			doll.ChangeCamera(camTarget);

		}
		public void PickUp()
		{
			DollController doll = caughtDoll.GetComponent<DollController>();
			CharacterLayerChange(caughtDoll, 8);
			CatchObj[doll.TypeIndex-5].gameObject.SetActive(true);
			doll.CaughtDoll(camTarget);

		}
	}
}