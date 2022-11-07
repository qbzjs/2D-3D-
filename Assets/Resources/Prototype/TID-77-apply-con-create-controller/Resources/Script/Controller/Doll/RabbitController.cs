using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;

namespace GHJ_Lib
{
	public class RabbitController: DollController
	{
		protected Sk_Default skDefault = new Sk_Default();

		public override void OnEnable()
		{
			base.OnEnable();
			BvActiveSkill = new BvRabbitActSkill();
			SkillSetting();
		}

		/*---Skill---*/

		protected override IEnumerator ExcuteActiveSkil()
		{
			useActiveSkill = true;
			//��ų��
			yield return new WaitForSeconds(0.2f);//����

			while (ActiveSkill.Count != 0)
			{
				ActiveSkill.Update(this, ref ActiveSkill);
			}
			SkillSetting();
			yield return new WaitForSeconds(0.2f);//�ĵ�
			ChangeBehaviorTo(BehaviorType.Idle);
			yield return new WaitForSeconds(14.6f);
			useActiveSkill = false;
		}

		private void SkillSetting()
		{
			ActiveSkill.PushSuccessorState(skDefault);
		}
	}
}