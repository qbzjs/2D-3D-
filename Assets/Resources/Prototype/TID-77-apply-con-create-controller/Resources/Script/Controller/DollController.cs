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
			// �������ͽ� �޾ƿ���

			//�ִϸ����� �ޱ� -> behavior���� �ҿ���

			// ī�޶� �����ϱ�
			if (photonView.IsMine)
			{
				//�������� �𸶻������� ���� Setactive �� ���ٰ�.
				fpvCam = GameObject.Find( "FPV_Cam(Clone)" ).GetComponent<KSH_Lib.FPV_CameraController>();
				fpvCam.InitCam(camTarget);
				fpvCam.gameObject.SetActive(false);
				tpvCam = GameObject.Find( "TPV_Cam(Clone)" ).GetComponent<TPV_CameraController>();
				tpvCam.InitCam(camTarget);
				tpvCam.gameObject.SetActive(true);
			}
			// CurcharacterAction, CurcharacterCondition,  �ʱ⼳���ϱ�
			CurCharacterAction.PushSuccessorState(idle);
			//CurCharacterCondition.PushSuccessorState

			//ó�� ���ð� �ֱ�( �̰� StageManger�� ����)

			switch (typeIndex) //5~9 �ϴ� �ӽ÷� ����� ������.
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


			//���� ������ �ϳ��ۿ����� ������ �� switch���� �����ֱ⸸ �Ұ�
			//PassiveSkill.PushSuccessorState();

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
			//������ �ٲ��
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
						//���� £����
						//���� �����ð� ����� (���������̺� ������) ���ٸ� �ڷ�ƾ���� ...
					}
					break;
				case 2:
					{
						//Hit Damageup
					}
					break;
				case 3:
					{
						//���ٽ� �̵��ӵ�����
					}
					break;
				case 4:
					{
						//�������� ������� ��ġǥ��
					}
					break;
				case 5:
					{
						//�̵��ӵ����� �ѹ��� 
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