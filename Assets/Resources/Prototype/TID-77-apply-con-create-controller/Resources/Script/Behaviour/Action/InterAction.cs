using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class InterAction: Behavior<BasePlayerController>
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
			Behavior<BasePlayerController> behavior = base.DoBehavior(actor);
			Debug.Log(behavior);
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