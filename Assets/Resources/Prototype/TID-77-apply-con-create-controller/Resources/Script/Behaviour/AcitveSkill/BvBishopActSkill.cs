using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;

namespace GHJ_Lib
{
	public class BvBishopActSkill: Behavior<NetworkBaseController>
	{
        protected override void Activate(in NetworkBaseController actor)
        {
            (actor.skill as BishopSkill).StartCoroutine("ExcuteActiveSkill");
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if (!actor.BaseAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                return null;
            }

            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (Bv is BvIdle)
            {
                return Bv;
            }
            return null;
        }
    }
}