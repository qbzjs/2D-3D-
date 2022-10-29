using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvCharacterInteraction: Behavior<BasePlayerController>
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/
		protected Interaction interactionObj;
		protected CastingType castingType;
		/*--- Private Fields ---*/

		/*--- Public Methods ---*/
		public void SetInteractObj(Interaction interaction)
		{
			this.interactionObj = interaction;
		}
		/*--- Protected Methods ---*/
		protected override void Activate(in BasePlayerController actor)
		{
			if (actor is DollController)
			{
				actor.BaseAnimator.Play("Attack");
			}
			else if (actor is ExorcistController)
			{
				actor.BaseAnimator.Play("Kick");
			}
			castingType = interactionObj.GetCastingType(actor);
		}

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
			if (actor.IsAutoCasting)
			{
				return null;
			}

			Behavior<BasePlayerController> behavior = PassIfHasSuccessor();

			if (behavior is BvIdle)
			{
				actor.IsCasting = false;
				actor.BaseAnimator.Play("Idle_A");
				return behavior;
			}

			switch (castingType)
			{
				case CastingType.Casting:
					{
						BarUI.Instance.SetTarget(interactionObj);
						actor.IsCasting = true;
						//actor 한테서 값 받기
						float velocity = 5;
						interactionObj.AddGauge(velocity*Time.deltaTime);
						BarUI.Instance.UpdateValue();
					}
					break;
				case CastingType.AutoCasting:
					{
						BarUI.Instance.SetTarget(interactionObj);
						(actor as NetworkBaseController).Interact("AutoCasting");
					}
					break;
				case CastingType.AutoCastingNull:
					{
						BarUI.Instance.SetTarget(null);
						(actor as NetworkBaseController).Interact("AutoCastingNull");
					}
					break;
				case CastingType.NotCasting:
					{
						
					}
					break;
			}

			//this.interactionObj.Interact(actor);
			return null;
        }



        /*--- Private Methods ---*/
    }
}