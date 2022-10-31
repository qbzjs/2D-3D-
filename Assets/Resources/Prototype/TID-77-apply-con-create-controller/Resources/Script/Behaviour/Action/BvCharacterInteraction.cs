using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvCharacterInteraction: Behavior<NetworkBaseController>
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/

		/*--- Protected Methods ---*/
		protected override void Activate(in NetworkBaseController actor)
		{
			if (actor is DollController)
			{
				actor.BaseAnimator.Play("Attack");
			}
			else if (actor is ExorcistController)
			{
				actor.BaseAnimator.Play("Kick");
			}

			if (actor.photonView.IsMine)
			{
				actor.InteractBy(actor.castingType);
			}

			actor.SetMoveInput(false);
		}

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
			if (actor.photonView.IsMine)
			{
				switch (actor.castingType)
				{

					case InteractionObj.CastingType.ManualCasting:
						{
							if (!actor.IsCasting)
							{
								actor.ChangeActionTo("Idle");
							}
						}
						break;
					case InteractionObj.CastingType.SharedAutoCasting:
					case InteractionObj.CastingType.LocalAutoCasting:
						{
							if (!actor.IsAutoCasting)
							{
								actor.ChangeActionTo("Idle");
							}
						}
						break;
					case InteractionObj.CastingType.NotCasting:
						{
							Debug.LogError("Wrong interact");
							actor.ChangeActionTo("Idle");
						}
						break;
				}
			}

			Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
			if (Bv is BvIdle)
			{
				return Bv;
			}
			return null;
        }



        /*--- Private Methods ---*/
    }
}