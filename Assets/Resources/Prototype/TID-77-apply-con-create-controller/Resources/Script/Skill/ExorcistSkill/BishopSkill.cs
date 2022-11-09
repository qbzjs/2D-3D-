using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;

namespace GHJ_Lib
{
	public class BishopSkill: ExorcistSkill
	{
		public GameObject CrossPrefab;
		public float InstallRange { get; protected set; } = 4.0f;
		public int CrossMaxCount = 5;
		public int InstallCross = 0;

		protected Sk_InstallCross skInstallCross = new Sk_InstallCross();
		protected Sk_CollectCross skCollectCross = new Sk_CollectCross();
		protected override void OnEnable()
		{
			base.OnEnable();
			Controller.AllocSkill(new BvBishopActSkill());
			SkillSettingToInstallCross();
		}

		/*---Skill---*/
		public override bool CanActiveSkill()
		{
			return true;
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
					GameObject cross = GameObject.Instantiate(CrossPrefab, transform);
					cross.transform.SetParent(this.transform.parent);
					break;
				}
			}
		}
        /*--animation Event--*/

    }
}