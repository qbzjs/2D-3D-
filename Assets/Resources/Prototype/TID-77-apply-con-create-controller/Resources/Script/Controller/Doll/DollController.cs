using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib.Data;
using MSLIMA.Serializer;
using KSH_Lib.Object;
using LSH_Lib;
namespace GHJ_Lib
{
	public class DollController : NetworkBaseController, IPunObservable
	{
		/*--- Public Fields ---*/
		public int CrossStack { get { return crossStack; } }
		protected int crossStack = 0;
		public bool IsCrowDebuff { get; set; } = false;
		public float CrowGauge { get; set; } = 0.0f;

		public GameObject trapInteractor;

		/*--- Protected Fields ---*/
		protected BvCollapse down = new BvCollapse();
		protected BvGetHit hit = new BvGetHit();
		protected BvBeCaught caught = new BvBeCaught();
		protected BvBePurifying purified = new BvBePurifying();
		protected BvEscape escape = new BvEscape();
		protected BvHide bvHide = new BvHide();
		protected BvbeTrapped bvbeTrapped = new BvbeTrapped();
		protected BvGhost bvGhost = new BvGhost();

		//[SerializeField] protected SkinnedMeshRenderer skinnedMeshRenderer;
		//[SerializeField] protected Material ghostMaterial;
		[SerializeField] protected GameObject ghostPrefab;
		[SerializeField] protected Animator ghostAnimator;
		[field: SerializeField] public TrailRenderer runTrail { get; protected set; }


		public GameObject BloodDecal;
		public GameObject BloodSpawner;
		public ParticleSystem HealEffect;
		public DollData GetDollData { get { return DataManager.Instance.PlayerDatas[PlayerIndex].roleData as DollData; } }
	


		/*--- MonoBehaviour Callbacks ---*/
		public override void OnEnable()
		{
			base.OnEnable();
			HealEffect.Stop();
			runTrail.emitting = false;		
			//CurBehavior.PushSuccessorState(idle);
		}

        protected override void Update()
        {
            base.Update();

			//For Debug Ghost
			if(Input.GetKeyDown(KeyCode.Alpha0))
            {
				if(IsMine)
                {
                    BecomeGhost();
                }
			}
        }

        public override void InitCameraSetting()
		{
			if (photonView.IsMine)
			{
				base.InitCameraSetting();
				tpvCam.gameObject.SetActive(true);
				fpvCam.gameObject.SetActive(false);
				CurCam = tpvCam;
			}
		}

		/*--- Public Methods ---*/

		public void ChangeBvToBeCaught(BaseCameraController cam)
		{
			characterModel.gameObject.SetActive(false);
			ChangeCamera(cam);
			ChangeBehaviorTo(BehaviorType.BeCaught);
		}
		public void ChangeBvToGetHit()
		{
			if (CurBehavior is not BvGetHit)
			{
				if (photonView.IsMine)
				{
					ChangeBehaviorTo(BehaviorType.GetHit);
				}
			}
		}


		public void ChangeBvToBePurifying(KSH_Lib.Object.PurificationBox puriBox)
		{
			characterModel.gameObject.SetActive(true);
			HealEffect.Stop();// 임시방편
			puriBox.SetDoll(this);

			if (photonView.IsMine)
			{
				byte[] bytes = new byte[0];
				Serializer.Serialize(puriBox.CharacterPos.position, ref bytes);
				photonView.RPC("ChangeTransform", RpcTarget.AllViaServer, bytes);
				ChangeBehaviorTo(BehaviorType.BePurifying);
				//StartCoroutine( ChangeDevilHPByDeltaTime( puriBox.Damage, () => (CurBehavior is BvEscape) ) );
			}
			characterModel.transform.rotation = puriBox.CharacterPos.rotation;

			ChangeCamera(tpvCam);
			StageManager.CharacterLayerChange(characterObj, LayerMask.NameToLayer("Player"));
		}
		[PunRPC]
		public void ChangeTransform(byte[] data)
		{
			int offset = 0;
			characterObj.transform.position = Serializer.DeserializeVector3(data, ref offset);
		}


