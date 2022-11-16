using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
using KSH_Lib.Data;
namespace GHJ_Lib
{
    public class BvEscape : Behavior<NetworkBaseController>
    {
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.behaviorType = NetworkBaseController.BehaviorType.Escape;
            //애니매이션이 있다면 play를 시킴.
            //Default layer = 0;
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.StopRotation);
        }
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            //애니매이션이 있다면 애니매이션이 끝날때 함수시작 를 반환.
            return new BvIdle();
        }
        /*--- Private Methods ---*/
    }

}

