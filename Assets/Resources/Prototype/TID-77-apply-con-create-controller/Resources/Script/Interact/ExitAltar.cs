using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class ExitAltar: Interaction
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

		public override CastingType GetCastingType(BasePlayerController player)
		{
			if (player is DollController)
			{
				return CastingType.AutoCastingNull;
			}

			if (player is ExorcistController)
			{
				return CastingType.AutoCasting;
			}

			Debug.LogError("Error get Casting Type");
			return CastingType.Casting;
		}

		public override void Interact(BasePlayerController controller)
		{
			if (controller is DollController)
			{
				BarUI.Instance.SetTarget(null);
				AutoCasting(controller);
			}
			if (controller is ExorcistController)
			{
				BarUI.Instance.SetTarget(this);
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