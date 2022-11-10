using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
    public class BvImprison : Behavior<NetworkBaseController>
    {
		protected override void Activate(in NetworkBaseController actor)
		{
			if (actor is ExorcistController)
			{
				actor.BaseAnimator.SetBool("IsImprison", true);
			}

			actor.ChangeMoveFunc(false);
		}

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
			AnimatorStateInfo animatorStateInfo = actor.BaseAnimator.GetCurrentAnimatorStateInfo(0);
			if (animatorStateInfo.normalizedTime >= 0.5f&&actor.BaseAnimator.GetBool("IsImprison"))
			{
				Log.Instance.WriteLog("BvImprison animation SetBool",1);
				actor.BaseAnimator.SetBool("IsImprison", false);
				actor.ImprisonDoll();
			}

			if (animatorStateInfo.IsName("Idle"))
			{
				return new BvIdle();
			}
			return null;
        }
	}

}

