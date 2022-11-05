using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;

namespace GHJ_Lib
{
	public class BishopController: ExorcistController
	{
		public GameObject CrossPrefab;
		public float InstallRange { get; protected set; } = 4.0f;
		public int CrossMaxCount = 5;
		public int InstallCross = 0;

		protected Sk_Default skDefault = new Sk_Default();
		protected Sk_InstallCross skInstallCross = new Sk_InstallCross();
		protected Sk_CollectCross skCollectCross = new Sk_CollectCross();
		public override void OnEnable()
		{
			base.OnEnable();
			BvActiveSkill = new BvBishopActSkill(); // 스킬은 각 직업, 및 캐릭터별로 바뀔것.
			SkillSettingToInstallCross();
		}

		/*---Skill---*/

		protected override IEnumerator ExcuteActiveSkil()
		{
			useActiveSkill = true;
			yield return new WaitForSeconds(0.2f);//선딜
			while (ActiveSkill.Count != 0)
			{
				ActiveSkill.Update(this, ref ActiveSkill);
			}
			yield return new WaitForSeconds(0.2f);//후딜
			yield return new WaitForSeconds(14.6f);
			useActiveSkill = false;
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
				AnimatorStateInfo animatorState = BaseAnimator.GetCurrentAnimatorStateInfo(0);
				if (animatorState.normalizedTime.Equals(1.0f))
				{
					GameObject cross = GameObject.Instantiate(CrossPrefab, transform);
					cross.transform.SetParent(this.transform.parent);
					break;
				}
			}
			
		}
	}
}