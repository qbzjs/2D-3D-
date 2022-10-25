using KSH_Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class Run: Behavior<BasePlayerController>
    {
        /*--- Public Fields ---*/
        

        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            actor.BaseAnimator.Play("Run");
        }

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {

            Behavior<BasePlayerController> nextAction = base.DoBehavior(actor);
            if (nextAction != null)
            {
                return nextAction;
            }

                return null; 
            
        }

        /*--- Private Methods ---*/
    }
}