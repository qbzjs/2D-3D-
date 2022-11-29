using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Object;
using Photon.Pun;
using Photon.Realtime;
using LSH_Lib;
namespace GHJ_Lib
{
	public class BishopSkill: ExorcistSkill
	{
		public GameObject CrossPrefab;
		public float InstallRange { get; protected set; } = 4.0f;
		public List<float> PoketInCross = new List<float>();
		public int CrossMaxCount = 5;
		public float RecoverGauge = 5.0f;
		public float MaxCrossGauge = 60.0f;
		protected float CollectRange = 3.0f;
		protected string CrossPrefabName = "CrossModel";
		[SerializeField] Transform spawnTransform;

		protected GameObject targetCross;
		protected GameObject targetAim;
		protected Sk_InstallCross skInstallCross = new Sk_InstallCross();
		protected Sk_CollectCross skCollectCross = new Sk_CollectCross();

		static bool isRegisterPrefab;

		LayerMask UninstallZoneLayer = 12;
		InteractionPromptUI interactionPromptUI;
		bool IsNotice = false;
		string NoticeTextUninstallArea = "This Area Can't install!!";
		string NoticeTextAlreadyInstallCrossAround = "you already install cross around";
		string NoticeTextIsWatingCross = "Push SkillButton to Collect";
		WaitForSeconds noticeTime = new WaitForSeconds(1.0f);
		public AudioPlayer AudioPlayer;
		protected override void OnEnable()
		{
			base.OnEnable();

			if(!isRegisterPrefab)
			{
				PhotonNetwork.PrefabPool.RegisterPrefab( CrossPrefabName, CrossPrefab );
				isRegisterPrefab = true;
			}

			Controller.AllocSkill(new BvBishopActSkill());
			
			PoketInCross.Add(60.0f);
			PoketInCross.Add(60.0f);
			PoketInCross.Add(60.0f);
			PoketInCross.Add(60.0f);
			PoketInCross.Add(60.0f);
			interactionPromptUI =StageManager.Instance.InteractionPrompt;
		}
        private void Update()
        {
			for(int i =0;i<PoketInCross.Count;++i)
			{
				if (MaxCrossGauge < PoketInCross[i])
				{
					PoketInCross[i] = MaxCrossGauge;
				}
				else if (MaxCrossGauge.Equals(PoketInCross[i]))
				{
					return;
				}
				else
				{ 
					PoketInCross[i] += Time.deltaTime * RecoverGauge;
				}
			}
        }
		/*---Skill---*/
		public override bool ShowCanUseSkillMsg()
		{
			GameObject targetObj;
			if (Controller.IsWatching(GameManager.CollectTriggerTag, out targetObj) && Vector3.ProjectOnPlane((targetObj.transform.position - transform.position), Vector3.up).sqrMagnitude < CollectRange * CollectRange)
			{
				interactionPromptUI.Activate(NoticeTextIsWatingCross);
				IsNotice = false;
				return true;
			}
			else
			{
				if (!IsNotice)
				{
					interactionPromptUI.Inactivate();
				}
				return false;
			}
		}
		public override bool CanActiveSkill()
		{
			Collider[] UninstallZones = new Collider[1];
			if (Physics.OverlapSphereNonAlloc(new Vector3(transform.position.x,0,transform.position.z), 1.0f, UninstallZones, UninstallZoneLayer) == 1)
			{
				StartCoroutine(NoticeUninstallReason(NoticeTextUninstallArea));
				return false;
			}

			if (actSkillArea.CanGetTarget())
			{
				targetAim = actSkillArea.GetNearestTarget();
				if (Controller.IsWatching(targetAim)&& Vector3.ProjectOnPlane((targetAim.transform.position - transform.position),Vector3.up).sqrMagnitude  < CollectRange*CollectRange)
				{
					targetCross = targetAim.transform.parent.gameObject;
					SkillSettingToCollectCross();
					return true;
				}
				StartCoroutine(NoticeUninstallReason(NoticeTextAlreadyInstallCrossAround));
				return false;
			}

			if (PoketInCross.Count == 0)
			{
				return false;
			}
			else
			{ 
				SkillSettingToInstallCross();
				return true;
			}
		}
		public override IEnumerator ExcuteActiveSkill()
		{
			IsCoolTime = true;
			if (Controller.IsMine)
			{ 
				StageManager.Instance.exorcistUI.CharacterSkill.StartCountDown(15.0f);
			}
			yield return new WaitForSeconds(0.2f);//¼±µô
			while (ActiveSkill.Count != 0)
			{
				ActiveSkill.Update(Controller, ref ActiveSkill);
			}
			yield return new WaitForSeconds(0.2f);//ÈÄµô
			yield return new WaitForSeconds(14.6f);
			IsCoolTime = false;
		}
		private void SkillSettingToInstallCross()
		{
			ActiveSkill.SetSuccessorStates(new List<Behavior<NetworkBaseController>>() {skDefault,skInstallCross });
		}
		private void SkillSettingToCollectCross()
		{
			ActiveSkill.SetSuccessorStates(new List<Behavior<NetworkBaseController>>() { skDefault, skCollectCross });
		}
		IEnumerator SetCross()
		{
			if (ActiveSkill is not Sk_InstallCross)
			{
				yield break;
			}
			while (true)
			{
				yield return new WaitForEndOfFrame();
				AnimatorStateInfo animatorState = Controller.BaseAnimator.GetCurrentAnimatorStateInfo(0);
				if (animatorState.normalizedTime >=0.6f && animatorState.IsName("install Cross"))
				{
					Controller.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
					Controller.BaseAnimator.SetBool("IsInstallCross", false);

					GameObject cross = PhotonNetwork.Instantiate(CrossPrefabName, spawnTransform.position, CrossPrefab.transform.rotation);
					AudioPlayer.Play("BishopSkill");
					PoketInCross.Sort();
					cross.GetComponent<Cross>().SetGauge(PoketInCross[PoketInCross.Count -1]);
					PoketInCross.RemoveAt(PoketInCross.Count - 1);
					//cross.transform.SetParent(this.transform.parent);
					break;
				}
			}
		}
		IEnumerator CollectCross()
		{
			if (ActiveSkill is not Sk_CollectCross)
			{
				yield break;
			}
			while (true)
			{
				yield return new WaitForEndOfFrame();
				AnimatorStateInfo animatorState = Controller.BaseAnimator.GetCurrentAnimatorStateInfo(0);
				if (animatorState.normalizedTime >= 0.2f && animatorState.IsName("Collect Cross"))
				{
					Controller.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
					Controller.BaseAnimator.SetBool("IsCollectCross", false);

					PoketInCross.Add(targetCross.GetComponent<Cross>().OriginGauge);
					AudioPlayer.Play("CollectObject");
					actSkillArea.RemoveInList(targetAim);
					PhotonNetwork.Destroy(targetCross);
					break;
				}
			}
		}

		IEnumerator NoticeUninstallReason(string reason)
		{
			if (IsNotice)
			{
				yield break;
			}
			IsNotice = true;
			interactionPromptUI.Activate(reason);
			yield return noticeTime;
			IsNotice = false;
			interactionPromptUI.Inactivate();
		}
		/*--animation Event--*/

	}
}