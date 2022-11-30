using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib.Object;
using LSH_Lib;
namespace GHJ_Lib
{
	public class RabbitSkill: DollSkill
	{
		public float HealAmount = 10.0f;
		public bool IsHeal = false;
		protected Sk_Heal SkHeal = new Sk_Heal();
		protected WaitForEndOfFrame frame = new WaitForEndOfFrame();
		protected InteractionPromptUI interactionPromptUI;
		protected string NoticeTextCanDoSkill = "Push SkillButton To Heal Peer";
		protected DollController HealTarget;

		public AudioPlayer Audio;
		protected override void OnEnable()
		{
			base.OnEnable();
			interactionPromptUI = StageManager.Instance.InteractionPrompt;
			Controller.AllocSkill(new BvRabbitActSkill());
			SkillSetting();
		}

		public override bool ShowCanUseSkillMsg()
		{
			if (IsHeal)
			{
				return false;
			}
			if (actSkillArea.CanGetTarget())
			{
				GameObject HealtargetObj = actSkillArea.GetNearestTarget();
				if (HealTarget == null|| HealtargetObj != HealTarget.gameObject)
				{ 
					HealTarget = HealtargetObj.GetComponent<DollController>();
				}

				if ((DataManager.Instance.PlayerDatas[HealTarget.PlayerIndex].roleData as DollData).DollHP < (DataManager.Instance.RoleInfos[HealTarget.TypeIndex] as DollData).DollHP)
				{
					interactionPromptUI.Activate(NoticeTextCanDoSkill);
				}
				else
				{
					interactionPromptUI.Inactivate();
				}
				
			}
			else
			{
				HealTarget = null;
				interactionPromptUI.Inactivate();
			}
			return false;

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
			Audio.Play("RabbitSkill");
			//½ºÅ³Áß
			yield return new WaitForSeconds(0.2f);//¼±µô
			IsHeal = true;
			if (Controller.IsMine)
			{ 
				StageManager.Instance.dollUI.CharacterSkill.PushButton(true);
				HealTarget.BeHealed_RPC(IsHeal);
			}
			
			while (true)
			{
				ActiveSkill.Update(Controller, ref ActiveSkill);
				yield return frame;
				if (!IsHeal)
				{
					if (Controller.IsMine)
					{
						StageManager.Instance.dollUI.CharacterSkill.PushButton(false);
						HealTarget.BeHealed_RPC(IsHeal);
						Controller.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
					}
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