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

		//protected PurificationBox interactionObj;
		//protected InteractionObj.CastingType castingType;
		//protected DollController doll;
		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/
		protected override void Activate(in NetworkBaseController actor)
		{
			if (actor is ExorcistController)
			{
				actor.BaseAnimator.Play("Imprison");
			}
			if (actor.photonView.IsMine)
			{
				actor.StartCoroutine("AutoCastingNull");
				//actor.Interact("AutoCastingNull");
			}
			actor.SetMoveInput(false);
		}

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
			if (actor.IsAutoCasting)
			{
				return null;
			}
			actor.Imprison();

			return new BvIdle();
        }

        /*--- Public Methods---*/

        /*--- Protected Methods ---*/

        /*--- Private Methods ---*/
    }

}

