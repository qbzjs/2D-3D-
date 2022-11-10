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

			if (bishopSkill.InstallCross >= bishopSkill.CrossMaxCount ) // 주변에 십자가가 있을 시( 주변범위는 4) 설치 불가 -> interact범위로 해줄것.
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