using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public class WolfActiveSkillArea : EffectArea
    {
        public GameObject GetExorcist()
        {
            if (CanGetTarget())
            {
                return Targets[0];
            }
            return null;
            
        }
        protected override GameObject FindTargets(Collider other)
        {
            if (other.gameObject.CompareTag(GameManager.ExorcistTag))
            {
                return other.gameObject;
            }
            return null;
        }
    }
}