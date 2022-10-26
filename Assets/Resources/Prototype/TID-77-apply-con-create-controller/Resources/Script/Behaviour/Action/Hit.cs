using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class Hit: Behavior<BasePlayerController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            
            actor.BaseAnimator.Play("Hit");
            //HP ´ÞÀ½        

        }

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            /*
            if(HP<=0.0f)
            {
                return new Down();
            }    
            */
            if (actor.BaseAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
               
                return new Idle();
            }
            return null;
        }

        /*--- Private Methods ---*/
    }
}