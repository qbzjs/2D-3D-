using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class RabbitActiveSkillArea: EffectArea
	{
        protected override GameObject FindTargets(Collider other)
        {
            if (other.CompareTag(GameManager.DollTag))
            {
                return other.gameObject;
            }
            return null;
        }
    }
}