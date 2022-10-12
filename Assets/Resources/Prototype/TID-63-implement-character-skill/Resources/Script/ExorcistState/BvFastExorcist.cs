using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using TID42;

namespace GHJ_Lib
{
	public class BvFastExorcist: Behavior<FPV_CharacterController1>
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
            actor.StartCoroutineMoveFast(5,Ratio);
        }

        /*--- Private Methods ---*/
    }
}