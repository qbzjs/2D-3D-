using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
	public class InputActionUI : MonoBehaviour
	{
		[Header("Setting Sprite According to TypeOrder")]
		public bool IsDifferentICon;
		[Header("ICon, you must follow roleType order")]
		public Sprite[] Icons = new Sprite[10];


		WaitForEndOfFrame frame = new WaitForEndOfFrame();
		[SerializeField] protected Image Icon;
		[SerializeField] protected Image CoolTimeCover;
		[SerializeField] protected TextMeshProUGUI CoolTime;
		bool IsBlink=false;
		bool IsCountDown = false;
		public float blinkControlTime = 0.5f;

        public void Start()
        {
			if (IsDifferentICon)
			{
				Icon.sprite = Icons[(int)DataManager.Instance.GetLocalRoleType];
			}
        }

        public void PushButton(bool isDown)
		{
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
					IsCountDown = false;
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
				Icon.enabled = IconEnable;
				blinkTime += Time.deltaTime;
				time -= Time.deltaTime;
				yield return frame;
				if (time <= 0.0f)
				{
					IsBlink = false;
					CoolTimeCover.enabled = true;
					break;
				}
				if (blinkTime > blinkControlTime)
				{
					if (IconEnable)
					{
						IconEnable = false;
					}
					else
					{
						IconEnable = true;
					}
					blinkTime = 0.0f;
				}
			}

		}

		
	}
}