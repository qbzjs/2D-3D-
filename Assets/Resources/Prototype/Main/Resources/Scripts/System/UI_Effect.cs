using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KSH_Lib.UI
{
	public class UI_Effect
	{
		/*--- Constructor ---*/
		public UI_Effect(in CanvasGroup canvasGroup)
        {
			this.TargetCanvasGroup = canvasGroup;
        }


		/*--- Public Fields ---*/
		public CanvasGroup TargetCanvasGroup;
		public bool IsEffectStart { get; private set; }


		/*--- Public Methods ---*/

		public IEnumerator FadeIn(float fadeTime)
		{
			if (!IsEffectStart)
			{
				IsEffectStart = true;
				while (TargetCanvasGroup.alpha < 1.0f)
				{
					TargetCanvasGroup.alpha += Time.deltaTime / fadeTime;
					yield return null;
				}
				yield return null;
				IsEffectStart = false;
			}
		}
		public IEnumerator FadeOut(float fadeTime)
		{
			if(!IsEffectStart)
            {
				IsEffectStart = true;
				while (TargetCanvasGroup.alpha > 0.0f)
				{
					TargetCanvasGroup.alpha -= Time.deltaTime / fadeTime;
					yield return null;
				}
				IsEffectStart = false;
			}
		}
		public IEnumerator PopUp(float fadeInTime, float waitTime, float fadeOutTime)
        {
			if(!IsEffectStart)
            {
				IsEffectStart = true;
				while (TargetCanvasGroup.alpha < 1.0f)
				{
					TargetCanvasGroup.alpha += Time.deltaTime / fadeInTime;
					yield return null;
				}

				yield return new WaitForSeconds(waitTime);

				while (TargetCanvasGroup.alpha > 0.0f)
				{
					TargetCanvasGroup.alpha -= Time.deltaTime / fadeOutTime;
					yield return null;
				}

				IsEffectStart = false;
			}
		}
	}
}