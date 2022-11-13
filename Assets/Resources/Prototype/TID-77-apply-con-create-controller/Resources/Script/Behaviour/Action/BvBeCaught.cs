using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvBeCaught: Behavior<NetworkBaseController>
	{
        protected override void Activate(in NetworkBaseController actor)
        {
            //resistGauge = 0.0f;
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.StopRotation);
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if (actor.photonView.IsMine)
            {
                //if (actor.DoResist())
                //{
                //    resistGauge += 1.0f;
                //}
            }

            //if (resistGauge > 10.0f)
            //{
            //    //actor.Escape
            //}

            Behavior<NetworkBaseController> Bv =  PassIfHasSuccessor();
            if (Bv is BvBePurifying)
            {
                return Bv;
            }

            return null;
        }
        /*--- Private Methods ---*/
    }
}