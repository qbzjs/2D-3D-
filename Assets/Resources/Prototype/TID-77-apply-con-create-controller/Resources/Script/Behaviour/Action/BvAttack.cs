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
        float attackTime = 0.9f;
        AttackArea attackArea;
        Image[] bloodImages;
        protected override void Activate(in NetworkBaseController actor)
        {
            if ( actor.IsMine )
            {
                DataManager.Instance.ShareBehavior( (int)NetworkBaseController.BehaviorType.Attack );
            }
            PlayAnimation( actor );
            AudioManager.instance.Play("BishopAttack");
            if (actor.skill is BishopSkill)
            {
                isBishopPassive = true;
            }
            actor.ChangeMoveFunc(NetworkBaseController.MoveType.CamForward);
            attackArea = (actor as ExorcistController).attackArea;
        }
        
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            AnimatorStateInfo animatorStateInfo = actor.BaseAnimator.GetCurrentAnimatorStateInfo(0);
            if (actor.BaseAnimator.GetBool("IsAttack")&& animatorStateInfo.IsName("Attack"))
            {
                if (attackArea.CanGetTarget())
                {
                    GameObject target = attackArea.GetNearestTarget();
                    DollController doll = target.GetComponent<DollController>();
                    doll.DoActionBy(Attack);
                    doll.ChangeBvToGetHit();
                    actor.BaseAnimator.SetBool("IsAttack", false);
                    if (actor.IsMine)
                    {
                        if (bloodImages == null)
                        { 
                            bloodImages  = (actor as ExorcistController).BloodImages;
                        }
                        EffectManager.Instance.RandomShowImageOnScreen(bloodImages[Random.Range(0, bloodImages.Length-1)]);
                    }
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
    }
}