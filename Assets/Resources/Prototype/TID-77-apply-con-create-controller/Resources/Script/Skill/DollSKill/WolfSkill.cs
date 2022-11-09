using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;

namespace GHJ_Lib
{
	public class WolfSkill: DollSkill
	{
		/*--- Public Fields ---*/
		/*--- Protected Fields ---*/
		protected Sk_Detected skDetected = new Sk_Detected();

		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/

		protected override void OnEnable()
		{
			base.OnEnable();
			Controller.AllocSkill(new BvWolfActSkill());
			SkillSetting();
		}
		/*--- Public Methods ---*/

		public override bool CanActiveSkill()
		{
			return true;
		}
		/*---Skill---*/

		protected override IEnumerator ExcuteActiveSkill()
		{
			IsCoolTime = true;
			//½ºÅ³Áß
			yield return new WaitForSeconds(0.2f);//¼±µô

			while (ActiveSkill.Count != 0)
			{
				ActiveSkill.Update(Controller, ref ActiveSkill);
			}
			SkillSetting();
			yield return new WaitForSeconds(0.2f);//ÈÄµô
			Controller.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
			yield return new WaitForSeconds(14.6f);
			IsCoolTime = false;
		}

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
		private void SkillSetting()
		{
			ActiveSkill.PushSuccessorState(skDefault);
			ActiveSkill.PushSuccessorState(skDetected);
		}

    }
}