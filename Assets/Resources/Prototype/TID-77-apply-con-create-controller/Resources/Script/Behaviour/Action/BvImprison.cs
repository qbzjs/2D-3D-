using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
    public class BvImprison : Behavior<BasePlayerController>
    {
		/*--- Public Fields ---*/

		/*--- Protected Fields ---*/

		protected PurificationBox interactionObj;
		protected CastingType castingType;

		/*--- Public Methods ---*/
		public void SetInteractObj(Interaction interaction)
		{
			this.interactionObj = interaction as PurificationBox;

		}

		/*--- Protected Methods ---*/
		protected override void Activate(in BasePlayerController actor)
		{
			if (actor is ExorcistController)
			{
				actor.BaseAnimator.Play("Imprison");
			}

			BarUI.Instance.SetTarget(null);
			actor.Interact("AutoCastingNull");

		}

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
			if (actor.IsAutoCasting)
			{
				return null;
			}
			(actor as ExorcistController).ImprisonDoll(interactionObj.CamTarget);
			interactionObj.PurifyDoll();
			return new BvIdle();
        }

        /*--- Public Methods---*/

        /*--- Protected Methods ---*/

        /*--- Private Methods ---*/
    }

}

