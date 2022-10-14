using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TID42;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvRage: Behavior<FPV_CharacterController1>
	{
		/*--- Public Fields ---*/
        public float RageDuration
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
            actor.StartCoroutineOnRage();
        }

        /*--- Private Methods ---*/
    }
}