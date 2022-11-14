using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GHJ_Lib
{
	public class InputActionUI: MonoBehaviour
	{
		WaitForEndOfFrame frame = new WaitForEndOfFrame();
		public Image ICon;
		public Image CoolTimeCover;
		public TextMeshProUGUI CoolTime; 
		bool IsBlink=false;
		bool IsCountDown = false;

		public void PushButton(bool isDown)
		{
			CoolTimeCover.fillAmount = 1.0f;
			CoolTimeCover.enabled = isDown;
		}
		

		public void StartCountDown(float time)
		{
			StartCoroutine(CountDown(time));
		}
		public IEnumerator CountDown(float time)
		{
			if (IsCountDown)
			{
				yield break;
			}
			IsCountDown = true;

			CoolTimeCover.enabled = true;
			CoolTime.enabled = true;

			float maxTime = time;
			while (true)
			{
				CoolTimeCover.fillAmount = time / maxTime;
				CoolTime.text = ((int)time).ToString();
				time -= Time.deltaTime;
				yield return frame;
				if (time<=3.0f)
				{
					StartCoroutine(Blink(time));
				}
				if (time <= 0.0f)
				{
					CoolTimeCover.enabled = false;
					CoolTime.enabled = false;
					IsBlink = false;
					break;
				}
			}
		}
		public IEnumerator Blink(float time)
		{
			if (IsBlink)
			{
				yield break;
			}
			IsBlink = true;
			bool IconEnable = false;
			float blinkTime = 0;
			while (true)
            {
				ICon.enabled = IconEnable;
				blinkTime += Time.deltaTime;
				time -= Time.deltaTime;
				yield return frame;
				if (time <= 0.0f)
				{
					IsBlink = false;
					break;
				}
				if (blinkTime > 0.5f)
				{
					IconEnable = true;
					blinkTime = 0.0f;
				}
			}

		}

		
	}
}