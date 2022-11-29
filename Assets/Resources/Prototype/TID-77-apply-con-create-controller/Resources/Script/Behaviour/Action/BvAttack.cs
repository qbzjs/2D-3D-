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
        float bishopDelayTime=0.358f;
        float bishopAttackTime = 0.463f;
        float firstHunterDelayTime=0.232f;
        float firstHunterAttackTime=0.287f;
        float secondHunterDelayTime=0.492f;
        float secondHunterAttackTime=0.523f;

        float attackTime = 0.9f;
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
            //AudioManager.instance.Play("BishopAttack");
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
                if (CanHitDoll(actor,animatorStateInfo))
                {
                    GameObject target = attackArea.GetNearestTarget();
                    DollController doll = target.GetComponent<DollController>();
                    doll.DoActionBy(Attack);
                    doll.ChangeBvToGetHit();
                    actor.BaseAnimator.SetBool("IsAttack", false);
                }
                else if (animatorStateInfo.normalizedTime >= attackTime)
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
            //AudioManager.instance.Play("BishopHit");
            targetData.DollHP -= (DataManager.Instance.PlayerDatas[0].roleData as ExorcistData).AttackPower;
            if (isBishopPassive)
            {
                targetData.DevilHP -= (DataManager.Instance.PlayerDatas[0].roleData as ExorcistData).AttackPower* BishopPassiveRate;
            }
        }

        bool CanHitDoll(NetworkBaseController actor,AnimatorStateInfo animatorStateInfo)
        {
            switch (actor.TypeIndex)
            {
                case 1:
                    {
                        return attackArea.CanGetTarget() && (animatorStateInfo.normalizedTime >= bishopDelayTime && animatorStateInfo.normalizedTime <= bishopAttackTime);
                    }
                case 4:
                    {
                        return attackArea.CanGetTarget() && (
                            (animatorStateInfo.normalizedTime >= firstHunterDelayTime && animatorStateInfo.normalizedTime <= firstHunterAttackTime)
                            || (animatorStateInfo.normalizedTime >= secondHunterDelayTime && animatorStateInfo.normalizedTime <= secondHunterAttackTime));
                    }
                default:
                    {
                        Debug.LogError("Player Type Error");
                        return false;
                    }
            }
            
        }
    }
}