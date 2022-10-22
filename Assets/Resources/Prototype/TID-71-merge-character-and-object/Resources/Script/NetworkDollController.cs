using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;
using Cinemachine;
using LSH_Lib;

namespace GHJ_Lib
{
	public class NetworkDollController : TestPlayerController,IPunObservable
	{
		/*--- Public Fields ---*/
		public DollAnimationController dollAnimationController;
		public DollStatus dollStatus;
		public CinemachineVirtualCamera cinemachineVirtual;
		public Network_TPV_CameraController network_TPV_CameraController;
		public Behavior<NetworkDollController> CurBehavior { get { return curBehavior; } }

		public GameObject CamTarget;
		public GameObject CharacterModel;

		public Transform escapeTransform;
		/*--- Protected Fields ---*/
		protected Behavior<NetworkDollController> curBehavior = new Behavior<NetworkDollController>();
		protected streamVector3 sVector3;
		protected BvNormal bvNormal = new BvNormal();
		protected BvSlow bvSlow = new BvSlow();
		protected BvBlind bvBlind = new BvBlind();
		protected BvExpose bvExpose = new BvExpose();
		protected BvGhost bvGhost = new BvGhost();
		protected BvFall bvFall = new BvFall();
		protected BvGrabbed bvGrabbed = new BvGrabbed();
		protected BvImprison bvImplement = new BvImprison();
		[SerializeField]
		protected SkinnedMeshRenderer skinnedMeshRenderer;
		protected Material originMaterial;
		protected bool isCanMove = true;
		protected PhotonTransformViewClassic photonTransformView;
		/*--- Private Fields ---*/

		PurificationBox purificatinBox;
		//---interaction Fields---//
		public float CastingVelocity
		{
			get { return castingVelocity; }
		}
		protected float castingVelocity = 2.0f;

		protected bool canInteract = false;
		protected bool isInteract = false;
		//

		/*---MonoBehaviorCallbacks---*/
		public override void OnEnable()
		{
			photonTransformView = GetComponent<PhotonTransformViewClassic>();
			//dollStatus = GetComponent<DollStatus>();
			if (dollStatus == null)
			{
				Debug.LogError("Missing DollStatus");
			}
			if (dollAnimationController == null)
			{
				Debug.LogError("MIssing DollAnimationController");
			}
			//dollAnimationController.SetStatus(dollStatus);
			GameObject virtualCamera = GameObject.Find("VirtualPlayerCamera");
			if (virtualCamera == null)
			{
				Debug.LogError("MIssing virtualCamera");
			}
			cinemachineVirtual = virtualCamera.gameObject.GetComponent<CinemachineVirtualCamera>();

			network_TPV_CameraController = virtualCamera.GetComponent<Network_TPV_CameraController>();

			moveSpeed = dollStatus.MoveSpeed; //최종 스피드는 이동속도*상태*디버프 


			curBehavior.PushSuccessorState(bvNormal);
			base.Start();
		}

		protected override void Update()
		{
			

			if (curBehavior is BvGrabbed)
			{
				if (Input.GetKeyDown(KeyCode.K))
				{
					photonView.RPC("EscapeGrab", RpcTarget.All);
					escapeTransform = cinemachineVirtual.Follow.transform;
				}
			}

			if (photonView.IsMine)
			{
				InputCheatKey();
				if (isCanMove)
				{ 
				PlayerInput();
				SetDirection();
				var velocity = controller.velocity;
				var turnSpeed = rotateSpeed;
				photonTransformView.SetSynchronizedValues(velocity, turnSpeed);
				}
			}
			RotateToDirection();
			MoveCharacter();
			//Debug.Log("MoveSpeed : " + moveSpeed);

			curBehavior.Update(this, ref curBehavior);
			dollAnimationController.UpdateHP_Rate();
		}

		// >> interaction 
		private void OnTriggerStay(Collider other)
		{
			if (!photonView.IsMine)
			{
				return;
			}

			if (other.CompareTag("interactObj"))
			{
				if (SceneManager.Instance.IsCoroutine)
				{
					return;
				}

				LSH_Lib.interaction obj = other.GetComponent<LSH_Lib.interaction>();
				if (Vector3.Dot(Vector3.ProjectOnPlane(obj.transform.position - this.transform.position, Vector3.up), this.transform.forward) < 90.0f
					&& obj.CanActiveToDoll)
				{
					canInteract = true;
					SceneManager.Instance.EnableInteractionText();
					SceneManager.Instance.DisableCastingBar();
				}
				else
				{
					canInteract = false;
					SceneManager.Instance.DisableInteractionText();
					SceneManager.Instance.DisableCastingBar();
					return;
				}

				if (isInteract)
				{
					SceneManager.Instance.DisableInteractionText();
					SceneManager.Instance.EnableCastingBar(other.gameObject);
					obj.Interact(gameObject.tag, this);
				}
				else
				{
					SceneManager.Instance.EnableInteractionText();
					SceneManager.Instance.DisableCastingBar();
				}
			}

			if (curBehavior is BvGhost)
			{
				return;
			}

			if (dollStatus.DollHealthPoint <= 0)
			{
				if (!(curBehavior is BvFall))
				{
					curBehavior.PushSuccessorState(bvFall);
				}
		
			}

			if (dollStatus.DevilHealthPoint <= 0)
			{
				curBehavior = bvGhost;
			}
		}

