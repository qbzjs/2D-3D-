using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvNormalExorcist: Behavior<NetworkExorcistController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- Public Methods ---*/

        
        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkExorcistController actor)
        {
            actor.InActive();
        }
        /*--- Private Methods ---*/
    }
}