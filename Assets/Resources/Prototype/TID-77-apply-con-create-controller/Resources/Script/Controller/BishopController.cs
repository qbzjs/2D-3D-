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

		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/
		public override void OnEnable()
		{
			base.OnEnable();
			SkillSettingToInstallCross();
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

        protected override IEnumerator ExcuteActiveSkil()
		{
			useActiveSkill = true;
			ChangeSkillBehaviorTo();
			yield return new WaitForSeconds(0.2f);//¼±µô
			while (ActiveSkill.Count != 0)
			{
				ActiveSkill.Update(this, ref ActiveSkill);
			}
			yield return new WaitForSeconds(0.2f);//ÈÄµô
			ChangeBehaviorTo(BehaviorType.Idle);//½ºÅ³³¡
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
	}
}