		IEnumerator ChangeDevilHPByDeltaTime(float damage, System.Func<bool> EndCond)
		{
			while (true)
			{
				if (GetDollData.DevilHP <= 0.0f || EndCond())
				{
					break;
				}
				ChangeDevilHP(-damage * Time.deltaTime);
				yield return null;
			}
		}


		public override void EscapeFrom(Transform transform, int layer)
		{
			this.transform.position = transform.position;
			this.transform.rotation = transform.rotation;
			characterModel.gameObject.SetActive(true);
			StageManager.CharacterLayerChange(characterObj, layer);
			ChangeCamera(tpvCam);
		}
		public override void BecomeGhost()
		{
			//UI 바뀐다
			StageManager.Instance.DecereseDollCount();
			photonView.RPC("_BecomeGhost", RpcTarget.All);
			ChangeBehaviorTo(BehaviorType.Idle);
		}

		public virtual IEnumerator Hide()
		{
			Transform modelTrans = characterModel.transform;
			BaseAnimator.SetBool("IsHide", true);
			float rotZ = modelTrans.localRotation.eulerAngles.z;
			float posY = modelTrans.localScale.x;
			while (true)
			{
				rotZ += 90.0f * Time.deltaTime;

				if (rotZ >= 90.0f)
				{
					rotZ = 90.0f;
				}
				modelTrans.localRotation = Quaternion.Euler(modelTrans.localRotation.eulerAngles.x, modelTrans.localRotation.eulerAngles.y, rotZ);
				modelTrans.localPosition = new Vector3(modelTrans.localPosition.x, posY / 2, modelTrans.localPosition.z);
				//modelTrans.position = new Vector3(
				//	(modelTrans.position.x - Mathf.Cos(modelTrans.rotation.z) + Mathf.Cos(PosZ)) * modelTrans.localScale.x / 2,
				//	(modelTrans.position.y - Mathf.Sin(modelTrans.rotation.z) + Mathf.Sin(PosZ)) * modelTrans.localScale.y / 2,
				//	modelTrans.position.z);
				yield return new WaitForEndOfFrame();
				if (rotZ.Equals(90.0f))
				{
					break;
				}
			}
		}

		public virtual IEnumerator UnHide()
		{
			yield return GameManager.Instance.WaitZeroPointFiveS;
		}


		public void ChangeDevilHP(float delta)
		{
			if (IsMine)
			{
				GetDollData.DevilHP += delta;
				DataManager.Instance.ShareRoleData();

				if (GetDollData.DevilHP <= 0.0f)
				{
					BaseAnimator.Play("Idle_A");
					BecomeGhost();
					CurBehavior.PushSuccessorState(new BvIdle());
				}
			}
		}

		[PunRPC]
		public void _BecomeGhost()
		{
			if (IsMine)
			{
				//skinnedMeshRenderer.material = ghostMaterial;
				characterModel.SetActive( false );
				characterModel = ghostPrefab;
				characterModel.SetActive( true );

				BaseAnimator = ghostAnimator;

				interactor.enabled = false;
			}
			else
			{
				interactor.gameObject.SetActive(false);
				characterObj.SetActive(false);
			}

			StageManager.CharacterLayerChange(characterObj, LayerMask.NameToLayer(GameManager.GhostLayer));//8 : Ghost Layer
			ChangeBehaviorTo(BehaviorType.BvGhost);
		}

