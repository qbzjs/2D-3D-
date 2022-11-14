using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;

namespace GHJ_Lib
{
	public class Sk_Detected: Behavior<NetworkBaseController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/
        protected EffectArea effectArea;

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/

        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.BaseAnimator.Play("Skill");
            effectArea = actor.skill.actSkillArea;
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            (actor.skill as WolfSkill).DoWolfActiveSkillTo_RPC();
            return PassIfHasSuccessor();

        }

        /*--- Private Methods ---*/




    }
}