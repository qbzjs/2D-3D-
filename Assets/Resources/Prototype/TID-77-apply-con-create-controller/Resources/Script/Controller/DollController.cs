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

			// 스테이터스 받아오기

			//애니매이터 받기 -> behavior에서 할예정

			// 카메라 설정하기
			if (photonView.IsMine)
			{
				fpvCam = GameObject.Find("FPV Cam(Clone)").GetComponent<FPV_CameraController>();
				tpvCam = GameObject.Find("TPV Cam(Clone)").GetComponent<TPV_CameraController>();
				tpvCam.InitCam(camTarget);
			}
			// CurcharacterAction, CurcharacterCondition,  초기설정하기



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
			}
			var velocity = controller.velocity;
			var turnSpeed = rotateSpeed;
			photonTransformView.SetSynchronizedValues(velocity, turnSpeed);

			RotateToDirection();
			MoveCharacter();
			//Debug.Log("MoveSpeed : " + moveSpeed);

			CurcharacterCondition.Update(this, ref CurcharacterCondition);
			CurcharacterAction.Update(this, ref CurcharacterAction);
			//HP동기화

        }


		/*--- Public Methods ---*/
		
		/*--- Protected Methods ---*/
		protected void PlayerInput()
		{ 
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				moveSpeed = moveSpeed*2;  //DataManager가 있다면 불러온 정보를 통해 속도조절
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