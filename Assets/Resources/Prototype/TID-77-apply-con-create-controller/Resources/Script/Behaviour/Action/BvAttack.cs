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
        float attackTime = 0.5f;
        protected override void Activate(in NetworkBaseController actor)
        {
            PlayAnimation( actor );
            if (actor is BishopSkill)
            {
                BishopPassive = true;
            }
            actor.ChangeMoveFunc(false);
        }
        
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            AnimatorStateInfo animatorStateInfo = actor.BaseAnimator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.normalizedTime >= attackTime && actor.BaseAnimator.GetBool("IsAttack")) 
            {
                AttackArea attackArea = (actor as ExorcistController).attackArea;
                if (attackArea.CanGetTarget())
                {
                    GameObject target = attackArea.GetNearestTarget();
                    DollController doll = target.GetComponent<DollController>();
                    doll.DoActionBy(Attack);
                    doll.ChangeBvToGetHit();
                }
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