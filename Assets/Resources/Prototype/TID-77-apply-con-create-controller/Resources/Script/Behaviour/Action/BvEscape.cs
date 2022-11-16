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
            //�ִϸ��̼��� �ִٸ� play�� ��Ŵ.
            //Default layer = 0;
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.StopRotation);
        }
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            //�ִϸ��̼��� �ִٸ� �ִϸ��̼��� ������ �Լ����� �� ��ȯ.
            return new BvIdle();
        }
        /*--- Private Methods ---*/
    }

}

