using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
using LSH_Lib;
namespace GHJ_Lib
{
	public class BvHide: Behavior<NetworkBaseController>
	{
        DollController dollActor;

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

            dollActor = actor as DollController;
            dollActor.ghostOutEffect.Clear();
            dollActor.ghostOutEffect.Play();

            //AudioManager.instance.Play("DollHide", AudioManager.PlayTarget.Doll);

            actor.BaseAnimator.SetBool("IsHide", true);

            
            actor.StartCoroutine(actor.InnerCoolTime(0.3f));

            //if (actor.photonView.IsMine)
            //{
            //    // >> Changed By KSH 22.11.26
            //    //actor.StartCoroutine("Hide");
            //    actor.BaseAnimator.SetBool( "IsHide", true );
            //}
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            //if()외부의 조건
            // 이동, 상호작용,아이템사용, 퇴마사로부터 피격 
            
            if (actor.photonView.IsMine)
            {
                if (!actor.IshideInnerCoolTime&&Input.GetKeyDown(KeyCode.B))
                {
                    actor.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
                }
            }
            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (Bv is BvIdle||Bv is BvGetHit)
            {
                // >> Changed By KSH 22.11.26
                dollActor.ghostInEffect.Clear();
                dollActor.ghostInEffect.Play();
                actor.BaseAnimator.SetBool("IsHide", false);
                actor.StartCoroutine("UnHide");
                actor.StartCoroutine(actor.InnerCoolTime(0.3f));
                return Bv;
            }
            return null; 
        }

    }
}