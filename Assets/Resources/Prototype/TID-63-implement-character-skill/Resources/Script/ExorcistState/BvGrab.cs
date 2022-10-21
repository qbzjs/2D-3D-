using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvGrab: Behavior<NetworkExorcistController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- Public Methods ---*/
        public override void Update(in NetworkExorcistController actor, ref Behavior<NetworkExorcistController> state)
        {
            Behavior<NetworkExorcistController> newState = DoBehavior(actor);
            if (newState is BvNormalExorcist) // Grab���� �ٲ���ִ� ���¸��� ���.
            {
                PushSuccessorState(newState);
                base.Update(actor, ref state);
            }
        }

        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}