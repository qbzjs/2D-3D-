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


		public override CastingType GetCastingType(BasePlayerController player)
		{
			if (player is DollController)
			{
				return CastingType.Casting;
			}

			if (player is ExorcistController)
			{
				return CastingType.AutoCastingNull;
			}

			Debug.LogError("Error get Casting Type");
			return CastingType.Casting;
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
			BarUI.Instance.SetTarget(this);
			BarUI.Instance.UpdateValue();
		}
        protected override void AutoCasting(BasePlayerController controller)
        {
			//controller에서 PlayerData 를 호출하고 interact Velocity를 받음.	
			float velocity = 1.0f;
			BarUI.Instance.SetTarget(null);
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
				BarUI.Instance.UpdateValue(velocity);
				yield return new WaitForEndOfFrame();
				if (BarUI.Instance.GetValue >= 1.0f)
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
				CanActiveToExorcist = false;
				CanActiveToDoll = false;
			}
			else
			{
				if (!CanActiveToDoll)
				{
					return;
				}
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