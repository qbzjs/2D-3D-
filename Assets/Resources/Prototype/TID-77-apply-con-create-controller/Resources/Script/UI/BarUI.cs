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
		public float GetValue
		{
			get { return bar.value; }
		}
		protected static BarUI instance;

		/*--- Private Fields ---*/
		[SerializeField]
		Slider bar;
		[SerializeField]
		TextMeshProUGUI interactionText;
		private Interaction targetInteraction;

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
			if (targetInteraction != null)
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
			if (targetInteraction == null)
			{
				bar.value = 0;
			}
		}

		public void UpdateValue(float velocity)
		{
			bar.value += velocity  ;
		}

		public void UpdateValue()
		{
			bar.value = targetInteraction.GetGaugeRate;
		}

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/


	}
}