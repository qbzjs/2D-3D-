using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Object;
using Photon.Pun;
using Photon.Realtime;

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

		protected GameObject targetCross;
		protected Sk_InstallCross skInstallCross = new Sk_InstallCross();
		protected Sk_CollectCross skCollectCross = new Sk_CollectCross();

		protected override void OnEnable()
		{
			base.OnEnable();
			PhotonNetwork.PrefabPool.RegisterPrefab(CrossPrefabName, CrossPrefab);
			Controller.AllocSkill(new BvBishopActSkill());
			
			PoketInCross.Add(60.0f);
			PoketInCross.Add(60.0f);
			PoketInCross.Add(60.0f);
			PoketInCross.Add(60.0f);
			PoketInCross.Add(60.0f);
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
        public override bool CanActiveSkill()
		{
			if (actSkillArea.CanGetTarget())
			{
				GameObject target = actSkillArea.GetNearestTarget();
				if (Controller.IsWatching(target)&&Vector3.Distance(target.transform.position,transform.position)< CollectRange)
				{
					targetCross = target;
					SkillSettingToCollectCross();
					return true;
				}
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
		protected override IEnumerator ExcuteActiveSkill()
		{
			IsCoolTime = true;
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
				if (animatorState.normalizedTime >=0.6f)
				{
					Controller.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
					Controller.BaseAnimator.SetBool("IsInstallCross", false);

					GameObject cross = PhotonNetwork.Instantiate(CrossPrefabName, new Vector3(transform.position.x,transform.position.y +0.23f,transform.position.z), CrossPrefab.transform.rotation);

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
				if (animatorState.normalizedTime >= 0.6f)
				{
					Controller.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
					Controller.BaseAnimator.SetBool("IsCollectCross", false);

					PoketInCross.Add(targetCross.GetComponent<Cross>().OriginGauge);

					actSkillArea.RemoveInList(targetCross);
					PhotonNetwork.Destroy(targetCross);
					break;
				}
			}
		}
        /*--animation Event--*/

    }
}