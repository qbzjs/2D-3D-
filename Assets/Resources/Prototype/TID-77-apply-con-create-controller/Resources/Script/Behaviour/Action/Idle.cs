using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class Idle: Behavior<BasePlayerController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            DollController dollController = (actor as DollController);
            dollController.Animator.Play("Idle_A");
        }
        /*--- Private Methods ---*/
    }
}