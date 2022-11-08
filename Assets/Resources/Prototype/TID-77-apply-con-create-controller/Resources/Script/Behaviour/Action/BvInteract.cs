using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvInteract: Behavior<NetworkBaseController>
	{
		protected override void Activate(in NetworkBaseController actor)
		{
			PlayAnimation( actor );


			actor.ChangeMoveFunc(false);
		}

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
			Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
			if (Bv is BvIdle)
			{
				actor.BaseAnimator.SetBool("IsInteract", false);
				return Bv;
			}
			return null;
        }


		void PlayAnimation( in NetworkBaseController actor )
        {
			actor.BaseAnimator.SetBool("IsInteract", true);
		}
    }
}