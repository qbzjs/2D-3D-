using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvDown: Behavior<BasePlayerController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/
        protected float UpGauge=0.0f;

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            UpGauge = 0.0f;
            actor.BaseAnimator.Play("Death");
        }

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            if (UpGauge >= 1.0f)
            {
                return new BvIdle();
            }

            return null;
        }

        /*--- Private Methods ---*/
    }
}