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
                actor.StartCoroutine("Hide");
            }
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {


            //if()�ܺ��� ����
            // �̵�, ��ȣ�ۿ�,�����ۻ��, �𸶻�κ��� �ǰ� 
            if (actor.photonView.IsMine)
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    actor.StartCoroutine("UnHide");
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