using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvCatch: Behavior<NetworkBaseController>
	{
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.BaseAnimator.Play("Pickup");
            actor.ChangeMoveFunc(true);
        }
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if (actor.photonView.IsMine)
            {
                if ( Input.GetKeyDown( KeyCode.Mouse0 ) )
                {
                    //if() CanInteract
                    actor.ChangeBvToImprison();
                }
            }


            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();

            if (Bv is BvImprison)
            {
                return Bv;
            }
            return null;

        }
    }
}