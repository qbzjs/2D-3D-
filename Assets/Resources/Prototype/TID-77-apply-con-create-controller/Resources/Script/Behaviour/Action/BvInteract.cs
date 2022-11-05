using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvInteract: Behavior<NetworkBaseController>
	{
		protected override void Activate(in NetworkBaseController actor)
		{
			PlayAnimation( actor );

			if (actor.photonView.IsMine)
			{
				//actor.InteractBy(actor.castingType);
			}

			actor.ChangeMoveFunc(false);
		}

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
			if (actor.photonView.IsMine)
			{
				// Interact �ϼ���
			}

			Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
			if (Bv is BvIdle)
			{
				return Bv;
			}
			return null;
        }


		void PlayAnimation( in NetworkBaseController actor )
        {
			if ( actor is DollController )
			{
				actor.BaseAnimator.CrossFade( "Attack" ,0.5f);
			}
			else if ( actor is ExorcistController )
			{
				actor.BaseAnimator.CrossFade( "Kick" ,0.5f);
			}
		}
    }
}