using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvDown: Behavior<NetworkBaseController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/
        protected float UpGauge=0.0f;

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkBaseController actor)
        {
            UpGauge = 0.0f;
            actor.BaseAnimator.Play("Death");
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            
            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (UpGauge >= 1.0f)
            {
                return new BvIdle();
            }
            
            return null;
        }

        /*--- Private Methods ---*/
    }
}