using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvFall : Behavior<NetworkTPV_CharacterController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkTPV_CharacterController actor)
        {
            actor.FallDown();
        }

        /*--- Private Methods ---*/
    }
}