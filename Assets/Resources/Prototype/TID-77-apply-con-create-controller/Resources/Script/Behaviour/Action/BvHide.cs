using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;

namespace GHJ_Lib
{
	public class BvHide: Behavior<NetworkBaseController>
	{
        bool hide = false;
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.ChangeMoveFunc(false);
            hide = false;
            if (actor.photonView.IsMine)
            { 
                actor.StartCoroutine("Hide");
            }
            actor.ChangeMoveFunc(false);
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if (!hide)
            {
                return null;
            }

            //if()외부의 조건
            // 이동, 상호작용,아이템사용, 퇴마사로부터 피격 
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
            hide = isHide;
        }
    }
}