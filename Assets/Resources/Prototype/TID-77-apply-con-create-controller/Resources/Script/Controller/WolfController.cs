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


		/*--- Protected Fields ---*/
		

		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/

		public override void OnEnable()
		{
			base.OnEnable();

			//actSkillBox = transform.GetChild(2).gameObject;
			//actSkillBox.SetActive(false);
			//psvSkillBox = transform.GetChild(3).gameObject;
			//psvSkillBox.SetActive(true);
		}
			
		/*--- Public Methods ---*/


		/*---Skill---*/
		[PunRPC]
		public override void DoActiveSkill()
		{
			StartCoroutine("ActiveSkillBox");
		}
		/*
		protected override IEnumerator ActiveSkillBox()
		{
			useActiveSkill = true;
			//½ºÅ³Áß
			yield return new WaitForSeconds(0.2f);//¼±µô
			actSkillBox.SetActive(true);
			yield return new WaitForSeconds(0.8f);
			actSkillBox.SetActive(false);
			yield return new WaitForSeconds(0.2f);//ÈÄµô
												  //½ºÅ³³¡
			yield return new WaitForSeconds(13.8f);
			useActiveSkill = false;
		}
		*/
		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}