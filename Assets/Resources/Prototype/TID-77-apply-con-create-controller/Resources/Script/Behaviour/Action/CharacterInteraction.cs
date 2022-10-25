using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class CharacterInteraction: Behavior<BasePlayerController>
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/
		protected DollController dollController;

		/*--- Private Fields ---*/

		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/
		protected override void Activate(in BasePlayerController actor)
		{
			dollController = (actor as DollController);
			dollController.Animator.Play("Attack");
		}

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
			//if(dollController.Animator.GetCurrentAnimatorStateInfo(0).length >
			

			Behavior<BasePlayerController> behavior = base.DoBehavior(actor);
			if (behavior is Idle)
			{
				dollController.Animator.Play("Idle_A");
				return behavior;
			}
			return null;
        }
        /*--- Private Methods ---*/
    }
}