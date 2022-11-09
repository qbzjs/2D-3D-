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

        protected override IEnumerator ExcuteActiveSkill()
		{
			IsCoolTime = true;
			//��ų��
			yield return new WaitForSeconds(0.2f);//����
			IsHeal = true;
			while (true)
			{
				ActiveSkill.Update(Controller, ref ActiveSkill);
				yield return new WaitForEndOfFrame();
				if (!IsHeal)
				{
					Controller.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
					break;
				}
			}
			yield return new WaitForSeconds(0.2f);//�ĵ�
			SkillSetting();
			yield return new WaitForSeconds(14.6f);
			IsCoolTime = false;
		}

		private void SkillSetting()
		{
			ActiveSkill.PushSuccessorState(skDefault);
			ActiveSkill.PushSuccessorState(SkHeal);
		}
	}
}