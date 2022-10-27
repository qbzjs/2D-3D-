using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvCatch: Behavior<BasePlayerController>
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/
		protected DollController nearestDoll;

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/
        public void SetNearestDoll(GameObject doll)
        {
            nearestDoll = doll.GetComponent<DollController>();
        }

        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            actor.BaseAnimator.Play("Pickup");
        }

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            Behavior<BasePlayerController> Bv = PassIfHasSuccessor();

            if (Bv is BvImprison)
            {
                return Bv;
            }
            return null;

        }

        /*--- Private Methods ---*/
    }
}