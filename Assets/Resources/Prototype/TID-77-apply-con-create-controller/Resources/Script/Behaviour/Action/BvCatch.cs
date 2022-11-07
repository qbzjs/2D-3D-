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
            actor.BaseAnimator.SetBool("IsCatch", true);
            actor.ChangeMoveFunc(false);
        }
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            AnimatorStateInfo animatorStateInfo = actor.BaseAnimator.GetCurrentAnimatorStateInfo(0);

            if (animatorStateInfo.IsName("Idle"))
            {
                actor.ChangeMoveFunc(true);
            }
            else
            {
                if (animatorStateInfo.normalizedTime >= 0.5f)
                {
                    actor.BaseAnimator.SetBool("IsCatch", false);
                }
            }

            if (actor.photonView.IsMine)
            {
                if ( Input.GetKeyDown( KeyCode.Mouse0 ) )
                {
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