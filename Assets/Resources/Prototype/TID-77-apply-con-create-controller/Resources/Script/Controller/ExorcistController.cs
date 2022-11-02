using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;
namespace GHJ_Lib
{
	public class ExorcistController: NetworkBaseController, IPunObservable
	{
		[field: SerializeField] public ParticleSystem Aura { get; protected set; }
		[SerializeField] private GameObject[] CatchObj;
		[SerializeField] public PickUpBox PickUpBox { get; protected set; }
		[SerializeField] public AttackBox AttackBox { get; protected set; }

		protected GameObject caughtDoll;


		// Behaviors
		protected BvIdle idle = new BvIdle();
		protected BvAttack attack = new BvAttack();
		protected BvInteract interact = new BvInteract();
		protected BvCatch catchDoll = new BvCatch();
		protected BvImprison imprison = new BvImprison();

		// Skills
		protected BvBishopActSkill actSkill = new BvBishopActSkill();


		/*--- MonoBehaviour Callbacks ---*/
		public override void OnEnable()
		{
			base.OnEnable();

			if (photonView.IsMine)
			{
				fpvCam.gameObject.SetActive(true);
				curCam = fpvCam;
			}
			CurBehavior.PushSuccessorState(idle);
		}


		// Behavior Callbacks
		public override void ImprisonDoll()
		{
			DollController doll = caughtDoll.GetComponent<DollController>();
			CatchObj[doll.TypeIndex - 5].gameObject.SetActive( false );
			doll.ChangeBvToBePurifying((interactObj as PurificationBox));
		}

		// Behavior Conditions
		public override void ChangeBvToImprison()
		{
			ChangeBehaviorTo(BehaviorType.Interact);
		}
		public virtual void ChangeBvToCatch()
		{
			ChangeBehaviorTo(BehaviorType.Catch);
		}


		/*--- Protected Methods ---*/
        protected override void RotateToDirection()
		{
			if (direction.sqrMagnitude > 0.01f)
			{
				BaseAnimator.SetFloat("MoveSpeed", direction.magnitude);
			}
			else
			{
				BaseAnimator.SetFloat("MoveSpeed", 0);
			}
			if (photonView.IsMine)
			{ 
				characterModel.transform.rotation =  Quaternion.Euler(0.0f, camTarget.transform.rotation.eulerAngles.y,0.0f);
			}
		}
		protected override void MoveCharacter()
		{
			if (controller.enabled == false)
			{
				return;
			}
			if(DataManager.Instance.PlayerDatas[0].roleData != null)
			{
				controller.SimpleMove(direction * DataManager.Instance.PlayerDatas[0].roleData.MoveSpeed);
			}
		}


		[PunRPC]
		protected override void ChangeBehaviorTo_RPC(BehaviorType behaviorType)
		{
			switch ( behaviorType )
			{
				case BehaviorType.Idle:
					{
						CurBehavior.PushSuccessorState(idle);
					}
					break;
				case BehaviorType.Attack:
					{
						CurBehavior.PushSuccessorState(attack);
					}
					break;
				case BehaviorType.Interact:
					{
						if (CurBehavior is BvIdle)
						{
							CurBehavior.PushSuccessorState(interact);
						}
						else if (CurBehavior is BvCatch)
						{
							CurBehavior.PushSuccessorState(imprison);
						}
					}
					break; 
				case BehaviorType.Catch:
					{
						caughtDoll = PickUpBox.FindNearestCollapsedDoll();
						CurBehavior.PushSuccessorState(catchDoll);
					}
					break;
			}
		}


		/*--- AnimationCallbacks Methods ---*/
		public void PickUp()
		{
			DollController doll = caughtDoll.GetComponent<DollController>();
			CatchObj[doll.TypeIndex-5].gameObject.SetActive(true);
			StageManager.CharacterLayerChange(caughtDoll, 8);
			doll.ChangeBvToBeCaught(tpvCam);
		}


        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
			if (stream.IsWriting)
			{
				stream.SendNext(direction.x);
				stream.SendNext(direction.y);
				stream.SendNext(direction.z);

				stream.SendNext(characterModel.transform.rotation.eulerAngles.x);
				stream.SendNext(characterModel.transform.rotation.eulerAngles.y);
				stream.SendNext(characterModel.transform.rotation.eulerAngles.z);

				stream.SendNext(this.transform.position.x);
				stream.SendNext(this.transform.position.y);
				stream.SendNext(this.transform.position.z);
			}
			if (stream.IsReading)
			{
				direction.x = (float)stream.ReceiveNext();
				direction.y = (float)stream.ReceiveNext();
				direction.z = (float)stream.ReceiveNext();

				float x	= (float)stream.ReceiveNext();
				float y = (float)stream.ReceiveNext();
				float z = (float)stream.ReceiveNext();

				characterModel.transform.rotation = Quaternion.Euler(new Vector3(x,y,z));

				x = (float)stream.ReceiveNext();
				y = (float)stream.ReceiveNext();
				z = (float)stream.ReceiveNext();
				this.transform.position = new Vector3(x, y, z);
			}
		}


		/*----Use ESC Menu---*/
		public override void ExitGame()
		{
			photonView.RPC("ExitAll", RpcTarget.All);
		}

		[PunRPC]
		public void ExitAll()
		{
			StageManager.Instance.EndGame();
		}
    }
}