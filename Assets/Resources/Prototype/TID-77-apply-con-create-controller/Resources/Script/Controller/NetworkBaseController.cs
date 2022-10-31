using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;
using KSH_Lib.Data;
using Cinemachine;

namespace GHJ_Lib
{
	public class NetworkBaseController : BasePlayerController, IPunObservable
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
		public Behavior<NetworkBaseController> CurCharacterCondition = new Behavior<NetworkBaseController>();
		public Behavior<NetworkBaseController> CurCharacterAction = new Behavior<NetworkBaseController>();
		/*--- Protected Fields ---*/

		/*--photon--*/
		protected PhotonTransformViewClassic photonTransformView;

		/*--Cam--*/


		[SerializeField]
		protected KSH_Lib.FPV_CameraController fpvCam;
		[SerializeField]
		protected TPV_CameraController tpvCam;

		protected BaseCameraController curCam;
		


		/*--initailData--*/
		protected int typeIndex;
		protected int playerIndex;
		protected float initialSpeed;
		protected float initialInteractSpeed;

		/*--interact--*/
		public bool CanInteract
		{
            get{ return canInteract; }
		}
		protected bool canInteract = false;
		protected InteractionObj interactObj;
		protected float maxViewAngle = 0.5f;

		/*---CastingType---*/
		public bool IsCasting = false;
		public bool IsAutoCasting = false;
		public InteractionObj.CastingType castingType = InteractionObj.CastingType.NotCasting;

		/*--Skill--*/
		protected bool useActiveSkill = false;


		public delegate void DelPlayerInput();
		protected DelPlayerInput moveInput;

		protected BarUI_Controller barUI;
        /*--- Private Fields ---*/


        /*--- MonoBehaviour Callbacks ---*/
        public override void OnEnable()
		{
			

			barUI = StageManager.Instance.BarUI;
			Debug.Log("BarUI : " + barUI.name);
			if (barUI == null)
			{
				Debug.LogError("BarUI StageManager.Instance.BarUI : Can not instance ui");
			}
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
			SetMoveInput(true);

		}
		protected override void Update()
		{
			if (photonView.IsMine)
			{
				//움직임 관련, 및 행동제한 부분
				moveInput();
				//PlayerInput();

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

		protected virtual void OnTriggerStay(Collider other)
		{

			
			if (other.CompareTag("interactObj"))
			{
				interactObj = other.GetComponent<InteractionObj>();


				if (photonView.IsMine)
				{
					Vector3 objVector = (other.transform.position - this.transform.position);
					float viewAngle = Vector3.Dot(forward, objVector);
					if (viewAngle > maxViewAngle
						|| viewAngle < 0)
					{
						barUI.SliderVisible(false);
						barUI.TextVisible(false);
						return;
					}

					castingType = interactObj.GetCastingType(this);


					switch (castingType)
					{
						case InteractionObj.CastingType.SharedAutoCasting:
							{
								if (IsAutoCasting)
								{
									barUI.SliderVisible(true);
									barUI.TextVisible(false);
									canInteract = false;
								}
								else
								{
									barUI.SliderVisible(false);
									barUI.TextVisible(true);
									canInteract = true;
								}
							}
							break;
						case InteractionObj.CastingType.LocalAutoCasting:
							{
								if (IsAutoCasting)
								{
									barUI.SliderVisible(true);
									barUI.TextVisible(false);
									canInteract = false;
								}
								else
								{
									barUI.SliderVisible(false);
									barUI.TextVisible(true);
									canInteract = true;
								}
							}
							break;
						case InteractionObj.CastingType.ManualCasting:
							{
								if (IsCasting)
								{
									barUI.SliderVisible(true);
									barUI.TextVisible(false);
								}
								else
								{
									barUI.SliderVisible(false);
									barUI.TextVisible(true);
								}
								canInteract = true;

							}
							break;
						case InteractionObj.CastingType.NotCasting:
							{
								canInteract = false;
								barUI.SliderVisible(false);
								barUI.TextVisible(false);
							}
							break;
					}
				}

			}
			
		}

		protected virtual void OnTriggerExit(Collider other)
		{
			if (!photonView.IsMine)
			{
				return;
			}

			if (other.CompareTag("interactObj"))
			{
				castingType = InteractionObj.CastingType.NotCasting;
				canInteract = false;
				barUI.TextVisible(false);
				barUI.SliderVisible(false);
			}
		}


		/*--- Public Methods ---*/
		/*--Input Controll--*/

		public void SetMoveInput(bool flag)
		{
			if (flag)
			{
				moveInput = SetDirection;
			}
			else
			{
				moveInput = Stop;
			}
		}

		/*--Do--*/

		public virtual void DoHit()
		{
			(DataManager.Instance.LocalPlayerData.roleData as DollData).DollHP -= (DataManager.Instance.PlayerDatas[0].roleData as ExorcistData).AttackPower;
			DataManager.Instance.ShareRoleData();
		}

		public virtual void DoSprint()
		{
			
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				DataManager.Instance.LocalPlayerData.roleData.MoveSpeed = initialSpeed * 2;
				DataManager.Instance.ShareRoleData();
			}
			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				DataManager.Instance.LocalPlayerData.roleData.MoveSpeed = initialSpeed;
				DataManager.Instance.ShareRoleData();
			}

			
		}

