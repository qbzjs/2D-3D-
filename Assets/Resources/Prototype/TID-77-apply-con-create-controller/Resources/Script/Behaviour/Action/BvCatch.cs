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

            if (animatorStateInfo.normalizedTime >= 0.8f && animatorStateInfo.IsName("Pickup") && actor.BaseAnimator.GetBool("IsCatch"))
            {
                actor.BaseAnimator.SetBool("IsCatch", false);
                actor.ChangeMoveFunc(true);
            }

            if (actor.BaseAnimator.GetBool("IsCatch"))
            {
                return null;
            }

            //if (actor.photonView.IsMine)
            //{
            //    if ( Input.GetKeyDown( KeyCode.Mouse0 ) )//내가 정화상자에 충분히 가까이 있는지, 보고있는지
            //    {
            //        actor.ChangeBvToImprison();
            //    }
            //}


            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();

            if (Bv is BvImprison)
            {
                return Bv;
            }
            return null;

        }
    }
}