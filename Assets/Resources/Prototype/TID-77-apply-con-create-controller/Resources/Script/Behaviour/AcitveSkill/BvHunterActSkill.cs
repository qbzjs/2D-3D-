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
            //actor.BaseAnimator.SetBool(); ���� �Ǵ� ��ġ �ִϸ��̼� 
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            //������ 
            return null;
        }
    }
}