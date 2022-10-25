using KSH_Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class NormalAltar: Interaction
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/
		void OnEnable()
		{
			curGauge = 0.0f;
			CanActiveToExorcist = false;
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
				BarUI.Instance.SetTarget(this);
				Casting(controller);
			}
			if (controller is ExorcistController)
			{
				BarUI.Instance.SetTarget(null);
				AutoCasting(controller);
			}
        }

        /*--- Protected Methods ---*/
        protected override void Casting(BasePlayerController controller)
        {
			//controller���� PlayerData �� ȣ���ϰ� interact Velocity�� ����.	
			float velocity=10.0f;
			curGauge += velocity*Time.deltaTime;
		}
        protected override void AutoCasting(BasePlayerController controller)
        {
			//controller���� PlayerData �� ȣ���ϰ� interact Velocity�� ����.	
			float velocity = 10.0f;
			BarUI.Instance.AutoCastingNull(maxGauge / velocity);

		}


		/*--- Private Methods ---*/
		void CheckGauge()
		{
			if (GetGaugeRate >= 1.0f)
			{
				//FinalAltar �θ���
				CanActiveToExorcist = false;
				CanActiveToDoll = false;
			}
			else
			{
				if (GetGaugeRate >= 0.3f)
				{
					CanActiveToExorcist = true;
				}
				else
				{
					CanActiveToExorcist = false;
				}
			}

		}
    }
}