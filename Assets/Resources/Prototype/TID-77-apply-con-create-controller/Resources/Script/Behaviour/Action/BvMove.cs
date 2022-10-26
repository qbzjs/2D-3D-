using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvMove: Behavior<BasePlayerController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            Behavior<BasePlayerController> nextBehavior = base.DoBehavior(actor);
            if (nextBehavior is BvIdle)
            {
                return nextBehavior;
            }
            return null;
        }

        /*--- Private Methods ---*/
    }
}