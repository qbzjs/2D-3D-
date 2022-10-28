using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvIdle: Behavior<NetworkBaseController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkBaseController actor)
        {
            if (actor is DollController)
            {
                actor.BaseAnimator.Play("Idle_A");
            }

            if (actor is ExorcistController)
            {
                actor.BaseAnimator.Play("Idle");
            }
        }

        /*--- Private Methods ---*/
    }
}