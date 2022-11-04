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
		protected Sk_Default skDefault = new Sk_Default();
		protected SK_InstallCross skInstallCross = new SK_InstallCross();


		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/
		public override void OnEnable()
		{
			base.OnEnable();
			SkillSetting();
		}

		/*---Skill---*/
		[PunRPC]
		public override void DoActiveSkill()
		{
			StartCoroutine("ExcuteActiveSkil");
		}

		public virtual void ChangeSkillBehaviorTo()
		{
			photonView.RPC("ChangeSkillBehaviorTo_RPC", RpcTarget.AllViaServer);
		}

		[PunRPC]
        protected void ChangeSkillBehaviorTo_RPC()
        {
			//CurBehavior.PushSuccessorState(actSkill);
		}
        protected override IEnumerator ExcuteActiveSkil()
		{
			useActiveSkill = true;
			ChangeSkillBehaviorTo();
			yield return new WaitForSeconds(0.2f);//¼±µô
			while (ActiveSkill.Count != 0)
			{
				ActiveSkill.Update(this, ref ActiveSkill);
			}
			SkillSetting();
			yield return new WaitForSeconds(0.2f);//ÈÄµô
			ChangeBehaviorTo(BehaviorType.Idle);//½ºÅ³³¡
			yield return new WaitForSeconds(14.6f);
			useActiveSkill = false;
		}
		private void SkillSetting()
		{
			ActiveSkill.PushSuccessorState(skDefault);
			ActiveSkill.PushSuccessorState(skInstallCross);
			
		}
	}
}