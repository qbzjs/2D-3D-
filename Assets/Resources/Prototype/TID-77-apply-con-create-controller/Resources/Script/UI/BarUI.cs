using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GHJ_Lib
{
	public class BarUI: MonoBehaviour
	{
		/*--- Public Fields ---*/
		public static BarUI Instance
		{
			get { return instance; }
		}
		public bool IsAutoCasting
		{
			get { return isAutoCastingNull; }
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

			if (bar.gameObject.activeInHierarchy)
			{
				bar.value = targetInteraction.GetGaugeRate;
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
			SliderVisible(true);
			bar.value = 0;
			while (true)
			{
				isAutoCastingNull = true;
				bar.value += 1 / chargeTime;
				yield return new WaitForEndOfFrame();
				if (bar.value >= 1.0f)
				{
					SliderVisible(false);
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
			SliderVisible(true);
			bar.value = 0;
			while (true)
			{
				isAutoCasting = true;
				bar.value += 1 / chargeTime;
				yield return new WaitForEndOfFrame();
				if (bar.value >= 1.0f)
				{
					SliderVisible(false);
					isAutoCasting = false;
				}

			}
		}

	}
}