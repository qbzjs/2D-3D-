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
            (actor as DollController).trapInteractor.SetActive(true);
        }
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {

            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (Bv is BvIdle)
            {
                (actor as DollController).trapInteractor.SetActive(false);
                return Bv;
            }
            else if (Bv is BvBeCaught)
            {
                (actor as DollController).trapInteractor.SetActive(false);
                return Bv;
            }
            return null;
        }
    }
}