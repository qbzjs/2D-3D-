using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using TID42;
namespace GHJ_Lib
{
	public class BvSlowExorcist : Behavior<FPV_CharacterController1>
	{
		/*--- Public Fields ---*/
		public float Ratio
		{
			get;
			set;
		}

        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in FPV_CharacterController1 actor)
        {
            actor.StartCoroutineMoveSlow(5, Ratio);
        }

        /*--- Private Methods ---*/
    }
}