		public override bool DoResist()
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow)
				|| Input.GetKeyDown(KeyCode.LeftArrow))
			{
				return true;
			}
			return false;
		}

		public void BeHealed_RPC(bool isHeal)
		{
			photonView.RPC("BeHealed", RpcTarget.AllViaServer, isHeal);
		}
		[PunRPC]
		public void BeHealed(bool isHeal)
		{
			if (isHeal)
			{
				HealEffect.Play();
				ChangeMoveFunc(MoveType.StopRotation);
			}
			else
			{
				HealEffect.Stop();
				ChangeMoveFunc(MoveType.Input);
			}
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
						//흔적 짙어짐
						//흔적 유지시간 길어짐 (데이터테이블에 있을지) 없다면 코루틴으로 ...
					}
					break;
				case 2:
					{
						//Hit Damageup
					}
					break;
				case 3:
					{
						//접근시 이동속도증가
					}
					break;
				case 4:
					{
						//범위내에 있을경우 위치표시
					}
					break;
				case 5:
					{
						//이동속도증가 한번더 
					}
					break;
			}

			if (photonView.IsMine)
			{ 
				Log.Instance.WriteLog("crossStack" + crossStack.ToString(),0);
			}
		}

		public void HitWolfPasSkill(bool flag)
		{
			if (photonView.IsMine)
			{ 
				if (flag)
				{
					DataManager.Instance.LocalPlayerData.roleData.InteractionSpeed += DataManager.Instance.RoleInfos[TypeIndex].InteractionSpeed * 0.05f;
				}
				else
				{
					DataManager.Instance.LocalPlayerData.roleData.InteractionSpeed-= DataManager.Instance.RoleInfos[TypeIndex].InteractionSpeed * 0.05f;
				}
				DataManager.Instance.ShareRoleData();
			}
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

				float x = (float)stream.ReceiveNext();
				float y = (float)stream.ReceiveNext();
				float z = (float)stream.ReceiveNext();

				characterModel.transform.rotation = Quaternion.Euler(new Vector3(x, y, z));

				x = (float)stream.ReceiveNext();
				y = (float)stream.ReceiveNext();
				z = (float)stream.ReceiveNext();
				this.transform.position = new Vector3(x, y, z);
			}
		}

		/*--- Protected Methods ---*/
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
            if ( controller.enabled == false )
            {
                BaseAnimator.SetFloat( "Move", 0 );
                CannotMove();
                return;
            }

            if ( DataManager.Instance.PlayerDatas[PlayerIndex].roleData == null )
            {
                return;
            }

            controller.SimpleMove( direction * DataManager.Instance.PlayerDatas[PlayerIndex].roleData.MoveSpeed );
			AudioManager.instance.Play("DollWalk", AudioManager.PlayTarget.Doll);
            if ( direction.sqrMagnitude <= 0 )
            {
                BaseAnimator.SetFloat( "Move", 0 );
            }
            else
            {
                BaseAnimator.SetFloat( "Move", DataManager.Instance.PlayerDatas[PlayerIndex].roleData.MoveSpeed );
				
            }
        }


        [PunRPC]
		protected override void ChangeBehaviorTo_RPC( BehaviorType type )
		{
			switch ( type )
			{
				case BehaviorType.Idle:
				{
					CurBehavior.PushSuccessorState( idle );
				}
				break;
				case BehaviorType.Interact:
				{
					CurBehavior.PushSuccessorState(interact);
				}
				break;
				case BehaviorType.Collapse:
				{
					CurBehavior.PushSuccessorState( down );
				}
				break;
				case BehaviorType.GetHit:
				{
					CurBehavior.PushSuccessorState( hit );
				}
				break;
				case BehaviorType.BeCaught:
				{
					CurBehavior.PushSuccessorState( caught );
				}
				break;
				case BehaviorType.BePurifying:
				{
					CurBehavior.PushSuccessorState( purified );
				}
				break;
				case BehaviorType.Escape:
				{
					CurBehavior.PushSuccessorState( escape );
				}
				break;
				case BehaviorType.Hide:
				{
					CurBehavior.PushSuccessorState(bvHide);
				}
				break;
				case BehaviorType.BeTrapped:
				{
					CurBehavior.PushSuccessorState(bvbeTrapped);
				}
				break;
				case BehaviorType.BvGhost:
				{
					CurBehavior.PushSuccessorState(bvGhost);
				}
				break;
			}
		}

		//effect
		public void ShowHitEffect()
		{
			EffectManager.Instance.ShowDecal(BloodSpawner, BloodDecal);
		}
	}
}