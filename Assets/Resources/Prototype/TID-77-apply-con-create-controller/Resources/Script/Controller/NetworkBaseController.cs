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
				photonView.RPC("SetPlayerIdx", RpcTarget.All, playerIndex, typeIndex);
				
				
			}
			initialSpeed = DataManager.Instance.RoleInfos[typeIndex].MoveSpeed;
			initialInteractSpeed = DataManager.Instance.RoleInfos[typeIndex].InteractionSpeed;
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
		public void SetPlayerIdx(int playerIdx, int typeIdx)
		{
			playerIndex = playerIdx;
			typeIndex = typeIdx;
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

		protected virtual IEnumerator AutoCasting()
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
		protected virtual IEnumerator AutoCastingNull()
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
		public void Interact(string castType)
		{
			StartCoroutine(castType);
		}


		public void CharacterLayerChange(GameObject Model, int layer)
		{
			Model.layer = layer;
			int count = Model.transform.childCount;
			Debug.Log("count : " + count);
			if (count != 0)
			{
				for (int i = 0; i < count; ++i)
				{
					CharacterLayerChange(Model.transform.GetChild(i).gameObject, layer);
				}
			}
			else
			{
				return;
			}
		}
		/*--- Private Methods ---*/
	}
}