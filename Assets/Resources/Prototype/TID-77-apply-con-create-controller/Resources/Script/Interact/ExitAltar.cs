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
			ExitAltarModel.SetActive(false);
			curGauge = 0.0f;
			CanActiveToExorcist = false;
			CanActiveToDoll = false;

			OpenExitAltar(); //디버그를 위해 
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

			Debug.LogError("Error get Casting Type");
			return CastingType.NotCasting;
		}

		public override void Interact(BasePlayerController controller)
		{
			if (controller is DollController)
			{
				BarUI_Controller.Instance.SetTarget(null);
				AutoCasting(controller);
			}
			if (controller is ExorcistController)
			{
				BarUI_Controller.Instance.SetTarget(this);
				AutoCasting(controller);
			}
		}
		public void OpenExitAltar()
		{
			isOpen = true;
			CanActiveToExorcist = true;
			CanActiveToDoll = true;
			ExitAltarModel.SetActive(true);
		}
		/*--- Protected Methods ---*/
		protected override void Casting(BasePlayerController controller)
		{
			//controller에서 PlayerData 를 호출하고 interact Velocity를 받은걸 사용.	
			float velocity = 10.0f;
			curGauge += velocity * Time.deltaTime;
		}
		protected override void AutoCasting(BasePlayerController controller)
		{
			//controller에서 PlayerData 를 호출하고 interact Velocity를 받은걸 사용.	
			float velocity = 10.0f;
			//Doll 이라면 끝날때 탈출을 넣어주기
		}
		/*--- Private Methods ---*/
		void CheckGauge()
		{
			if (GetGaugeRate >= 1.0f)
			{
				CanActiveToExorcist = false;
				CanActiveToDoll = false;
				isOpen = false;
			}
		

		}

	
	}
}