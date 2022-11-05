using KSH_Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib.Data;

namespace GHJ_Lib
{
	public class BvAttack: Behavior<NetworkBaseController>
    {
        const float animationEndPoint = 1.0f;
        
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
            Behavior<NetworkBaseController> Bv = PassIfHasSuccessor();
            if (Bv is BvIdle)
            {
                actor.BaseAnimator.CrossFade("Attack", 0.5f);
                return Bv;
            }
            return null;
        }

        void PlayAnimation( in NetworkBaseController actor )
        {
            actor.BaseAnimator.CrossFade( "Attack" ,0.3f);
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