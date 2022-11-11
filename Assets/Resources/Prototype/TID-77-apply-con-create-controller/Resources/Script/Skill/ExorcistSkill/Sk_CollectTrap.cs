using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class Sk_CollectTrap: Behavior<NetworkBaseController>
	{
		protected override void Activate(in NetworkBaseController actor)
		{
			actor.BaseAnimator.SetBool("IsCollectTrap", true);
			actor.ChangeMoveFunc(false);
		}

		protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
		{
			Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();

			if (actor.BaseAnimator.GetBool("IsCollectTrap") && Bv is BvIdle)
			{
				actor.BaseAnimator.SetBool("IsCollectTrap", false);
			}

			if (Bv is BvIdle )
			{
				actor.BaseAnimator.SetBool("IsCollectTrap", false);
			}
			return null;
		}
	}
}