        private void OnTriggerExit(Collider other)
        {
			if (!photonView.IsMine)
			{
				return;
			}

			if (other.CompareTag("interactObj"))
			{
				canInteract = false;
				SceneManager.Instance.DisableInteractionText();
				SceneManager.Instance.DisableCastingBar();
			}
		}
        // <<


        /*--- Public Methods ---*/
        public DollStatus GetStatus()
		{
			if (dollStatus == null)
			{
				return null;
			}
			else
			{
				return dollStatus;
			}
			
		}

		public void Hit(float Power)
		{
			//curBehavior.PushSuccessorState(bvSlow);

			if (curBehavior is BvFall)
			{
				return;
			}

			dollAnimationController.PlayHitAnimation();
			dollStatus.HitDollHP(Power);

			if (dollStatus.DollHealthPoint <= 0)
			{
				curBehavior.PushSuccessorState(bvFall);
			}

			if (dollStatus.DevilHealthPoint <= 0)
			{
				curBehavior = bvGhost;
			}
		}

		public void ExposedByExorcist()
		{
			if (curBehavior is BvNormal)
			{
				curBehavior.PushSuccessorState(bvExpose);
				dollAnimationController.PlayHitAnimation();
			}
		}
		public void StartCoroutineSlow()
		{
			StartCoroutine("Slow", 5);
		}

		public void StartCoroutineExpose()
		{
			if (GameManager.Instance.Data.Role == DEM.RoleType.Exorcist)
			{
				StartCoroutine("Expose", 40);
			}
		}

		public void BecomeGhost()
		{
			if (GameManager.Instance.Data.Role == DEM.RoleType.Doll)
			{
				skinnedMeshRenderer.material = Resources.Load<Material>("Materials/Ghost");
			}
			else
			{
				skinnedMeshRenderer.material = Resources.Load<Material>("Materials/Invisible");
			}

			CharacterLayerChange(this.gameObject, 8);
		}

		public void FallDown()
		{
			isCanMove = false;
			dollAnimationController.PlayFallDownAnimation();

		}

		public void Grabbed(GameObject exorcistCamTarget)
		{
			
			Debug.Log("Grabbed");
			//photonView.ObservedComponents.Remove(photonTransform);
			//controller.enabled = false;

			if (photonView.IsMine)
			{ 
				cinemachineVirtual.Follow = exorcistCamTarget.transform;
				network_TPV_CameraController.SetCamTarget(exorcistCamTarget);
			}
			CharacterModel.SetActive(false);
			CharacterLayerChange(this.gameObject, 8);
			curBehavior.PushSuccessorState(bvGrabbed);
		}

		public void Imprison()
		{
			Debug.Log("The doll imprisons");

			float Power = 5.0f;

			dollStatus.HitDevilHP(Power);
			Debug.Log("DevilHP: "+dollStatus.DevilHealthPoint); 
			if (dollStatus.DevilHealthPoint<=0.0f)
			{
				Debug.Log("becomeGhost");
				curBehavior = bvGhost;
			}

			if (curBehavior is BvGhost)
			{
				escapeTransform = cinemachineVirtual.Follow.transform;
				purificatinBox.DieToGhost();
				return;
			}
			curBehavior.PushSuccessorState(bvImplement);
		}

		public void Imprison(GameObject purificatinBox)
		{
			this.purificatinBox = purificatinBox.GetComponent<PurificationBox>();
			Transform camTarget = purificatinBox.transform.GetChild(2);
			if (photonView.IsMine)
			{
				cinemachineVirtual.Follow = camTarget;
				network_TPV_CameraController.SetCamTarget(camTarget.gameObject);
			}

			Imprison();
		}




		public void BecomeIdle()
		{
			dollAnimationController.CancelAnimation();
		}

		public void Released(Transform transform)
		{
			//isCanMove = true;
			//dollAnimationController.CancelAnimation();
			//curBehavior.PushSuccessorState(bvReleased);
			EscapeGrab(transform);
		}

