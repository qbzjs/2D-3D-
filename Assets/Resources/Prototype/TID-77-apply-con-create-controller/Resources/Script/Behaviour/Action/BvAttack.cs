using KSH_Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib.Data;
using UnityEngine.UI;
using LSH_Lib;
namespace GHJ_Lib
{
	public class BvAttack: Behavior<NetworkBaseController>
    {
        bool isBishopPassive = false;
        const float BishopPassiveRate = 0.15f;
        //bishiop
        float delayTime = 0.36f;
        float attackTime = 0.461f;
        //hunter
        float firstDelayTime = 0.228f;
        float firstAttackTime = 0.263f;
        float secondDelayTime = 0.482f;
        float secondAttackTime = 0.513f;
        float attackEndTime = 0.9f;
        AttackArea attackArea;
        Image[] bloodImages;
        ExorcistController exorcistController;

        protected override void Activate(in NetworkBaseController actor)
        {
            if ( actor.IsMine )
            {
                DataManager.Instance.ShareBehavior( (int)NetworkBaseController.BehaviorType.Attack );
            }

            exorcistController = (actor as ExorcistController);
            exorcistController.weaponTrail.enabled = true;
            PlayAnimation( actor );
            AudioManager.instance.Play("BishopAttack");
            if (actor.skill is BishopSkill)
            {
                isBishopPassive = true;
            }
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.CamForward);
            attackArea = exorcistController.attackArea;
        }
        
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            AnimatorStateInfo animatorStateInfo = actor.BaseAnimator.GetCurrentAnimatorStateInfo(0);
            if (actor.BaseAnimator.GetBool("IsAttack")&& animatorStateInfo.IsName("Attack"))
            {
                if (IsCanGetTargetAfterDelay(actor,animatorStateInfo))
                {
                    GameObject target = attackArea.GetNearestTarget();
                    DollController doll = target.GetComponent<DollController>();
                    doll.DoActionBy(Attack);
                    doll.ChangeBvToGetHit();
                    actor.BaseAnimator.SetBool("IsAttack", false);
                   
                }
                else if (animatorStateInfo.normalizedTime >= attackEndTime)
                {
                    actor.BaseAnimator.SetBool("IsAttack", false);
                }
            }
            else
            {
                if (actor.BaseAnimator.GetBool("IsAttack"))
                {
                    return null;
                }
                if (animatorStateInfo.IsName("Idle"))
                {
                    exorcistController.weaponTrail.enabled = false;
                    actor.BaseAnimator.SetBool("IsAttack", false);
                    return new BvIdle();
                }
            }
            return null;
        }

        void PlayAnimation( in NetworkBaseController actor )
        {
            actor.BaseAnimator.SetBool("IsAttack", true);
        }

        public virtual void Attack(DollData targetData)
        {
            AudioManager.instance.Play("BishopHit");
            targetData.DollHP -= (DataManager.Instance.PlayerDatas[0].roleData as ExorcistData).AttackPower;
            if (isBishopPassive)
            {
                targetData.DevilHP -= (DataManager.Instance.PlayerDatas[0].roleData as ExorcistData).AttackPower* BishopPassiveRate;
            }
        }

        bool IsCanGetTargetAfterDelay(NetworkBaseController actor, AnimatorStateInfo animatorStateInfo)
        {
            switch (actor.TypeIndex)
            {
                case 1:
                    {
                        return attackArea.CanGetTarget() && animatorStateInfo.normalizedTime >= delayTime && animatorStateInfo.normalizedTime >= attackTime;
                    }
                    break;
                case 4:
                    {
                        return attackArea.CanGetTarget() && ((animatorStateInfo.normalizedTime >= firstDelayTime && animatorStateInfo.normalizedTime <= firstAttackTime)
                            ||(animatorStateInfo.normalizedTime >= secondDelayTime && animatorStateInfo.normalizedTime <= secondAttackTime));
                    }
                    break;
            }
            return false;
        }


    }
}