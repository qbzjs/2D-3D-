using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
	public class BvCatch: Behavior<NetworkBaseController>
	{
        //float pickUpTime;
        protected override void Activate(in NetworkBaseController actor)
        {
            if ( actor.IsMine )
            {
                DataManager.Instance.ShareBehavior( (int)NetworkBaseController.BehaviorType.Catch );
            }
            actor.BaseAnimator.SetBool("IsCatch", true);
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.Stop);

            //switch (DataManager.Instance.GetLocalRoleType)
            //{
            //    case RoleData.RoleType.Bishop:
            //        {
            //            pickUpTime = 0.30f;
            //        }
            //        break;
            //    case RoleData.RoleType.Hunter:
            //        {
            //            pickUpTime = 0.30f;
            //        }
            //        break;
            //}
        }
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            AnimatorStateInfo animatorStateInfo = actor.BaseAnimator.GetCurrentAnimatorStateInfo(1);

            //if (animatorStateInfo.normalizedTime >= pickUpTime && animatorStateInfo.IsName("Pickup") && actor.BaseAnimator.GetBool("IsCatch"))
            ExorcistController exorcistActor = actor as ExorcistController;
            if (exorcistActor.IsPickupDoll&& exorcistActor.pickUpArea.CanGetTarget())
            {
                exorcistActor.PickUp();
                actor.BaseAnimator.SetBool("IsCatch", false);
                actor.ChangeMoveFunc(NetworkBaseController.MoveType.Input);
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