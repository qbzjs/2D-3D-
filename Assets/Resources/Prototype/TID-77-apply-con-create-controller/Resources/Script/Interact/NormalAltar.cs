using KSH_Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class NormalAltar: GaugedObject
	{
		protected bool isEnable = true;

        protected override void OnEnable()
		{
			castingSystem.ResetCasting();
		}

        protected override void DoResult()
        {
            isEnable = false;
            StageManager.Instance.DecreaseAltarCount();
        }

        protected override bool ResultCondition()
        {
            return isEnable && castingSystem.IsFinshCasting;
        }

        protected override void TryInteract()
        {
            throw new System.NotImplementedException();
        }
    }
}