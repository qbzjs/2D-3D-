using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvIdle: Behavior<BasePlayerController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
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