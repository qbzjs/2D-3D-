using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvHunterActSkill: Behavior<NetworkBaseController>
	{
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.BaseAnimator.SetBool("IsInstallTrap",true); 
            (actor.skill as HunterSkill).StartCoroutine("ExcuteActiveSkill");
            actor.ChangeMoveFunc(false);
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (Bv is BvIdle)
            {
                actor.BaseAnimator.SetBool("IsInstallTrap", false);
                return Bv;
            }
            return null;
        }
    }
}