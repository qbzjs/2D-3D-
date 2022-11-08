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
            //actor.BaseAnimator.SetBool(); 수집 또는 성치 애니매이션 
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            //끝나면 
            return null;
        }
    }
}