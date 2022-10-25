using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class ExitAltar: Interaction
	{
		/*--- Public Fields ---*/
		

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/
		void OnEnable()
		{
			curGauge = 0.0f;
			CanActiveToExorcist = true;
			CanActiveToDoll = true;
		}
		void Update()
		{
			CheckGauge();
		}

		/*--- Public Methods ---*/
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
			BarUI.Instance.AutoCastingNull(maxGauge / velocity);
			//Doll 이라면 끝날때 탈출을 넣어주기
		}
		/*--- Private Methods ---*/
		void CheckGauge()
		{
			if (GetGaugeRate >= 1.0f)
			{
				CanActiveToExorcist = false;
				CanActiveToDoll = false;
			}
			else
			{
				CanActiveToExorcist = true;
				CanActiveToExorcist = true;
			}

		}
	}
}