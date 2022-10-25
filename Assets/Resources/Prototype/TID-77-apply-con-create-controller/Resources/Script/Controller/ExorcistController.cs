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

		public Behavior<BasePlayerController> CurcharacterCondition = new Behavior<BasePlayerController>();
		public Behavior<BasePlayerController> CurcharacterAction = new Behavior<BasePlayerController>();


		/*--- Protected Fields ---*/
		protected PhotonTransformViewClassic photonTransformView;

		protected Idle idle = new Idle();
		protected Attack attack = new Attack();
		protected CharacterInteraction interact = new CharacterInteraction();

		protected KSH_Lib.FPV_CameraController fpvCam;
		protected TPV_CameraController tpvCam;

		protected GameObject nearestDownDoll;
		protected bool canInteract = false;
		/*--- Private Fields ---*/


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
				//인형인지 퇴마사인지에 따라서 Setactive 를 해줄것.
				fpvCam = GameObject.Find("FPV Cam(Clone)").GetComponent<KSH_Lib.FPV_CameraController>();
				fpvCam.InitCam(camTarget);

				tpvCam = GameObject.Find("TPV Cam(Clone)").GetComponent<TPV_CameraController>();
				tpvCam.InitCam(camTarget);
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
				if (CurcharacterAction is Idle)
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
				Interaction interactObj = other.GetComponent<Interaction>();
				if (!photonView.IsMine)
				{
					return;
				}

				if (BarUI.Instance.IsAutoCasting || BarUI.Instance.IsAutoCastingNull)
				{
					canInteract = false;
					BarUI.Instance.SliderVisible(true);
					BarUI.Instance.TextVisible(false);
					return;
				}
				else if (BarUI.Instance.IsCasting)
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
					if (CurcharacterAction is not Idle)
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
			if (BarUI.Instance.IsAutoCasting || BarUI.Instance.IsAutoCastingNull)
			{
				return;
			}

			if (BarUI.Instance.IsCasting)
			{
				if (Input.GetKeyUp(KeyCode.Mouse0))
				{
					BarUI.Instance.EndCasting();
				}
				return;
			}
			

			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				if (canInteract)
				{
					ChangeActionTo("Interact");
				}
				else if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
				{
					ChangeActionTo("Attack");
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
			controller.SimpleMove(direction * moveSpeed);

		}

		[PunRPC]
		protected void _ChangeActionTo(string ActionName)
		{
			switch (ActionName)
			{
				case "idle":
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
						CurcharacterAction.PushSuccessorState(interact);
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
	}
}