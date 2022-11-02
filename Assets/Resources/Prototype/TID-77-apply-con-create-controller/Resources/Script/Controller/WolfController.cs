using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;

namespace GHJ_Lib
{
	public class WolfController: DollController
	{
		/*--- Public Fields ---*/
		public WolfActSkillBox actSkillBox;
		public WolfPsvSkillBox psvSkillBox;

		/*--- Protected Fields ---*/
		protected Sk_Default skDefault = new Sk_Default();
		protected Sk_Detected skDetected = new Sk_Detected();

		protected List<Behavior<NetworkBaseController>> wolfSkill = new List<Behavior<NetworkBaseController>>();


		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/

		public override void OnEnable()
		{
			base.OnEnable();
			SkillSetting();
		}

		/*--- Public Methods ---*/

		/*---Skill---*/
		[PunRPC]
		public override void DoActiveSkill()
		{
			StartCoroutine("ActiveSkillBox");
		}

		protected override IEnumerator ActiveSkillBox()
		{
			useActiveSkill = true;
			//½ºÅ³Áß
			yield return new WaitForSeconds(0.2f);//¼±µô

			while (ActiveSkill.Count != 0)
			{
				ActiveSkill.Update(this, ref ActiveSkill);
			}
			SkillSetting();
			yield return new WaitForSeconds(0.2f);//ÈÄµô
												  //½ºÅ³³¡
			yield return new WaitForSeconds(14.6f);
			useActiveSkill = false;
		}

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
		private void SkillSetting()
		{
			wolfSkill.Add(skDetected);
			wolfSkill.Add(skDefault);
			ActiveSkill.PushSuccessorStates(wolfSkill);
		}
		

		


	}
}