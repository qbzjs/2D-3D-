using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvCatch: Behavior<NetworkBaseController>
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
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.BaseAnimator.Play("Pickup");
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();

            if (Bv is BvImprison)
            {
                return Bv;
            }
            return null;

        }

        /*--- Private Methods ---*/
    }
}