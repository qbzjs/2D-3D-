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
				actor.BaseAnimator.CrossFade("Imprison",0.5f);
			}

			actor.ChangeMoveFunc(false);
		}

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
			actor.ImprisonDoll();
			return new BvIdle();
        }
	}

}

