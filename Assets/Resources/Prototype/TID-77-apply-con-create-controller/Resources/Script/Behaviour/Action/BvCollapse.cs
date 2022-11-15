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
            actor.BaseAnimator.Play("Death");
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
                return Bv;
            }

            return null;
        }
    }
}