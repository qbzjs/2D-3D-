using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class RabbitPassiveSkillArea : EffectArea
    {
        public RabbitController rabbit;
        bool canMoveInHide = false;
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
            var target = FindTargets(other);
            if (target == null)
            {
                return;
            }
            if (!targets.Contains(target))
            {
                rabbit.ChangeMoveFunc(true);
                targets.Add(target);
            }
        }
        protected override void OnTriggerExit(Collider other)
        {
            if (targets.Remove(FindTargets(other)))
            {
                //move Coroutine
            }
        }
        /*
        IEnumerator MoveInHide(float time)
        {
            if (!canMoveInHide)
            {
                canMoveInHide = true;
            }

            while (true)
            {
                yield return
            }

      
        }
        */
    }
}