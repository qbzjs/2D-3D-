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
		public Behavior<BasePlayerController> CurcharacterCondition	= new Behavior<BasePlayerController>();
		public Behavior<BasePlayerController> CurcharacterAction		= new Behavior<BasePlayerController>();
		/*--- Protected Fields ---*/
		protected PhotonTransformViewClassic photonTransformView;

		protected Idle idle					= new Idle();
		protected Down down = new Down();
		protected Hit hit = new Hit();
		protected Caught caught = new Caught();
		protected CharacterInteraction interaction	= new CharacterInteraction();

		protected KSH_Lib.FPV_CameraController	fpvCam;
		protected TPV_CameraController			tpvCam;

		protected bool canInteract = false;
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
				BarUI.Instance.SliderVisible(false);
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
		public void CaughtDoll(GameObject ExorcistCamTarget)
		{
			if (photonView.IsMine)
			{
				tpvCam.InitCam(ExorcistCamTarget);
			}
			ChangeActionTo("Caught");
		}
		public void HitDamage(float Damage)
		{
			if (CurcharacterAction is not Hit)
			{ 
				ChangeActionTo("Hit");
			}
			
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

				}
				if (Input.GetKeyUp(KeyCode.Mouse0))
				{
					ChangeActionTo("Idle");
				}
			}
			else
			{
				if (!(CurcharacterAction is Idle))
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
				
				float ChargeVel = 3;//�����ӵ�
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
				case "Interact":
					{
						interaction.SetInteractObj(interactObj);
						CurcharacterAction.PushSuccessorState(interaction);
					}
					break;
				case "Down":
					{
						CurcharacterAction.PushSuccessorState(down);
					}
					break;
				case "Hit":
					{
						CurcharacterAction.PushSuccessorState(hit);
					}
					break;
				case "Caught":
					{
						CurcharacterAction.PushSuccessorState(caught);
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