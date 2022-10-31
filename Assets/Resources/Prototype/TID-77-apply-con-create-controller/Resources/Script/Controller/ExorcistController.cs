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
		/*--- Public Fields ---*/
	
		public ParticleSystem Ayra;
		public GameObject[] CatchObj;

		public PickUpBox pickUpBox;
		public AttackBox attackBox;

		/*--- Protected Fields ---*/

		protected BvIdle idle = new BvIdle();
		protected BvAttack attack = new BvAttack();
		protected BvCharacterInteraction interact = new BvCharacterInteraction();
		protected BvCatch catchDoll = new BvCatch();
		protected BvImprison imprison = new BvImprison();

		protected GameObject caughtDoll;

		/*--- Private Fields ---*/

		/*--- MonoBehaviour Callbacks ---*/


		public override void OnEnable()
		{
			base.OnEnable();
			// 스테이터스 받아오기

			//애니매이터 받기 -> behavior에서 할예정

			// 카메라 설정하기
			if (photonView.IsMine)
			{
				fpvCam.InitCam(camTarget);
				tpvCam.InitCam(camTarget);
				fpvCam.gameObject.SetActive(true);
				curCam = fpvCam;
				//StartCoroutine("CameraActive");
				//tpvCam.gameObject.SetActive(false);
			}
			else
			{
				fpvCam.InitCam(camTarget);
				tpvCam.InitCam(camTarget);
				//fpvCam.gameObject.SetActive(false);
				//tpvCam.gameObject.SetActive(false);
			}
			// CurcharacterAction, CurcharacterCondition,  초기설정하기
			CurCharacterAction.PushSuccessorState(idle);


		}


		IEnumerator CameraActive()
		{

			yield return new WaitForSeconds(3);
			fpvCam.gameObject.SetActive(true);
			curCam = fpvCam;
		}

		/*
		protected override void OnTriggerStay(Collider other)
		{

			if (other.CompareTag("interactObj"))
			{
				interactObj = other.GetComponent<Interaction>();
				
				if (!photonView.IsMine)
				{
					return;
				}
				Log.Instance.WriteLog(interactObj.name.ToString(), 1);
				if (IsAutoCasting)
				{
					canInteract = false;
					BarUI.Instance.SliderVisible(true);
					BarUI.Instance.TextVisible(false);
				}
				else if (IsCasting)
				{
					canInteract = true;
					BarUI.Instance.SliderVisible(true);
					BarUI.Instance.TextVisible(false);
					if (!interactObj.CanActiveToExorcist)
					{
						canInteract = false;
					}
					return;
				}
				else
				{
					if (CurCharacterAction is BvCharacterInteraction)
					{
						ChangeActionTo("Idle");
					}
					BarUI.Instance.SliderVisible(false);
				}

				if (Vector3.Dot(forward, Vector3.ProjectOnPlane((other.transform.position - this.transform.position), Vector3.up)) > 0 &&
					interactObj.CanActiveToExorcist)
				{
					BarUI.Instance.TextVisible(true);
					canInteract = true;

				}
				else
				{
					BarUI.Instance.TextVisible(false);
					canInteract = false;
				}

			}
		}

		protected override void OnTriggerExit(Collider other)
		{
			if (other.CompareTag("interactObj"))
			{
				BarUI.Instance.TextVisible(false);
				BarUI.Instance.SliderVisible(false);
			}
		}
		*/

		/*--- Public Methods ---*/


		/*---Skill---*/
		[PunRPC]
		public override void DoActiveSkill()
		{
			StartCoroutine("ActiveSkillBox");
		}

		protected override IEnumerator ActiveSkillBox()
		{
			useActiveSkill = true;
			//스킬중
			yield return new WaitForSeconds(0.2f);//선딜
			
			yield return new WaitForSeconds(13.8f);
			useActiveSkill = false;
		}



		/*---HIT_SKILL---*/
		public void HitSkillBy(string skillname)
		{
			switch (skillname)
			{
				case "wolfActSkill":
					{
						StartCoroutine("WolfActSkill");
					}
					break;
			}
		}


		IEnumerator WolfActSkill()
		{
			CharacterLayerChange(characterModel, 6);
			yield return new WaitForSeconds(5);
			CharacterLayerChange(characterModel, 7);

		}



		public override void DoImprison()
		{
			if(Input.GetKeyDown(KeyCode.Mouse0))
			{
				if (canInteract)
				{
					ChangeActionTo("Interact");
				}
			}
		}

		public virtual void DoPickUp()
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				ChangeActionTo("Catch");
			}
		}

		public override void Imprison()
		{
			DollController doll = caughtDoll.GetComponent<DollController>();
			CatchObj[doll.TypeIndex - 5].gameObject.SetActive(false);
			//doll.ChangeCamera((interactObj as PurificationBox).Cam);
			doll.Imprisoned((interactObj as PurificationBox));
		}



		/*--- Protected Methods ---*/


		protected override void PlayerInput()
		{
			if (Input.GetKeyDown(KeyCode.Mouse1))
			{
				if (!useActiveSkill)
				{
					photonView.RPC("DoActiveSkill", RpcTarget.AllViaServer);
				}
			}

			Debug.Log("CanPickUp : " + pickUpBox.CanPickUp());
			if (pickUpBox.CanPickUp()&&(CurCharacterAction is not BvCatch))
			{
				if (Input.GetKeyDown(KeyCode.Mouse0))
				{
					ChangeActionTo("Catch");
					return;
				}
			}

			if (canInteract)
			{
				if (Input.GetKeyDown(KeyCode.Mouse0))
				{

					ChangeActionTo("Interact");
					return;
				}
				if (Input.GetKeyUp(KeyCode.Mouse0))
				{

					ChangeActionTo("Idle");
					return;
				}
			}
			else
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

		}

        protected override void RotateToDirection()
		{

			if (direction.sqrMagnitude > 0.01f)
			{
				animator.SetFloat("MoveSpeed", direction.magnitude);
				//forward = Vector3.Slerp(characterModel.transform.forward, direction,
				//	rotateSpeed * Time.deltaTime / Vector3.Angle(characterModel.transform.forward, direction));
				//characterModel.transform.LookAt(characterModel.transform.position + forward);
			}
			else
			{
				animator.SetFloat("MoveSpeed", 0);
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
		protected override void _ChangeActionTo(string ActionName)
		{
			switch (ActionName)
			{
				case "Idle":
					{
						CurCharacterAction.PushSuccessorState(idle);
					}
					break;
				case "Attack":
					{
						CurCharacterAction.PushSuccessorState(attack);
					}
					break;
				case "Interact":
					{
						if (CurCharacterAction is BvIdle)
						{
							CurCharacterAction.PushSuccessorState(interact);
						}
						if (CurCharacterAction is BvCatch)
						{
							CurCharacterAction.PushSuccessorState(imprison);
						}
					}
					break; 
				case "Catch":
					{
						caughtDoll = pickUpBox.FindNearestFallDownDoll();
						CurCharacterAction.PushSuccessorState(catchDoll);
					}
					break;
			}


		}

		[PunRPC]
		protected override void _AddCondition(string ConditionName)
		{
			switch (ConditionName)
			{
				
			}

		}


		/*--- Private Methods ---*/








		/*--- AnimationCallbacks Methods ---*/

		public void PickUp()
		{
			DollController doll = caughtDoll.GetComponent<DollController>();
			CatchObj[doll.TypeIndex-5].gameObject.SetActive(true);
			CharacterLayerChange(caughtDoll, 8);
			doll.CaughtDoll(tpvCam);
		}



        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
			if (stream.IsWriting)
			{

				stream.SendNext(characterModel.transform.rotation.x);
				stream.SendNext(characterModel.transform.rotation.y);
				stream.SendNext(characterModel.transform.rotation.z);

				stream.SendNext(this.transform.position.x);
				stream.SendNext(this.transform.position.y);
				stream.SendNext(this.transform.position.z);

			}
			if (stream.IsReading)
			{
				float x	= (float)stream.ReceiveNext();
				float y = (float)stream.ReceiveNext();
				float z = (float)stream.ReceiveNext();

				characterModel.transform.rotation = Quaternion.Euler(new Vector3(x, y, z));  


				this.transform.position = new Vector3((float)stream.ReceiveNext(), (float)stream.ReceiveNext(), (float)stream.ReceiveNext());
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