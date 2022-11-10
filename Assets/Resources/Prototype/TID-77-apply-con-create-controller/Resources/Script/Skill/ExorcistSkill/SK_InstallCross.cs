using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class Sk_InstallCross: Behavior<NetworkBaseController>
	{
		protected BishopSkill bishopSkill;
		
		protected override void Activate(in NetworkBaseController actor)
        {
			if (bishopSkill == null)
			{
				bishopSkill = (actor.skill as BishopSkill);
			}

			if (bishopSkill.InstallCross >= bishopSkill.CrossMaxCount ) // �ֺ��� ���ڰ��� ���� ��( �ֺ������� 4) ��ġ �Ұ� -> interact������ ���ٰ�.
			{
				return;
			}
			else
			{
				actor.BaseAnimator.SetBool("IsInstallCross", true);
				bishopSkill.StartCoroutine("SetCross");
				bishopSkill.InstallCross++;
			}

			actor.ChangeMoveFunc(false);
		}

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
			return PassIfHasSuccessor();
        }
	}
}