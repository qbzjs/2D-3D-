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
		protected DollController doll;
		/*--- Public Methods ---*/
		public void SetInteractObj(Interaction interaction)
		{
			this.interactionObj = interaction as PurificationBox;

		}
		public void SetCaughtDoll(GameObject dollObj)
		{
			doll = dollObj.GetComponent<DollController>();
		}
		/*--- Protected Methods ---*/
		protected override void Activate(in BasePlayerController actor)
		{
			if (actor is ExorcistController)
			{
				actor.BaseAnimator.Play("Imprison");
			}
			if (actor.photonView.IsMine)
			{ 
				BarUI.Instance.SetTarget(null);
				(actor as NetworkBaseController).Interact("AutoCastingNull");
			}

		}

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
			if (actor.IsAutoCasting)
			{
				return null;
			}
			(actor as ExorcistController).ImprisonDoll(interactionObj.CamTarget);
			interactionObj.PurifyDoll(doll);
			doll.Imprisoned(interactionObj);
			return new BvIdle();
        }

        /*--- Public Methods---*/

        /*--- Protected Methods ---*/

        /*--- Private Methods ---*/
    }

}

