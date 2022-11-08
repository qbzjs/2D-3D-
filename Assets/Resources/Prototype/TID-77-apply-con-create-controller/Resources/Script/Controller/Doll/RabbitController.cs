using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib.Object;

namespace GHJ_Lib
{
	public class RabbitController: DollController
	{
		public float HealAmount = 10.0f;
		public bool IsHeal = false;
		protected Sk_Default skDefault = new Sk_Default();
		protected Sk_Heal SkHeal = new Sk_Heal();

		public override void OnEnable()
		{
			base.OnEnable();
			BvActiveSkill = new BvRabbitActSkill();
			SkillSetting();
		}

        /*---Skill---*/
        public override void DoSkill()
        {
			if(actSkillArea.CanGetTarget())
			{
				base.DoSkill();
			}
        }
		public void CancelHeal()
		{
			photonView.RPC("CancelHealTo_RPC", RpcTarget.All);
		}

		[PunRPC]
		public void CancelHealTo_RPC()
		{
			IsHeal = false;
		}

        protected override IEnumerator ExcuteActiveSkil()
		{
			useActiveSkill = true;
			//½ºÅ³Áß
			yield return new WaitForSeconds(0.2f);//¼±µô
			IsHeal = true;
			while (true)
			{
				ActiveSkill.Update(this, ref ActiveSkill);
				yield return new WaitForEndOfFrame();
				if (!IsHeal)
				{
					ChangeBehaviorTo(BehaviorType.Idle);
					break;
				}
			}
			yield return new WaitForSeconds(0.2f);//ÈÄµô
			SkillSetting();
			yield return new WaitForSeconds(14.6f);
			useActiveSkill = false;
		}

		private void SkillSetting()
		{
			ActiveSkill.PushSuccessorState(skDefault);
			ActiveSkill.PushSuccessorState(SkHeal);
		}
	}
}