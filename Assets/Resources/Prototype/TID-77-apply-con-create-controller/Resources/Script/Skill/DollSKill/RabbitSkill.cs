using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib.Object;

namespace GHJ_Lib
{
	public class RabbitSkill: DollSkill
	{
		public float HealAmount = 10.0f;
		public bool IsHeal = false;
		protected Sk_Heal SkHeal = new Sk_Heal();

		protected override void OnEnable()
		{
			base.OnEnable();
			Controller.AllocSkill(new BvRabbitActSkill());
			SkillSetting();
		}

		public override void DecideActiveSkill()
		{

		}
		public override bool CanActiveSkill()
		{
			return actSkillArea.CanGetTarget();
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

        public override IEnumerator ExcuteActiveSkill()
		{
			IsCoolTime = true;
			//½ºÅ³Áß
			yield return new WaitForSeconds(0.2f);//¼±µô
			IsHeal = true;
			StageManager.Instance.dollUI.CharacterSkill.PushButton(true);
			while (true)
			{
				ActiveSkill.Update(Controller, ref ActiveSkill);
				yield return new WaitForEndOfFrame();
				if (!IsHeal)
				{
					StageManager.Instance.dollUI.CharacterSkill.PushButton(false);
					Controller.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
					break;
				}
			}
			yield return new WaitForSeconds(0.2f);//ÈÄµô
			SkillSetting();
			//yield return new WaitForSeconds(14.6f);
			IsCoolTime = false;
		}

		private void SkillSetting()
		{
			ActiveSkill.PushSuccessorState(skDefault);
			ActiveSkill.PushSuccessorState(SkHeal);
		}
	}
}