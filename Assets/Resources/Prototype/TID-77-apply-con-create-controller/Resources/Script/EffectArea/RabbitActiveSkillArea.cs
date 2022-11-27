using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib.Data;
using KSH_Lib;
namespace GHJ_Lib
{
	public class RabbitActiveSkillArea: EffectArea
	{
        protected override GameObject FindTargets(Collider other)
        {
            if (other.CompareTag(GameManager.HealingTriggerTag))
            {
                Debug.Log($"Collider Name : {other.gameObject.name}");
                return other.transform.parent.parent.gameObject;
            }
            return null;
        }
    }
}