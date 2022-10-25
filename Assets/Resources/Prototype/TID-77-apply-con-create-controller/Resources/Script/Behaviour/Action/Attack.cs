using KSH_Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class Attack: Behavior<BasePlayerController>
    {
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            if (actor.BaseAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return;
            }
            actor.BaseAnimator.Play("Attack");
        }

        
        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            if (actor.BaseAnimator.GetCurrentAnimatorStateInfo(0).length<=0.9)
            {
                return null;
            }
            return new Idle();
        }
        /*--- Private Methods ---*/
    }
}