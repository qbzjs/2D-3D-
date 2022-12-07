using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
    public class BvBehealed : Behavior<NetworkBaseController>
    {
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.StopRotation);
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            Behavior<NetworkBaseController> Bv =  PassIfHasSuccessor();
            if (Bv is BvIdle ||
                Bv is BvGetHit)
            {
                return Bv; 
            }
            return null;
        }
    }

}

