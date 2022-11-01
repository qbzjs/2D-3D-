using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class ExitAltar: InteractionObj
	{
		/*--- Public Fields ---*/
		public GameObject ExitAltarModel;

		/*--- Protected Fields ---*/
		protected bool isOpen = false;

		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/
		void OnEnable()
		{
			StageManager.Instance.SetAltar(this);
			ExitAltarModel.SetActive(false);
			curGauge = 0.0f;
		}
		void Update()
		{
			if (isOpen)
			{ 
				CheckGauge();
			}
		}

		/*--- Public Methods ---*/

		public override CastingType GetCastingType(NetworkBaseController player)
		{
			if (!isOpen)
			{
				return CastingType.NotCasting;
			}

			if (player is DollController)
			{

				return CastingType.LocalAutoCasting;

			}

			if (player is ExorcistController)
			{
				if (GetGaugeRate >= 1.0f)
				{
					return CastingType.ManualCasting;
				}
				return CastingType.SharedAutoCasting;
			}
			

			return CastingType.NotCasting;
		}

		public void OpenExitAltar()
		{
			isOpen = true;
			ExitAltarModel.SetActive(true);
		}
		/*--- Protected Methods ---*/

		/*--- Private Methods ---*/
		void CheckGauge()
		{
			if (GetGaugeRate >= 1.0f)
			{
				isOpen = false;
			}
		

		}

	
	}
}