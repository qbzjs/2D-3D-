using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class Sk_CollectCross: Behavior<NetworkBaseController>
	{
		protected BishopSkill bishopSkill;

		protected override void Activate(in NetworkBaseController actor)
		{

			actor.ChangeMoveFunc(NetworkBaseController.MoveType.Stop);

			if (bishopSkill == null)
			{
				bishopSkill = (actor.skill as BishopSkill);
			}
			actor.BaseAnimator.SetBool("IsCollectCross", true);
			bishopSkill.StartCoroutine("CollectCross");
		}
		protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
		{
			return PassIfHasSuccessor();
		}
	}
}