using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
	public class BvGetHit: Behavior<NetworkBaseController>
	{
        const float AnimationFinishPoint = 0.9f;
        const float CrossStackBonusRate = 0.2f;
        protected override void Activate(in NetworkBaseController actor)
        {
            if ( actor.IsMine )
            {
                DataManager.Instance.ShareBehavior( (int)NetworkBaseController.BehaviorType.GetHit );
            }
            actor.BaseAnimator.Play("Hit");

            if (actor.photonView.IsMine)
            {
                if ((actor as DollController).CrossStack >= 2)
                {
                    (DataManager.Instance.LocalPlayerData.roleData as DollData).DollHP -= (DataManager.Instance.PlayerDatas[0].roleData as ExorcistData).AttackPower * CrossStackBonusRate;
                }
            }
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.Input);
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if ((DataManager.Instance.PlayerDatas[actor.PlayerIndex].roleData as DollData).DollHP < 0.0f)
            {
                return new BvCollapse();
            }

            AnimatorStateInfo animatorStateInfo = actor.BaseAnimator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsName("Idle_A"))
            {
                return new BvIdle();
            }
            return null;
        }

        /*--- Private Methods ---*/
    }
}