		[PunRPC]
		public void SetPosition(Vector3 position)
		{
			this.gameObject.transform.position = position;
		}
		[PunRPC]
		public void EscapeGrab()
		{
			if (escapeTransform)
			{
				EscapeGrab(escapeTransform);
			}

		}

		public void GhostEscape()
		{
			photonView.RPC("EscapeGrab", RpcTarget.All);
		}



		/*---IPunObserve---*/
		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{

			if (stream.IsWriting)
			{
				sVector3.x = direction.x;
				sVector3.y = direction.y;
				sVector3.z = direction.z;
				stream.SendNext(sVector3.x);
				stream.SendNext(sVector3.y);
				stream.SendNext(sVector3.z);
			}
			if (stream.IsReading)
			{
				this.sVector3.x = (float)stream.ReceiveNext();
				this.sVector3.y = (float)stream.ReceiveNext();
				this.sVector3.z = (float)stream.ReceiveNext();
				this.direction = new Vector3(sVector3.x, sVector3.y, sVector3.z);
			}

		}


		/*--- Protected Methods ---*/

		protected override void PlayerInput()
		{
			if (Input.GetKeyDown(KeyCode.G))
			{
				//interact button in 
				dollAnimationController.PlayInteractAnimation();
				isInteract = true;// << interaction
				return;
			}
			if (Input.GetKeyUp(KeyCode.G))
			{
				//interact button out
				dollAnimationController.CancelAnimation();
				isInteract = false; // << interaction
				return;
			}


			horizontal = Input.GetAxis("Horizontal");
			vertical = Input.GetAxis("Vertical");

			if (vertical != 0 || horizontal != 0)
			{
				dollAnimationController.IsMove = true;
			}
			else
			{
				dollAnimationController.IsMove = false;
			}



			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				dollAnimationController.IsRoll = true;
				moveSpeed = dollStatus.MoveSpeed * 3;
			}
			if (Input.GetKeyUp(KeyCode.LeftShift))
			{
				dollAnimationController.IsRoll = false;
				moveSpeed = dollStatus.MoveSpeed;
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

		
		protected void EscapeGrab(Transform transform)
		{
			isCanMove = true;
			if (photonView.IsMine)
			{
				cinemachineVirtual.Follow = CamTarget.transform;
				network_TPV_CameraController.SetCamTarget(CamTarget);
				CamTarget.transform.rotation = Quaternion.identity;
			}
			this.transform.position = transform.position;
			this.transform.rotation = transform.rotation;
			CharacterModel.SetActive(true);

			dollStatus.HitDollHP(-dollStatus.MaxDollHitPoint / 2);
			BecomeIdle();

			if (curBehavior is BvGhost)
			{
				return;
			}
 
			CharacterLayerChange(this.gameObject, 7);
			

		}

		/*--- Private Methods ---*/

		/*------*/
		IEnumerator Slow(float slowTime)
		{
			moveSpeed *= 0.8f;
			yield return new WaitForSeconds(slowTime);
			moveSpeed = dollStatus.MoveSpeed;
			curBehavior.PushSuccessorState(bvNormal);
		}

		IEnumerator Expose(float ExposeTime)
		{
			CharacterLayerChange(CharacterModel, 6);
			//originMaterial = skinnedMeshRenderer.material;
			//skinnedMeshRenderer.material = Resources.Load<Material>("Materials/Always Visible");
			yield return new WaitForSeconds(ExposeTime);
			CharacterLayerChange(CharacterModel, 0);
			//skinnedMeshRenderer.material = originMaterial;
			curBehavior.PushSuccessorState(bvNormal);
		}

		public void CharacterLayerChange(GameObject Model,int layer)
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


		/*---cheat---*/
		private bool isCheat = false;
		private string cheatMethodName;
		void InputCheatKey()
		{
			if (Input.GetKeyDown(KeyCode.Alpha0))
			{
				if (isCheat)
				{
					isCheat = false;
				}
				else
				{
					isCheat = true;
				}
			}

			if (isCheat)
			{
				if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					cheatMethodName = "CheatFallDown";
					photonView.RPC(cheatMethodName,RpcTarget.All);
				}
				else if (Input.GetKeyDown(KeyCode.Alpha1))
				{
					cheatMethodName = "CheatGhost";
					photonView.RPC(cheatMethodName, RpcTarget.All);
				}
				
			}


		}

		[PunRPC]
		void CheatFallDown()
		{
			dollStatus.HitDollHP(dollStatus.DollHealthPoint);
		}

		[PunRPC]
		void CheatGhost()
		{
			dollStatus.HitDevilHP(dollStatus.DevilHealthPoint);
		}
	}
}