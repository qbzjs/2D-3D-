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
		
		public Idle GetIdle
		{
			get { return idle; }
		}
		/*--- Protected Fields ---*/
		protected PhotonTransformViewClassic photonTransformView;
		protected Behavior<BasePlayerController> CurcharacterCondition	= new Behavior<BasePlayerController>();
		protected Behavior<BasePlayerController> CurcharacterAction		= new Behavior<BasePlayerController>();

		protected Idle idle					= new Idle();

		protected CharacterInteraction interaction	= new CharacterInteraction();

		protected KSH_Lib.FPV_CameraController	fpvCam;
		protected TPV_CameraController			tpvCam;

		protected bool canInteract = false;
		protected streamVector3 sVector3;
		/*--- Private Fields ---*/
		Interaction interactObj;

		/*--- MonoBehaviour Callbacks ---*/

		public override void OnEnable()
		{
			photonTransformView = GetComponent<PhotonTransformViewClassic>();
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
				if (CurcharacterAction is Idle)
				{
					SetDirection();
				}
				else
				{
					Stop();
				}
				
				PlayerInput();

				//Stop();
				var velocity = direction*moveSpeed;
				var turnSpeed = rotateSpeed;
				photonTransformView.SetSynchronizedValues(velocity, turnSpeed);
				
			}
			RotateToDirection();
			MoveCharacter();


			CurcharacterAction.Update(this, ref CurcharacterAction);
			CurcharacterCondition.Update(this, ref CurcharacterCondition);
			//HP����ȭ

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
					BarUI.Instance.SliderVisible(false);
				}

				if (Vector3.Dot(forward, Vector3.ProjectOnPlane((other.transform.position - this.transform.position), Vector3.up)) < 90&&
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
			}
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
				sVector3.x = direction.x;
				sVector3.y = direction.y;
				sVector3.z = direction.z;
				stream.SendNext(sVector3.x);
				stream.SendNext(sVector3.y);
				stream.SendNext(sVector3.z);

				stream.SendNext(this.transform.position.x);
				stream.SendNext(this.transform.position.y);
				stream.SendNext(this.transform.position.z);

			}
			if (stream.IsReading)
			{
				this.sVector3.x = (float)stream.ReceiveNext();
				this.sVector3.y = (float)stream.ReceiveNext();
				this.sVector3.z = (float)stream.ReceiveNext();
				this.direction = new Vector3(sVector3.x, sVector3.y, sVector3.z);

				this.transform.position = new Vector3((float)stream.ReceiveNext(), (float)stream.ReceiveNext(), (float)stream.ReceiveNext());
			}
		}

		/*--- Protected Methods ---*/
		protected void PlayerInput()
		{

			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				moveSpeed = 12.0f;
			}
			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				moveSpeed = 6.0f;
			}


			if (canInteract)
			{
				if (Input.GetKeyDown(KeyCode.Mouse0))
				{
					ChangeActionTo("Interact");
					BarUI.Instance.BeginCasting();
				}
				if (Input.GetKeyUp(KeyCode.Mouse0))
				{
					ChangeActionTo("Idle");
					BarUI.Instance.EndCasitng();
				}
			}
			else
			{
				if (!(CurcharacterAction is Idle))
				{
					ChangeActionTo("Idle");
					BarUI.Instance.EndCasitng();
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
			controller.SimpleMove(direction * moveSpeed);

			if (direction.sqrMagnitude <= 0)
			{
				BaseAnimator.SetFloat("Move", 0);
			}
			else
			{
				BaseAnimator.SetFloat("Move", moveSpeed);
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
				case "Interact":
					{
						CurcharacterAction.PushSuccessorState(interaction);
						interaction.SetInteractObj(interactObj);
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

		/*
		[PunRPC]
		void sendNext(������,�ε���)
		{
			����Ʈ(�ε���).���� = ������
		}
		*/
        /*--- Private Methods ---*/
    }
}