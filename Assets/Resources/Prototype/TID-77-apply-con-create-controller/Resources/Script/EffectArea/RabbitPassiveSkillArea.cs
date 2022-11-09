using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib.Data;
using KSH_Lib;
namespace GHJ_Lib
{
    public class RabbitPassiveSkillArea : EffectArea
    {
        public RabbitSkill rabbit;
        bool isReadyToBuff = false;
        bool isWaitToBuff = false;
        bool isOnBuff = false;
        IEnumerator buffCoroutine;
        private void OnEnable()
        {
            buffCoroutine = OnBuff(5.0f);
        }
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
                if(rabbit.Controller.CurBehavior is BvHide)
                {
                    StartCoroutine("ReadyToMoveInHide");
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
                    isWaitToBuff = false;
                    StartCoroutine(buffCoroutine);
                }
            }
        }
        IEnumerator ReadyToMoveInHide()
        {
            float startTime = Time.time;
            isWaitToBuff = true;
            while (Time.time - startTime < 1.0f)
            {
                yield return new WaitForEndOfFrame(); // 문서에 시간: 1초
                if (!isWaitToBuff)
                {
                    yield break;
                }
            }
            isReadyToBuff = true;
        }

        IEnumerator OnBuff(float time) //문서에 버프시간이 5초
        {
            if (!isReadyToBuff)
            {
                yield break;
            }
            isReadyToBuff = false;
            isOnBuff = true;
            rabbit.Controller.ChangeMoveSpeed(1.5f);
            yield return new WaitForSeconds(time);
            isOnBuff = false;
            rabbit.Controller.ChangeMoveSpeed(1.0f);
        }
    }
}