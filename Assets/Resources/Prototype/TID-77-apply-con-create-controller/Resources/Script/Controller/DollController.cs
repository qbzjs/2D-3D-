using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;

namespace GHJ_Lib
{
	public class DollController: BasePlayerController
	{

		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/
		protected PhotonTransformViewClassic photonTransformView;
		protected Behavior<BasePlayerController> CurcharacterCondition = new Behavior<BasePlayerController>();
        protected Behavior<BasePlayerController> CurcharacterAction = new Behavior<BasePlayerController>();
		protected FPV_CameraController fpvCam;
		protected TPV_CameraController tpvCam;
        /*--- Private Fields ---*/

        /*--- MonoBehaviour Callbacks ---*/
        protected override void Start()
        {
            
        }
        public override void OnEnable()
        {
			photonTransformView = GetComponent<PhotonTransformViewClassic>();

			// �������ͽ� �޾ƿ���

			//�ִϸ����� �ޱ� -> behavior���� �ҿ���

			// ī�޶� �����ϱ�
			if (photonView.IsMine)
			{
				fpvCam = GameObject.Find("FPV Cam(Clone)").GetComponent<FPV_CameraController>();
				tpvCam = GameObject.Find("TPV Cam(Clone)").GetComponent<TPV_CameraController>();
				tpvCam.InitCam(camTarget);
			}
			// CurcharacterAction, CurcharacterCondition,  �ʱ⼳���ϱ�



			//ó�� ���ð� �ֱ�( �̰� StageManger�� ����)
			base.Start();
        }
        protected override void Update()
        {
			
			//���¿� ���� �ൿ���� -> ������Ʈ���� �߾����� ���� behavior ���� �Ұ�.

			if (photonView.IsMine)
			{
				//������ ����, �� �ൿ���� �κ�
				PlayerInput();
				SetDirection();
			}
			var velocity = controller.velocity;
			var turnSpeed = rotateSpeed;
			photonTransformView.SetSynchronizedValues(velocity, turnSpeed);

			RotateToDirection();
			MoveCharacter();
			//Debug.Log("MoveSpeed : " + moveSpeed);

			CurcharacterCondition.Update(this, ref CurcharacterCondition);
			CurcharacterAction.Update(this, ref CurcharacterAction);
			//HP����ȭ

        }


		/*--- Public Methods ---*/
		
		/*--- Protected Methods ---*/
		protected void PlayerInput()
		{ 
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				moveSpeed = moveSpeed*2;  //DataManager�� �ִٸ� �ҷ��� ������ ���� �ӵ�����
			}
			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				moveSpeed = moveSpeed / 2;
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


		/*--- Private Methods ---*/
	}
}