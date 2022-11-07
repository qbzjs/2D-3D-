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
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.BaseAnimator.Play("Hit");

            if (actor.photonView.IsMine)
            {
                // 피격 구현하기
            }
            actor.ChangeMoveFunc(true);
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if ((DataManager.Instance.PlayerDatas[actor.PlayerIndex].roleData as DollData).DollHP < 0.0f)
            {
                return new BvCollapse();
            }

            AnimatorStateInfo animatorStateInfo = actor.BaseAnimator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.normalizedTime.Equals(1.0f) )
            {
                return new BvIdle();
            }
            return null;
        }

        /*--- Private Methods ---*/
    }
}