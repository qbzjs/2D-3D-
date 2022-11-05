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
            actor.BaseAnimator.Play("install Cross");
            (actor as BishopController).StartCoroutine("ExcuteActiveSkil");
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if (actor.BaseAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
            {
                return new BvIdle();    
            }
            return null;
        }
        

    }
}