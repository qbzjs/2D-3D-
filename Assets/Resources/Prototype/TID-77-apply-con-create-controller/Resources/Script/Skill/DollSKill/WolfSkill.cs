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
			//��ų��
			yield return new WaitForSeconds(0.2f);//����

			while (ActiveSkill.Count != 0)
			{
				ActiveSkill.Update(Controller, ref ActiveSkill);
			}
			SkillSetting();
			yield return new WaitForSeconds(0.2f);//�ĵ�
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
			if (actSkillArea.CanGetTarget())
			{
				actSkillArea.Targets[0].GetComponent<ExorcistController>().DoActionBy(Detected);
			}
		}
		IEnumerator Detected(GameObject characterModel)
		{
			StageManager.CharacterLayerChange(characterModel, 6); //6 : �����°�
			yield return new WaitForSeconds(5);//�ð��� CSV�� ������ �Ǵ� �������� ���Ƿ� 5�� �س���
			StageManager.CharacterLayerChange(characterModel, 7); //7: �������·ε��ƿ�
		}

		/*--- Private Methods ---*/
		private void SkillSetting()
		{
			ActiveSkill.PushSuccessorState(skDefault);
			ActiveSkill.PushSuccessorState(skDetected);
		}

    }
}