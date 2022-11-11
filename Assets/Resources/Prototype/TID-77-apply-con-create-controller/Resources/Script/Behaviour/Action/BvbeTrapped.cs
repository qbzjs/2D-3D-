using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvbeTrapped: Behavior<NetworkBaseController>
	{
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.ChangeMoveFunc(false);
        }
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (Bv is BvIdle)
            {
                return Bv;
            }
            else if (Bv is BvBeCaught)
            {
                return Bv;
            }
            return null;
        }
    }
}