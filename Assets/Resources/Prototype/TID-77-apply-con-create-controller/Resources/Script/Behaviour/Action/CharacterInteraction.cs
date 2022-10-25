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
		protected Interaction interactionObj;

		/*--- Private Fields ---*/

		/*--- Public Methods ---*/
		public void SetInteractObj(Interaction interaction)
		{
			this.interactionObj = interaction;
		}

		/*--- Protected Methods ---*/
		protected override void Activate(in BasePlayerController actor)
		{
			actor.BaseAnimator.Play("Attack");
		}

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
			//if(dollController.Animator.GetCurrentAnimatorStateInfo(0).length >
			Behavior<BasePlayerController> behavior = base.DoBehavior(actor);
			if (behavior is Idle)
			{
				actor.BaseAnimator.Play("Idle_A");
				return behavior;
			}
			this.interactionObj.Interact(actor);
			return null;
        }
        /*--- Private Methods ---*/
    }
}