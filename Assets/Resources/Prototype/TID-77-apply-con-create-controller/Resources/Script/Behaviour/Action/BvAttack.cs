using KSH_Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib.Data;

namespace GHJ_Lib
{
	public class BvAttack: Behavior<NetworkBaseController>
    {
        bool BishopPassive = false;
        const float BishopPassiveRate = 0.15f;
        protected override void Activate(in NetworkBaseController actor)
        {
            PlayAnimation( actor );
            AttackArea attackArea = (actor as ExorcistController).attackArea;
            if (attackArea.CanGetTarget())
            {
                GameObject target = attackArea.GetNearestTarget();
                DollController doll = target.GetComponent<DollController>();
                doll.DoActionBy(Attack);
                doll.ChangeBvToGetHit();
                
            }
            if (actor is BishopController)
            {
                BishopPassive = true;
            }
            actor.ChangeMoveFunc(false);
        }
        
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            AnimatorStateInfo animatorStateInfo = actor.BaseAnimator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.normalizedTime >= 0.5f)
            {
                actor.BaseAnimator.SetBool("IsAttack", false);
            }

            if(animatorStateInfo.IsName("Idle"))
            {
                actor.BaseAnimator.SetBool("IsAttack", false);
                return new BvIdle();
            }
            return null;
        }

        void PlayAnimation( in NetworkBaseController actor )
        {
            actor.BaseAnimator.SetBool("IsAttack", true);
        }

        public virtual void Attack(DollData targetData)
        {
            targetData.DollHP -= (DataManager.Instance.PlayerDatas[0].roleData as ExorcistData).AttackPower;
            if (BishopPassive)
            {
                targetData.DevilHP -= (DataManager.Instance.PlayerDatas[0].roleData as ExorcistData).AttackPower* BishopPassiveRate;
            }
        }
    }
}