		public virtual void DoSkill()
		{
		
			if (Input.GetKeyDown(KeyCode.Mouse1))
			{
				if (!useActiveSkill)
				{
					photonView.RPC("DoActiveSkill", RpcTarget.AllViaServer);
				}
			}
			
		}

		public virtual void DoInteract()
		{	
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				ChangeActionTo("Interact");

			}

		}

		public void DoAttack()
		{

			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
				{
					ChangeActionTo("Attack");
					return;
				}

			}
			
		}

		public virtual void DoImprison()
		{
			
		}





        public virtual void BecomeGhost()
		{
			
		}

		public virtual bool DoResist()
		{
			return false;
		}

		public virtual bool HelpUp()
		{
			return true;
		}


		public virtual void EscapeFrom(Transform transform, int layer)
		{
			
		}

		public virtual void Imprison()
		{
			
		}

		public virtual void InteractBy( InteractionObj.CastingType type )
		{
			StartCoroutine( _InteractBy( type ) );
		}
		protected virtual IEnumerator _InteractBy( InteractionObj.CastingType type )
		{
			switch ( type )
			{
				case InteractionObj.CastingType.ManualCasting:
				{
					yield return Cast();
				}
				break;
				case InteractionObj.CastingType.SharedAutoCasting:
				{
					yield return SharedAutoCasting();
				}
				break;
				case InteractionObj.CastingType.LocalAutoCasting:
				{
					yield return LocalAutoCasting();
				}
				break;
				case InteractionObj.CastingType.NotCasting:
				{
					Debug.LogError( "Wrong interact" );
				}
				break;
			}
		}

		public virtual IEnumerator Cast()
		{
			if (IsCasting)
			{
				yield break;
			}
			IsCasting = true;
			barUI.SetTarget(interactObj);
			while (true)
			{
				float ChargeVel = 3.0f;//차지속도
				interactObj.AddGauge(ChargeVel * Time.deltaTime);
				Debug.Log(interactObj.name);
				Debug.Log(interactObj.GetGaugeRate);
				yield return new WaitForEndOfFrame();
				if (!Input.GetKey(KeyCode.Mouse0))
				{
					IsCasting = false;
					break;
				}
			}
		}

		protected virtual IEnumerator SharedAutoCasting()
		{
			if (IsAutoCasting)
			{
				yield break;
			}
			IsAutoCasting = true;
			barUI.SetTarget(interactObj);
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
		protected virtual IEnumerator LocalAutoCasting()
		{
			if (IsAutoCasting)
			{
				yield break;
			}
			IsAutoCasting = true;
			barUI.SetTarget(null);
			while (true)
			{
				float ChargeVel = 3;
				barUI.UpdateValue(ChargeVel * Time.deltaTime);
				yield return new WaitForEndOfFrame();
				if (BarUI_Controller.Instance.GetValue >= 1.0f)
				{
					IsAutoCasting = false;
					break;
				}
			}
		}

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
		public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
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
		


		public void CharacterLayerChange(GameObject Model, int layer)
		{
			Model.layer = layer;
			int count = Model.transform.childCount;
			//Debug.Log("count : " + count);
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