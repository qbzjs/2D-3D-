using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
	public class BvHit: Behavior<BasePlayerController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/
        protected float damage;

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/
        public void SetDamage(float damage)
        {
            this.damage = damage;
        }

        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            
            actor.BaseAnimator.Play("Hit");
            (DataManager.Instance.LocalPlayerData.roleData as DollData).DollHP -= damage;
            DataManager.Instance.ShareRoleData();
        }

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            
            if((DataManager.Instance.LocalPlayerData.roleData as DollData).DollHP<0.0f)
            {
                return new BvDown();
            }    
            
            if (actor.BaseAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
            {
                return new BvIdle();
            }
            return null;
        }

        /*--- Private Methods ---*/
    }
}