using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class Caught: Behavior<BasePlayerController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/
        protected float resistGauge = 0.0f;

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            
            resistGauge = 0.0f;

        }

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            if (resistGauge > 1.0f)
            {
                //escape
            }

            return null;
        }
        /*--- Private Methods ---*/
    }
}