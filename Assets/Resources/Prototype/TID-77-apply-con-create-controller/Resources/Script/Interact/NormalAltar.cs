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
			if (GetGaugeRate >= 1.0f && isEnable)
			{
				isEnable = false;
				FinalAltar.Instance.DisableNormalAltar();
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






		
		public override void Interact(BasePlayerController controller)
        {
			if (controller is DollController)
			{
				Casting(controller);
			}

			if (controller is ExorcistController)
			{
				AutoCasting(controller);
			}
        }


        /*--- Protected Methods ---*/
        protected override void Casting(BasePlayerController controller)
        {
			//controller에서 PlayerData 를 호출하고 interact Velocity를 받음.	
			float velocity=5.0f;
			curGauge += velocity*Time.deltaTime;
			BarUI_Controller.Instance.SetTarget(this);
			BarUI_Controller.Instance.UpdateValue();
		}
        protected override void AutoCasting(BasePlayerController controller)
        {
			//controller에서 PlayerData 를 호출하고 interact Velocity를 받음.	
			float velocity = 1.0f;
			BarUI_Controller.Instance.SetTarget(null);
			StartCoroutine(AutoCastingNull(velocity));
		}

		protected override IEnumerator AutoCasting(float CoolTime)
		{
			IsAutoCasting = true;
			yield return new WaitForSeconds(CoolTime);
			IsAutoCasting = false;
		}

		protected override IEnumerator AutoCastingNull(float velocity)
		{
			if (IsAutoCasting)
			{
				yield break;
			}

			while (true)
			{ 
				IsAutoCasting = true;
				BarUI_Controller.Instance.UpdateValue(velocity);
				yield return new WaitForEndOfFrame();
				if (BarUI_Controller.Instance.GetValue >= 1.0f)
				{ 
					IsAutoCasting = false;
				}
			}
		}

		


		/*--- Private Methods ---*/
		void CheckGauge()
		{
			if (GetGaugeRate >= 1.0f&& CanActiveToDoll)
			{
				FinalAltar.Instance.DisableNormalAltar();
			}
		}


		
    }
}