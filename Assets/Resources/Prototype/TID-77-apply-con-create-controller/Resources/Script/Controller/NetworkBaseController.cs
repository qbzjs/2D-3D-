using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;
using KSH_Lib.Data;
using Cinemachine;
using System;
using KSH_Lib.Object;
namespace GHJ_Lib
{
	public class NetworkBaseController : BasePlayerController, IPunObservable
	{
		public enum BehaviorType
		{
			Null,

			Idle,
			Interact,

			Attack,
			Catch,
			Imprison,

			GetHit,
			Collapse,
			BeCaught,
			BePurifying,
			Escape,
			Hide,
			BeTrapped,
			BvGhost,
			Hold,
		}

		public enum MoveType
		{
			Input,
			Stop,
			CamForward,
			StopRotation
		}

		public int TypeIndex { get; protected set; }
		public int PlayerIndex { get; protected set; }
		public bool IsMine { get { return photonView.IsMine; } }
		public BaseCameraController FPVCam { get { return fpvCam; } }
		public BaseCameraController TPVCam { get { return tpvCam; } }
		public Behavior<NetworkBaseController> CurBehavior = new Behavior<NetworkBaseController>();
		protected BvIdle idle = new BvIdle();
		protected BvInteract interact = new BvInteract();
		protected Behavior<NetworkBaseController> BvActiveSkill = new Behavior<NetworkBaseController>(); 

		protected PhotonTransformViewClassic photonTransformView;

		[field: SerializeField] public Animator BaseAnimator { get; protected set; }

		[Header( "Camera Settings" )]
		[SerializeField]
		protected KSH_Lib.FPV_CameraController fpvCam;
		[SerializeField]
		protected TPV_CameraController tpvCam;
		public BaseCameraController CurCam { get; protected set; }

		[Header("Interactor")]
		[SerializeField]
		protected Interactor interactor;
		//move 여부
		public delegate void DelPlayerInput();
		protected DelPlayerInput SetDirectionFunc;

		//skill Component
		public BaseSkill skill;


		/*--- MonoBehaviour Callbacks ---*/
		public override void OnEnable()
		{
			base.Start();
			photonTransformView = GetComponent<PhotonTransformViewClassic>();
			if ( photonTransformView == null )
			{
				Debug.LogError( "NetworkBaseController.OnEnable: Can not init photonTransformView" );
			}

			if ( photonView.IsMine )
			{
				TypeIndex = (int)DataManager.Instance.LocalPlayerData.roleData.Type;
				PlayerIndex = DataManager.Instance.PlayerIdx;
				photonView.RPC( "SetPlayerIdx", RpcTarget.All, PlayerIndex, TypeIndex );
			}
			StageManager.Instance.RegisterPlayer(gameObject);
			
		}
		protected override void Update()
		{
			if ( photonView.IsMine )
			{
				SetDirectionFunc();
				var velocity = direction * DataManager.Instance.LocalPlayerData.roleData.MoveSpeed;
				var turnSpeed = rotateSpeed;
				photonTransformView.SetSynchronizedValues( velocity, turnSpeed );

			}
			RotateToDirection();
			MoveCharacter();

			CurBehavior.Update( this, ref CurBehavior );
		}

        private void OnGUI()
        {
            if(DataManager.Instance.IsAllClientInited)
            {
				GUI.Box( new Rect( 300, PlayerIndex * 30, 200, 30 ), $"Player{PlayerIndex}: {DataManager.Instance.PlayerDatas[PlayerIndex].behaviorType}");
            }
        }

        public virtual void InitCameraSetting()
		{
			fpvCam.InitCam();
			tpvCam.InitCam();
		}
		public void ChangeCameraTo(bool isFPV)
		{
			if (isFPV)
			{
				fpvCam.gameObject.SetActive(true);
				tpvCam.gameObject.SetActive(false);
				CurCam = fpvCam;
			}
			else
			{
				tpvCam.gameObject.SetActive(true);
				fpvCam.gameObject.SetActive(false);
				CurCam = tpvCam;
			}
		}

		/*--- Public Methods ---*/
		public void ChangeMoveFunc(MoveType moveType)
		{
			if(IsMine)
			{
				switch (moveType)
				{
					case MoveType.Input:
						{
							SetDirectionFunc = SetDirection;
							CurCam.ActiveCameraControl(true);
						}
						break;
					case MoveType.Stop:
						{
							SetDirectionFunc = CannotMove;
							CurCam.ActiveCameraControl( false );
						}
						break;
					case MoveType.CamForward:
						{
							SetDirectionFunc = CamForwardMove;
							CurCam.ActiveCameraControl(true);
						}
						break;
					case MoveType.StopRotation:
						{
							SetDirectionFunc = CannotMove;
							CurCam.ActiveCameraControl(true);
							CurCam.ActiveCameraUpdate(true);
						}
						break;
				}
			}
		}

		protected virtual void CamForwardMove()
		{
			Vector3 moveDirection = camTarget.transform.forward;
			direction = moveDirection = new Vector3(moveDirection.x, 0, moveDirection.z).normalized;
		}
		protected void CannotMove()
		{
			direction = Vector3.zero;
		}

