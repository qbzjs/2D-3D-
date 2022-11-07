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
            //토끼 패시브 넣을곳
            actor.StartCoroutine("Hide");
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if (!hide)
            {
                return null;
            }
            DoUnHide(actor);

            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (Bv is BvIdle)
            {
                return Bv;
            }
            return null; 
        }

        void DoUnHide(in NetworkBaseController actor)
        {
            if (!Input.GetKeyDown(KeyCode.B))
            {
                actor.StartCoroutine("UnHide");
            }
        }

        public void CompleteHide(bool isHide)
        {
            hide = isHide;
        }
    }
}