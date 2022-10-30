using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvCatch: Behavior<NetworkBaseController>
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/
		

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/
      

        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.BaseAnimator.Play("Pickup");
            actor.SetMoveInput(true);
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if (actor.photonView.IsMine)
            {
                actor.DoImprison();
            }


            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();

            if (Bv is BvImprison)
            {
                return Bv;
            }
            return null;

        }

        /*--- Private Methods ---*/
    }
}