using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;

namespace GHJ_Lib
{
	public class BvRabbitActSkill: Behavior<NetworkBaseController>
	{
        protected override void Activate(in NetworkBaseController actor)
        {
            (actor.skill as RabbitSkill).StartCoroutine("ExcuteActiveSkil");
            actor.BaseAnimator.SetBool("IsSkill",true);
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (Bv is BvIdle||Bv is BvGetHit)
            {
                actor.BaseAnimator.SetBool("IsSkill", false);
                return Bv;
            }
            return null;
        }
    }
}