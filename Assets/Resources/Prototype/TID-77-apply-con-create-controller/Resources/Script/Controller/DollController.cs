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
		public Animator Animator
		{
			get { return animator; }
		}
		public Idle GetIdle
		{
			get { return idle; }
		}
		/*--- Protected Fields ---*/
		protected PhotonTransformViewClassic photonTransformView;
		protected Behavior<BasePlayerController> CurcharacterCondition	= new Behavior<BasePlayerController>();
		protected Behavior<BasePlayerController> CurcharacterAction		= new Behavior<BasePlayerController>();

		protected Idle idle					= new Idle();
		protected Move move					= new Move();
		protected Walk walk					= new Walk();
		protected Run run					= new Run();
		protected CharacterInteraction interAction	= new CharacterInteraction();

		protected KSH_Lib.FPV_CameraController	fpvCam;
		protected TPV_CameraController			tpvCam;
		[SerializeField]
		protected Animator animator;
		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/

		public override void OnEnable()
		{
			photonTransformView = GetComponent<PhotonTransformViewClassic>();
			animator = GetComponent<Animator>();
			// �������ͽ� �޾ƿ���

			//�ִϸ����� �ޱ� -> behavior���� �ҿ���

			// ī�޶� �����ϱ�
			if (photonView.IsMine)
			{
				//�������� �𸶻������� ���� Setactive �� ���ٰ�.
				fpvCam = GameObject.Find("FPV Cam(Clone)").GetComponent<KSH_Lib.FPV_CameraController>();
				fpvCam.InitCam(camTarget);
				fpvCam.gameObject.SetActive(false);
				tpvCam = GameObject.Find("TPV Cam(Clone)").GetComponent<TPV_CameraController>();
				tpvCam.InitCam(camTarget);
				tpvCam.gameObject.SetActive(true);
			}
			// CurcharacterAction, CurcharacterCondition,  �ʱ⼳���ϱ�
			CurcharacterAction.PushSuccessorState(idle);


			//ó�� ���ð� �ֱ�( �̰� StageManger�� ����)
			base.Start();

			
		}
		protected override void Update()
		{
			//���� ���
			//camTarget.transform.Rotate(Vector3.up, 30.0f, Space.World);


			//���¿� ���� �ൿ���� -> ������Ʈ���� �߾����� ���� behavior ���� �Ұ�.

			if (photonView.IsMine)
			{
				//������ ����, �� �ൿ���� �κ�
				SetDirection();
				
				PlayerInput();

				Stop();
				
				
				var velocity = controller.velocity;
				var turnSpeed = rotateSpeed;
				photonTransformView.SetSynchronizedValues(velocity, turnSpeed);
			}

			RotateToDirection();
			MoveCharacter();

			CurcharacterAction.Update(this, ref CurcharacterAction);
			CurcharacterCondition.Update(this, ref CurcharacterCondition);
			//HP����ȭ

		}


		/*--- Public Methods ---*/

		//�ൿ�� �ѹ��� �ϳ��� ����
		public virtual void ChangeActionTo(string ActionName)
		{
			photonView.RPC("_ChangeActionTo", RpcTarget.AllViaServer, ActionName);
		}

		//���´� �ߺ����簡��
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
		public void PlayAnimation()
		{
			
		}
	
		/*--- Protected Methods ---*/
		protected void PlayerInput()
		{
			
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				ChangeActionTo("Run");
			}
			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				ChangeActionTo("Idle");
			}
			

			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				ChangeActionTo("Interact");
			}
			if(Input.GetKeyUp(KeyCode.Mouse0))
			{
				Debug.Log("To Idle");
				ChangeActionTo("Idle");
			}

		}
		protected void Stop()
		{
			if (CurcharacterAction is Move
				||CurcharacterAction is Run)
			{
				return;
			}
			direction = Vector3.zero;
		}
        protected override void SetDirection()
        {
			inputDir = BasePlayerInputManager.Instance.GetPlayerMove();
			
			camForward = camTarget.transform.forward;
			camProjToPlane = Vector3.ProjectOnPlane(camForward, Vector3.up);
			camRight = camTarget.transform.right;
			direction = (inputDir.x * camRight + inputDir.y * camProjToPlane).normalized;

			if (CurcharacterAction is Run)
			{
				return;
			}

			if (direction.sqrMagnitude > 0.01f)
			{
				if (CurcharacterAction is Move)
				{
					return;
				}
				ChangeActionTo("Move");
			}
			else
			{
				if (CurcharacterAction is Idle)
				{
					return;
				}
				ChangeActionTo("Idle");
			}
		}
		protected override void RotateToDirection()
        {
			if (direction.sqrMagnitude > 0.01f)
			{
				
				forward = Vector3.Slerp(characterModel.transform.forward, direction,
					rotateSpeed * Time.deltaTime / Vector3.Angle(characterModel.transform.forward, direction));
				characterModel.transform.LookAt(characterModel.transform.position + forward);
			}

			animator.SetFloat("Move", direction.sqrMagnitude);
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
				case "Idle":
					{
						CurcharacterAction.PushSuccessorState(idle);
					}
					break;
				case "Run":
					{
						CurcharacterAction.PushSuccessorState(run);
					}
					break;
				case "Move":
					{
						CurcharacterAction.PushSuccessorState(move);
					}
					break;
				case "Interact":
					{
						CurcharacterAction.PushSuccessorState(interAction);
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
    }
}