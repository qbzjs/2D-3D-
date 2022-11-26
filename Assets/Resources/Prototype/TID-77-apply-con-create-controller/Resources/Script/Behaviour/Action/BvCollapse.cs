using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvCollapse: Behavior<NetworkBaseController>
	{
        protected override void Activate(in NetworkBaseController actor)
        {
            if(actor.IsMine)
            {
                DataManager.Instance.ShareBehavior( (int)NetworkBaseController.BehaviorType.Collapse );
            }
            //actor.BaseAnimator.Play("Death");
            actor.BaseAnimator.SetBool("IsCollapse", true);
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.StopRotation);
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            //if (recoverRate >= 1.0f)
            //{
            //    return new BvIdle();
            //}

            if (Bv is BvBeCaught)
            {
                actor.BaseAnimator.SetBool("IsCollapse", false);
                return Bv;
            }

            return null;
        }
    }
}