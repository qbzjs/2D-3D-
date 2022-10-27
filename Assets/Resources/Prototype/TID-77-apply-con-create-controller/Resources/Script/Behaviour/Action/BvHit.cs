using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
	public class BvHit: Behavior<BasePlayerController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/
        protected int playerIdx;

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/
        public void SetPlayerIdx(int idx)
        {
            playerIdx = idx;
        }

        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            
            actor.BaseAnimator.Play("Hit");
            if (actor.photonView.IsMine)
            { 
                (DataManager.Instance.PlayerDatas[playerIdx].roleData as DollData).DollHP -= (DataManager.Instance.PlayerDatas[0].roleData as ExorcistData).AttackPower;
                DataManager.Instance.ShareRoleData();
            }
        }

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            //PassIfHasSuccessor();
            if ((DataManager.Instance.PlayerDatas[playerIdx].roleData as DollData).DollHP<0.0f)
            {
                return new BvDown();
            }    
            
            if (actor.BaseAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
                return new BvIdle();
            }
            return null;
        }

        /*--- Private Methods ---*/
    }
}