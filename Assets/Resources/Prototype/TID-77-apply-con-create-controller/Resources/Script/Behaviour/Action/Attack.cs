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
        protected ExorcistController exorcistController;

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            exorcistController = (actor as ExorcistController);

            if (exorcistController.BaseAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return;
            }
            exorcistController.BaseAnimator.Play("Attack");
        }

        
        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            if (exorcistController.BaseAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return null;
            }
            return base.DoBehavior(actor);
        }
        /*--- Private Methods ---*/
    }
}