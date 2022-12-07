using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using Photon.Realtime;
using LSH_Lib;
namespace GHJ_Lib
{
	public class WolfSkill: DollSkill
	{
		/*--- Public Fields ---*/
		/*--- Protected Fields ---*/
		[SerializeField] protected SourceOfSonar sourceOfSonar;
		[SerializeField] protected float SonarInterval=0.4f;
		[SerializeField] protected float howlingTime = 1.2f;
		protected Sk_Detected skDetected = new Sk_Detected();
		/*--- Private Fields ---*/

		public AudioPlayer Audio;
		/*--- MonoBehaviour Callbacks ---*/

		protected override void OnEnable()
		{
			base.OnEnable();
			Controller.AllocSkill(new BvWolfActSkill());
			SkillSetting();
		}
		/*--- Public Methods ---*/

		public override bool ShowCanUseSkillMsg()
		{
			return false;
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
			Controller.ChangeMoveFunc(NetworkBaseController.MoveType.StopRotation);
			//AudioManager.instance.Play("WolfSkill");
			Audio.Play("WolfSkill");
			
			//��ų��
			yield return new WaitForSeconds(0.2f);//����

			while (ActiveSkill.Count != 0)
			{
				ActiveSkill.Update(Controller, ref ActiveSkill);
			}
			SkillSetting();
			yield return new WaitForSeconds(0.2f);//�ĵ�

			yield return new WaitForSeconds(howlingTime);

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
			sourceOfSonar.StartCircleSonar(3, SonarInterval);
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