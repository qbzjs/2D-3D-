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
			BarUI.Instance.BeginCasting();
			if (actor is DollController)
			{
				actor.BaseAnimator.Play("Attack");
			}
			else if (actor is ExorcistController)
			{
				actor.BaseAnimator.Play("Kick");
			}
		}

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
			//if(dollController.Animator.GetCurrentAnimatorStateInfo(0).length >
			Behavior<BasePlayerController> behavior = base.DoBehavior(actor);
			if (behavior is Idle)
			{
				BarUI.Instance.EndCasting();
				actor.BaseAnimator.Play("Idle_A");
				return behavior;
			}
			this.interactionObj.Interact(actor);
			return null;
        }
        /*--- Private Methods ---*/
    }
}