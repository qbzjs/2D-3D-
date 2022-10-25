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
		}
		void Update()
		{
			if (isOpen)
			{ 
				CheckGauge();
			}
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
			//controller���� PlayerData �� ȣ���ϰ� interact Velocity�� ������ ���.	
			float velocity = 10.0f;
			curGauge += velocity * Time.deltaTime;
		}
		protected override void AutoCasting(BasePlayerController controller)
		{
			//controller���� PlayerData �� ȣ���ϰ� interact Velocity�� ������ ���.	
			float velocity = 10.0f;
			BarUI.Instance.AutoCastingNull(maxGauge / velocity);
			//Doll �̶�� ������ Ż���� �־��ֱ�
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