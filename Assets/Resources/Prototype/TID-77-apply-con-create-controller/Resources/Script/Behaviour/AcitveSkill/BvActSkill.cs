using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;

namespace GHJ_Lib
{
	public class BvActSkill: Behavior<BasePlayerController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/



        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            actor.BaseAnimator.Play("install Cross");
        }

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            return null;
        }
        /*--- Private Methods ---*/
    }
}