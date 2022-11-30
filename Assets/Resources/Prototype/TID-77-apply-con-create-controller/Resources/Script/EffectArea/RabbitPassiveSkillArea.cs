using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib.Data;
using KSH_Lib;
namespace GHJ_Lib
{
    public class RabbitPassiveSkillArea : EffectArea
    {
        public DollController rabbit;
        enum BuffCondition {None ,Approach , Ready, Running }
        BuffCondition buffCondition = BuffCondition.None;
        
        //bool isOnBuff = false;
        protected override GameObject FindTargets(Collider other)
        {
            if (other.CompareTag(GameManager.ExorcistTag))
            {
                return other.gameObject;
            }
            return null;
        }

        public bool CanMoveInHide()
        {
            if (CanGetTarget())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (!rabbit.photonView.IsMine)
            {
                return;
            }

                var target = FindTargets(other);
            if (target == null)
            {
                return;
            }
            if (!targets.Contains(target))
            {
                if(rabbit.CurBehavior is BvHide)
                {
                    StartCoroutine(ReadyToMoveInHide());
                }
                targets.Add(target);
            }
        }
        protected override void OnTriggerExit(Collider other)
        {
            if (targets.Remove(FindTargets(other)))
            {
                if (rabbit.photonView.IsMine)
                {
                    if (buffCondition == BuffCondition.Approach)
                    {
                        buffCondition = BuffCondition.None;
                    }
                    else if (buffCondition == BuffCondition.Ready)
                    { 
                        StartCoroutine(OnBuff(5.0f));
                    }
                }
            }
        }
        IEnumerator ReadyToMoveInHide()
        {
            float startTime = Time.time;
            buffCondition = BuffCondition.Approach; 
            while (Time.time - startTime < 1.0f)
            {
                yield return new WaitForEndOfFrame(); // 문서에 시간: 1초
                if (buffCondition != BuffCondition.Approach)
                {
                    yield break;
                }
            }
            buffCondition = BuffCondition.Ready;
        }

        IEnumerator OnBuff(float time) //문서에 버프시간이 5초
        {
            if (buffCondition!=BuffCondition.Ready)
            {
                yield break;
            }
            buffCondition = BuffCondition.Running;
            rabbit.ChangeMoveSpeed(1.5f);
            yield return new WaitForSeconds(time);
            buffCondition = BuffCondition.None;
            rabbit.ChangeMoveSpeed(1.0f);
        }
    }
}