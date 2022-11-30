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

			//actor.BaseAnimator.SetBool("IsInstallCross", true);
			actor.ShareAnimationBoll( "IsInstallCross", true );

			bishopSkill.StartCoroutine("SetCross");
			
			actor.ChangeMoveFunc(NetworkBaseController.MoveType.Stop);
		}

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
			return PassIfHasSuccessor();
        }
	}
}