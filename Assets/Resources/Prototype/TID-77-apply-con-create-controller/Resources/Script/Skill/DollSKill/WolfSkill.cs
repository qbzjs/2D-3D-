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
		[SerializeField] protected SourceOfSonar sourceOfSonar;
		WaitForSeconds SonarInterval = new WaitForSeconds(0.4f);

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

		public override void DecideActiveSkill()
		{

		}
		public override bool CanActiveSkill()
		{
			return true;
		}
		/*---Skill---*/

		public override IEnumerator ExcuteActiveSkill()
		{
			if (photonView.IsMine)
			{ 
				StageManager.Instance.dollUI.CharacterSkill.StartCountDown(15.0f);
			}
			IsCoolTime = true;
			//스킬중
			yield return new WaitForSeconds(0.2f);//선딜

			while (ActiveSkill.Count != 0)
			{
				ActiveSkill.Update(Controller, ref ActiveSkill);
			}
			SkillSetting();
			yield return new WaitForSeconds(0.2f);//후딜
			Controller.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
			yield return new WaitForSeconds(14.6f);
			IsCoolTime = false;
		}

		public void DoWolfActiveSkillTo_RPC()
		{
			photonView.RPC("DoWolfActiveSkill", RpcTarget.AllViaServer);
		}

		[PunRPC]
		public void DoWolfActiveSkill()
		{
			StartCoroutine(TripleSonar());
			if (actSkillArea.CanGetTarget())
			{
				actSkillArea.Targets[0].GetComponent<ExorcistController>().DoActionBy(Detected);
			}
		}

		IEnumerator TripleSonar()
		{
			sourceOfSonar.StartCircleSonar();
			yield return SonarInterval;
			sourceOfSonar.StartCircleSonar();
			yield return SonarInterval;
			sourceOfSonar.StartCircleSonar();
		}
		IEnumerator Detected(GameObject characterModel)
		{
			StageManager.CharacterLayerChange(characterModel, 6); //6 : 빛나는거
			yield return new WaitForSeconds(5);//시간은 CSV로 받을것 또는 문서참조 임의로 5로 해놓음
			StageManager.CharacterLayerChange(characterModel, 7); //7: 원래상태로돌아옴
		}

		/*--- Private Methods ---*/
		private void SkillSetting()
		{
			ActiveSkill.PushSuccessorState(skDefault);
			ActiveSkill.PushSuccessorState(skDetected);
		}

    }
}