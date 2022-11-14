using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;

namespace GHJ_Lib
{
	public class BvHide: Behavior<NetworkBaseController>
	{
        bool ishide = false;
        protected override void Activate(in NetworkBaseController actor)
        {
            StageManager.Instance.dollUI.CommomSkill.StartCountDown(1.0f);
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.Stop);
            ishide = false;
            if (actor.photonView.IsMine)
            { 
                actor.StartCoroutine("Hide");
            }
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if (!ishide)
            {
                return null;
            }

            //if()�ܺ��� ����
            // �̵�, ��ȣ�ۿ�,�����ۻ��, �𸶻�κ��� �ǰ� 
            if (actor.photonView.IsMine)
            {
                DoUnHide(actor);
            }

            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (Bv is not null)
            {
                if (actor.photonView.IsMine)
                {
                    actor.StartCoroutine("UnHide");
                }
                return Bv;
            }
            return null; 
        }

        private void DoUnHide(in NetworkBaseController actor)
        {
            if (Input.anyKey)
            {
                actor.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
            }
        }
        public void CompleteHide(bool isHide)
        {
            ishide = isHide;
        }
    }
}