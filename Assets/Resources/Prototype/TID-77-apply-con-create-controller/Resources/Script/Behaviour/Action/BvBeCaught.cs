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
            if ( actor.IsMine )
            {
                DataManager.Instance.ShareBehavior( (int)NetworkBaseController.BehaviorType.BeCaught );
            }
            //resistGauge = 0.0f;
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.StopRotation);
            actor.BaseAnimator.SetBool("IsCaught", true);
            if (actor.IsMine)
            { 
                actor.CurCam.ActiveCameraControl(true);
                actor.CurCam.ActiveCameraUpdate(true);
            }
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            actor.transform.SetPositionAndRotation(StageManager.Instance.Exorcist.catchTransform.position, StageManager.Instance.Exorcist.catchTransform.rotation);

            Behavior<NetworkBaseController> Bv =  PassIfHasSuccessor();
            if (Bv is BvBePurifying)
            {
                actor.BaseAnimator.SetBool("IsCaught", false);
                return Bv;
            }

            return null;
        }
        /*--- Private Methods ---*/
    }
}