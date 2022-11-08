using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class RabbitPassiveSkillArea : EffectArea
    {
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
    }
}