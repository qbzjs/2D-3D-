using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Object;
namespace GHJ_Lib
{
    public class BishopActiveSkillArea : EffectArea
    {
        protected override GameObject FindTargets(Collider other)
        {
            if (other.CompareTag(GameManager.CollectTriggerTag))
            {
                return other.gameObject;
            }
            return null;
        }

    }
}