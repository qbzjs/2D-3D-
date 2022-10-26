using KSH_Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class BvAttack: Behavior<BasePlayerController>
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
            (actor as ExorcistController).attackBox.gameObject.SetActive(true);
        }

        
        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            if (actor.BaseAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.9)
            {
                (actor as ExorcistController).attackBox.gameObject.SetActive(false);
                return new BvIdle();
            }
            return null;
        }
        /*--- Private Methods ---*/
    }
}