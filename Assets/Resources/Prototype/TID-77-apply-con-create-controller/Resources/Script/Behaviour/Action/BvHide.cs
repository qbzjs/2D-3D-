using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;

namespace GHJ_Lib
{
	public class BvHide: Behavior<NetworkBaseController>
	{
        protected override void Activate(in NetworkBaseController actor)
        {
            if ( actor.IsMine )
            {
                DataManager.Instance.ShareBehavior( (int)NetworkBaseController.BehaviorType.Hide );
            }

            if (actor.IsMine)
            { 
                StageManager.Instance.dollUI.CommomSkill.StartCountDown(1.0f);
            }
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.StopRotation);

            if (actor.photonView.IsMine)
            {
                // >> Changed By KSH 22.11.26
                //actor.StartCoroutine("Hide");
                actor.BaseAnimator.SetBool( "IsHide", true );
            }
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {


            //if()외부의 조건
            // 이동, 상호작용,아이템사용, 퇴마사로부터 피격 
            if (actor.photonView.IsMine)
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    // >> Changed By KSH 22.11.26
                    //actor.StartCoroutine("UnHide");
                    actor.BaseAnimator.SetBool( "IsHide", false );
                }
            }
            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (Bv is BvIdle||Bv is BvGetHit)
            { 
                return Bv;
            }
            return null; 
        }

    }
}