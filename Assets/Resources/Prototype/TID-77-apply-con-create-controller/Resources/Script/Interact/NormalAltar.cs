using KSH_Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class NormalAltar: InteractionObj
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/
		protected bool isEnable = true;

		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/
		void OnEnable()
		{
			curGauge = 0.0f;
			
		}
		void Update()
		{
			Debug.Log("Normal CurGauge :" + curGauge);
			if (GetGaugeRate >= 1.0f && isEnable)
			{
				isEnable = false;

				StageManager.Instance.DecreaseAltarCount();
				
			}
			
		}




		/*--- Public Methods ---*/


		public override CastingType GetCastingType(NetworkBaseController player)
		{
			if (player is DollController)
			{
				if (GetGaugeRate < 1.0f)
				{
					return CastingType.ManualCasting;
				}
				
			}

			if (player is ExorcistController)
			{
				if (GetGaugeRate > 0.3f)
				{
					return CastingType.LocalAutoCasting;
				}
				
			}

			
			return CastingType.NotCasting;
		}

		


		/*--- Private Methods ---*/
		void CheckGauge()
		{
			if (GetGaugeRate >= 1.0f&& CanActiveToDoll)
			{
				StageManager.Instance.DecreaseAltarCount();
			}
		}


		
    }
}