		public void ChangeCamera(BaseCameraController cam)
		{
			if (photonView.IsMine)
			{
				CurCam.gameObject.SetActive(false);
				CurCam = cam;
				cam.gameObject.SetActive(true);
			}
		}
		public virtual bool IsWatching(string tag)
		{
			if(photonView.IsMine)
            {
				RaycastHit[] hits;
				Ray ray = Camera.main.ScreenPointToRay( new Vector2( Screen.width, Screen.height ) / 2 );

				float maxDist = 10.0f;

				hits = Physics.RaycastAll( ray, maxDist );
				foreach(var hit in hits)
                {
					if(hit.collider.CompareTag(tag))
                    {
						return true;
                    }
                }
				return false;

				//if ( Physics.Raycast( ray, out hit, maxDist, LayerMask.NameToLayer("Environment"), QueryTriggerInteraction.Ignore ) )
			}
			return false;
		}

		public virtual bool IsWatching(GameObject gameObject)
		{
			if (photonView.IsMine)
			{
				RaycastHit[] hits;
				Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width, Screen.height) / 2);

				float maxDist = 10.0f;

				hits = Physics.RaycastAll(ray, maxDist);
				foreach (var hit in hits)
				{
					if (hit.collider.gameObject == gameObject)
					{
						return true;
					}
				}
				return false;

				//if ( Physics.Raycast( ray, out hit, maxDist, LayerMask.NameToLayer("Environment"), QueryTriggerInteraction.Ignore ) )
			}
			return false;
		}


		public bool IsInteractionKeyHold()
        {
            return Input.GetKey( KeyCode.G )&&(CurBehavior is not BvGetHit);
        }
		public bool IsInteractionKeyDown()
		{
			return Input.GetKeyDown( KeyCode.G );
		}

		[PunRPC]
		public void SetPlayerIdx( int playerIdx, int typeIdx )
		{
			PlayerIndex = playerIdx;
			TypeIndex = typeIdx;
			
		}

		public void ActivateCameraCollision(bool enable)
        {
			if(enable)
            {
				tpvCam.Cm3rdPersonFollow.CameraCollisionFilter.value = LayerMask.NameToLayer("Environment") | LayerMask.NameToLayer("Player");
			}
			else
            {
				tpvCam.Cm3rdPersonFollow.CameraCollisionFilter.value = 0;
			}
        }

		// Behavior Callback
		public virtual void ChangeMoveSpeed(float speedRate)
        {
			DataManager.Instance.LocalPlayerData.roleData.MoveSpeed = DataManager.Instance.RoleInfos[TypeIndex].MoveSpeed * speedRate;
			DataManager.Instance.ShareRoleData();
		}

		public virtual void DoSkill()
		{
			if (Input.GetKeyDown(KeyCode.Mouse1))
			{
				if (!skill.IsCoolTime && CurBehavior is BvIdle&& skill.CanActiveSkill())
				{
					photonView.RPC("ChangeSkillBehaviorTo_RPC", RpcTarget.AllViaServer);
				}
			}
		}

		public virtual void ChangeBvToInteract()
		{	
			ChangeBehaviorTo(BehaviorType.Interact);
		}
		public void ChangeBehaviorToAttack()
		{
			ChangeBehaviorTo(BehaviorType.Attack);
		}
		public virtual void ChangeBvToImprison() { }
		public virtual void BecomeGhost() { }
		public virtual bool DoResist() { return false; }
		public virtual bool HelpUp() { return true; }
		public virtual void EscapeFrom( Transform transform, int layer ) { }
		public virtual void ImprisonDoll() { }


		/*-- Condition --*/
		//행동은 한번에 하나씩 존재
		public virtual void ChangeBehaviorTo( BehaviorType type )
		{
			Debug.Log("Type : " + type);
			photonView.RPC( "ChangeBehaviorTo_RPC", RpcTarget.AllViaServer, type );
		}
		[PunRPC]
		protected virtual void ChangeBehaviorTo_RPC( BehaviorType type ) { }
		[PunRPC]
		public void ChangeSkillBehaviorTo_RPC()
		{
			CurBehavior.PushSuccessorState(BvActiveSkill);
		}

		public void AllocSkill(Behavior<NetworkBaseController> skillBehavior)
		{
			BvActiveSkill = skillBehavior;
		}
		/*---Skill---*/

		public void DoActionBy(Action ActionFunc)
        {
			ActionFunc();
        }
		public void DoActionBy(Action<GameObject> AcionFunc)
		{
			AcionFunc(characterModel);
		}
		public void DoActionBy(Func<GameObject,IEnumerator> AcionFunc)
		{
			StartCoroutine(AcionFunc(characterModel));
		}
		public void DoActionBy(Action<PhotonView> AcionFunc)
		{
			AcionFunc(photonView);
		}
		public void DoActionBy(Action<DollData> AcionFunc)
		{
			if (photonView.IsMine)
			{ 
				AcionFunc(DataManager.Instance.LocalPlayerData.roleData as DollData);
				DataManager.Instance.ShareRoleData();
			}
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
	}
}