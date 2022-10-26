using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class Catch: Behavior<BasePlayerController>
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

        
        /*--- Private Methods ---*/
    }
}