using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;
using KSH_Lib.Data;
using Cinemachine;
using System;

namespace GHJ_Lib
{
	public class NetworkBaseController : BasePlayerController, IPunObservable
	{
		public enum BehaviorType
		{
			Idle,
			Interact,

			Attack,
			Catch,
			Imprison,

			GetHit,
			Collapse,
			BeCaught,
			BePurifying,
			Escape
		}


		public int TypeIndex { get; protected set; }
		public int PlayerIndex { get; protected set; }

		public Behavior<NetworkBaseController> CurBehavior = new Behavior<NetworkBaseController>();
		public Behavior<NetworkBaseController> ActiveSkill = new Behavior<NetworkBaseController>();
		protected PhotonTransformViewClassic photonTransformView;

		[field: SerializeField] public Animator BaseAnimator { get; protected set; }

		[Header( "Camera Settings" )]
		[SerializeField]
		protected KSH_Lib.FPV_CameraController fpvCam;
		[SerializeField]
		protected TPV_CameraController tpvCam;
		protected BaseCameraController curCam;


		public delegate void DelPlayerInput();
		protected DelPlayerInput SetDirectionFunc;

		protected RoleData initData = new RoleData();


		/*---Skill Component---*/
		public EffectArea actSkillArea;
		protected bool useActiveSkill = false;



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
				TypeIndex = (int)DataManager.Instance.LocalPlayerData.roleData.TypeOrder;
				PlayerIndex = DataManager.Instance.PlayerIdx;
				photonView.RPC( "SetPlayerIdx", RpcTarget.All, PlayerIndex, TypeIndex );
			}

			initData = DataManager.Instance.RoleInfos[TypeIndex];



			ChangeMoveFunc( true );
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

			// Behavior
			CurBehavior.Update( this, ref CurBehavior );
		}


		/*--- Public Methods ---*/
		public void ChangeMoveFunc(bool canMove)
		{
			if (canMove)
			{
				SetDirectionFunc = SetDirection;
			}
			else
			{
				SetDirectionFunc = CannotMove;
			}
		}
		protected void CannotMove()
		{
			direction = Vector3.zero;
		}
		public void ChangeCamera(BaseCameraController cam)
		{
			if (photonView.IsMine)
			{
				curCam.gameObject.SetActive(false);
				curCam = cam;
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

		[PunRPC]
		public void SetPlayerIdx( int playerIdx, int typeIdx )
		{
			PlayerIndex = playerIdx;
			TypeIndex = typeIdx;
		}


		// Behavior Callback
		public virtual void ChangeMoveSpeed(float speedRate)
        {
			DataManager.Instance.LocalPlayerData.roleData.MoveSpeed = initData.MoveSpeed * speedRate;
			DataManager.Instance.ShareRoleData();
		}

		public virtual void DoSkill()
		{
			if (Input.GetKeyDown(KeyCode.Mouse1))
			{
				if (!useActiveSkill&&CurBehavior is BvIdle)
				{
					photonView.RPC("DoActiveSkill", RpcTarget.AllViaServer);
				}
			}
		}
		public virtual void ChangeBvToInteract()
		{	
			ChangeBehaviorTo(BehaviorType.Interact);
		}
		public void ChangeBehaviorToAttack()
		{
			if (!BaseAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
			{
				ChangeBehaviorTo(BehaviorType.Attack);
				return;
			}
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
			photonView.RPC( "ChangeBehaviorTo_RPC", RpcTarget.AllViaServer, type );
		}
		[PunRPC]
		protected virtual void ChangeBehaviorTo_RPC( BehaviorType type ) { }


		// Exit
		public virtual void ExitGame()
		{
			photonView.RPC( "_ExitGame", RpcTarget.All );
		}
		[PunRPC]
		public void _ExitGame()
		{
			StageManager.Instance.DoExit( this );
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
		public void DoActionBy(Action<DollData> AcionFunc)
		{
			if (photonView.IsMine)
			{ 
				AcionFunc(DataManager.Instance.LocalPlayerData.roleData as DollData);
				DataManager.Instance.ShareRoleData();
			}
		}

		[PunRPC]
		public virtual void DoActiveSkill()
		{
			StartCoroutine("ExcuteActiveSkil");
		}
		protected virtual IEnumerator ExcuteActiveSkil()
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
	}
}