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
            AudioManager.instance.Play("DollHide", AudioManager.PlayTarget.Doll);

            actor.BaseAnimator.SetBool("IsHide", true);
            
            //if (actor.photonView.IsMine)
            //{
            //    // >> Changed By KSH 22.11.26
            //    //actor.StartCoroutine("Hide");
            //    actor.BaseAnimator.SetBool( "IsHide", true );
            //}
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            //if()�ܺ��� ����
            // �̵�, ��ȣ�ۿ�,�����ۻ��, �𸶻�κ��� �ǰ� 
            
            if (actor.photonView.IsMine)
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    actor.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
                }
            }
            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (Bv is BvIdle||Bv is BvGetHit)
            {
                // >> Changed By KSH 22.11.26
                actor.BaseAnimator.SetBool("IsHide", false);
                actor.StartCoroutine("UnHide");
                return Bv;
            }
            return null; 
        }

    }
}