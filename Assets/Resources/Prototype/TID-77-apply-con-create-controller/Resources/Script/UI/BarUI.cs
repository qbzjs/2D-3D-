using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GHJ_Lib
{
	public class BarUI : MonoBehaviour
	{
		/*--- Public Fields ---*/
		public static BarUI Instance
		{
			get { return instance; }
		}
		public bool IsAutoCastingNull
		{
			get { return isAutoCastingNull; }
		}
		public bool IsAutoCasting
		{
			get { return isAutoCasting; }
		}
		public bool IsCasting
		{
			get { return isCasting; }
		}
		/*--- Protected Fields ---*/
		protected static BarUI instance;

		/*--- Private Fields ---*/
		[SerializeField]
		Slider bar;
		[SerializeField]
		TextMeshProUGUI interactionText;
		private Interaction targetInteraction;
		private bool isAutoCastingNull = false;
		private bool isAutoCasting = false;
		private bool isCasting = false;
		private float autoCastingTime = 0.0f;

		/*--- MonoBehaviour Callbacks ---*/
		void Start()
		{
			bar = GetComponentInChildren<Slider>();
			bar.gameObject.SetActive(false);
			interactionText = GetComponentInChildren<TextMeshProUGUI>();
			interactionText.gameObject.SetActive(false);
			instance = this;
		}
		void Update()
		{
			if (isAutoCastingNull)
			{
				return;
			}

			if (isAutoCasting)
			{
				targetInteraction.UpdateCurGaugeRate(bar.value);
				return;
			}

			if (isCasting&& targetInteraction)
			{
				bar.value = targetInteraction.GetGaugeRate;
				return;
			}

			
		}


		/*--- Public Methods ---*/
		public void SliderVisible(bool flag)
		{
			bar.gameObject.SetActive(flag);
		}
		public void TextVisible(bool flag)
		{
			interactionText.gameObject.SetActive(flag);
		}
		public void SetTarget(Interaction interaction)
		{
			targetInteraction = interaction;
		}
		public void BeginCasting()
		{
			isCasting = true;
		}
		public void EndCasitng()
		{
			isCasting = false;
		}
		public void AutoCastingNull(float chargeTime)
		{
			StartCoroutine("_AutoCastingNull", chargeTime);
		}
		public void AutoCasting(float chargeTime)
		{
			StartCoroutine("_AutoCasting", chargeTime);
		}
		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
		IEnumerator _AutoCastingNull(float chargeTime)
		{
			if (isAutoCastingNull)
			{
				yield break;
			}

			bar.value = 0;
			while (true)
			{
				isAutoCastingNull = true;
				bar.value += 1 / chargeTime;
				yield return new WaitForEndOfFrame();
				if (bar.value >= 1.0f)
				{

					isAutoCastingNull = false;
				}

			}
		}

		IEnumerator _AutoCasting(float chargeTime)
		{
			if (isAutoCasting)
			{
				yield break;
			}

			bar.value = 0;
			while (true)
			{
				isAutoCasting = true;
				bar.value += 1 / chargeTime;
				yield return new WaitForEndOfFrame();
				if (bar.value >= 1.0f)
				{

					isAutoCasting = false;
				}

			}
		}




	}
}