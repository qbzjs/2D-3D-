using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;
namespace GHJ_Lib
{
	public class ExorcistController: BasePlayerController
	{
		/*--- Public Fields ---*/
		public Animator Animator
		{
			get { return animator; }
		}

		public ParticleSystem Ayra;
		public GameObject[] CatchObj;

		/*--- Protected Fields ---*/
		protected PhotonTransformViewClassic photonTransformView;
		protected Behavior<BasePlayerController> CurcharacterCondition = new Behavior<BasePlayerController>();
		protected Behavior<BasePlayerController> CurcharacterAction = new Behavior<BasePlayerController>();

		protected Idle idle = new Idle();
		protected Walk walk = new Walk();
		protected Attack attack = new Attack();


		protected KSH_Lib.FPV_CameraController fpvCam;
		protected TPV_CameraController tpvCam;
		[SerializeField]
		protected Animator animator;

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
				SetDirection();
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
			}
			if (stream.IsReading)
			{
				this.direction.x = (float)stream.ReceiveNext();
				this.direction.y = (float)stream.ReceiveNext();
				this.direction.z = (float)stream.ReceiveNext();
			}
		}
		/*--- Protected Methods ---*/
		protected void PlayerInput()
		{

			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
				{
					ChangeActionTo("Attack");
				}
				
			}
			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				
			}

		}
		protected override void RotateToDirection()
		{
			if (direction.sqrMagnitude > 0.01f)
			{
				animator.SetFloat("Move", direction.sqrMagnitude);
				forward = Vector3.Slerp(characterModel.transform.forward, direction,
					rotateSpeed * Time.deltaTime / Vector3.Angle(characterModel.transform.forward, direction));
				characterModel.transform.LookAt(characterModel.transform.position + forward);
			}
			else
			{
				animator.SetFloat("Move", 0);
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