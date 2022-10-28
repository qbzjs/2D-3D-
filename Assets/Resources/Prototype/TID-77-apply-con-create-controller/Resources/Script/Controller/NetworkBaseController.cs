using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;

namespace GHJ_Lib
{
	public class NetworkBaseController: BasePlayerController, IPunObservable
	{
		/*--- Public Fields ---*/
		public int TypeIndex
		{
			get { return typeIndex; }
		}
		public int PlayerIndex
		{
			get { return playerIndex; }
		}
		public Behavior<BasePlayerController> CurCharacterCondition = new Behavior<BasePlayerController>();
		public Behavior<BasePlayerController> CurCharacterAction = new Behavior<BasePlayerController>();
		/*--- Protected Fields ---*/

		/*--photon--*/
		protected PhotonTransformViewClassic photonTransformView;

		/*--Cam--*/
		protected KSH_Lib.FPV_CameraController fpvCam;
		protected TPV_CameraController tpvCam;

		/*--initailData--*/
		protected int typeIndex;
		protected int playerIndex;
		protected float initialSpeed;
		protected float initialInteractSpeed;

		/*--interact--*/
		protected bool canInteract = false;
		protected Interaction interactObj;

		/*--Skill--*/
		protected bool useActiveSkill = false;
		/*--- Private Fields ---*/


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
				initialSpeed = DataManager.Instance.RoleInfos[typeIndex].MoveSpeed;
				initialInteractSpeed = DataManager.Instance.RoleInfos[typeIndex].InteractionSpeed;
				photonView.RPC("SetPlayerIdx", RpcTarget.All, playerIndex, typeIndex, initialSpeed);
			}
			base.Start();
		}
		protected override void Update()
		{
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
				var velocity = direction * DataManager.Instance.LocalPlayerData.roleData.MoveSpeed;
				var turnSpeed = rotateSpeed;
				photonTransformView.SetSynchronizedValues(velocity, turnSpeed);

			}
			RotateToDirection();
			MoveCharacter();


			CurCharacterAction.Update(this, ref CurCharacterAction);
			CurCharacterCondition.Update(this, ref CurCharacterCondition);
		}

		

		private void OnGUI()
		{
			GUI.Box(new Rect(200, 0, 150, 30), $"direction: {direction}");
			GUI.Box(new Rect(200, 30, 150, 30), $"camTagetRot: {camTarget.transform.rotation}");
			GUI.Box(new Rect(200, 60, 150, 30), $": {transform.rotation}");
		}


		/*--- Public Methods ---*/

		/*--Action--*/
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

		/*--initial--*/
		[PunRPC]
		public virtual void SetPlayerIdx(int playerIdx, int typeIdx, float initialSpeed, int initialInteractSpeed)
		{
			playerIndex = playerIdx;
			typeIndex = typeIdx;
			this.initialSpeed = initialSpeed;
			this.initialInteractSpeed = initialInteractSpeed;
		}

		/*---Skill---*/
		[PunRPC]
		public virtual void DoActiveSkill()
		{
			StartCoroutine("ActiveSkillBox");
		}

		protected virtual IEnumerator ActiveSkillBox()
		{

			yield return new WaitForSeconds(0.2f);
	
		}

		/*--IpunObserve--*/
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
		protected virtual void PlayerInput()
		{

			

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
		protected virtual void _ChangeActionTo(string ActionName)
		{
			switch (ActionName)
			{

			}
		}

		[PunRPC]
		protected virtual void _AddCondition(string ConditionName)
		{
			switch (ConditionName)
			{ }

		}
		/*--- Private Methods ---*/
	}
}