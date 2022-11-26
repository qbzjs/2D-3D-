using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
	public class AttackArea: EffectArea
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