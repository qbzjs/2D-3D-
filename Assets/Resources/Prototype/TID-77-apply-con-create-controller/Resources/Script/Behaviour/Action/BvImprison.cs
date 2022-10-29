using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
    public class BvImprison : Behavior<NetworkBaseController>
    {
		/*--- Public Fields ---*/

		/*--- Protected Fields ---*/

		protected PurificationBox interactionObj;
		protected CastingType castingType;
		protected DollController doll;
		/*--- Public Methods ---*/
		public void SetInteractObj(InteractionObj interaction)
		{
			this.interactionObj = interaction as PurificationBox;

		}
		public void SetCaughtDoll(GameObject dollObj)
		{
			doll = dollObj.GetComponent<DollController>();
		}
		/*--- Protected Methods ---*/
		protected override void Activate(in NetworkBaseController actor)
		{
			if (actor is ExorcistController)
			{
				actor.BaseAnimator.Play("Imprison");
			}
			if (actor.photonView.IsMine)
			{ 
				BarUI_Controller.Instance.SetTarget(null);
				//actor.Interact("AutoCastingNull");
			}

		}

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
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

