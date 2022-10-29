using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KSH_Lib.UI
{
	public class UIEffector
	{
		/*--- Constructor ---*/
		//public UIEffector(in CanvasGroup canvasGroup)
   //     {
			//this.TargetCanvasGroup = canvasGroup;
   //     }


		/*--- Public Fields ---*/
		public CanvasGroup TargetCanvasGroup;
		//public bool IsEffectStart { get; private set; }


		/*--- Public Methods ---*/

		//public IEnumerator FadeIn(float fadeTime)
		//{
		//	if (!IsEffectStart)
		//	{
		//		IsEffectStart = true;
		//		while (TargetCanvasGroup.alpha < 1.0f)
		//		{
		//			TargetCanvasGroup.alpha += Time.deltaTime / fadeTime;
		//			yield return null;
		//		}
		//		yield return null;
		//		IsEffectStart = false;
		//	}
		//}
		

		public static IEnumerator Fade( CanvasGroup canvasGroup, float fadeTime, float srcAlpha, float dstAlpha)
        {
			canvasGroup.alpha = srcAlpha;
			canvasGroup.LeanAlpha( 1, 1.0f );
			yield return null;
		}
		public static IEnumerator PopUp( CanvasGroup canvasGroup, float fadeInTime, float waitTime, float fadeOutTime )
		{
			if ( canvasGroup.alpha <= 0.0f )
			{
				canvasGroup.LeanAlpha( 1.0f, fadeInTime );
				yield return null;
			}
			else if ( canvasGroup.alpha >= 1.0f )
			{
				yield return new WaitForSeconds( waitTime );
				canvasGroup.LeanAlpha( 0.0f, fadeOutTime );
				yield return null;
			}
		}
		public static IEnumerator Fliker( CanvasGroup canvasGroup, float fadeInTime, float waitTime, float fadeOutTime )
        {
			while ( true )
			{
				yield return PopUp( canvasGroup, fadeInTime, waitTime, fadeOutTime );
			}
		}


		//public IEnumerator FadeOut(float fadeTime)
		//{
		//	if(!IsEffectStart)
		//          {
		//		IsEffectStart = true;
		//		while (TargetCanvasGroup.alpha > 0.0f)
		//		{
		//			TargetCanvasGroup.alpha -= Time.deltaTime / fadeTime;
		//			yield return null;
		//		}
		//		IsEffectStart = false;
		//	}
		//}
		//public IEnumerator PopUp(float fadeInTime, float waitTime, float fadeOutTime)
		//      {
		//	if(!IsEffectStart)
		//          {
		//		IsEffectStart = true;
		//		while (TargetCanvasGroup.alpha < 1.0f)
		//		{
		//			TargetCanvasGroup.alpha += Time.deltaTime / fadeInTime;
		//			yield return null;
		//		}

		//		yield return new WaitForSeconds(waitTime);

		//		while (TargetCanvasGroup.alpha > 0.0f)
		//		{
		//			TargetCanvasGroup.alpha -= Time.deltaTime / fadeOutTime;
		//			yield return null;
		//		}

		//		IsEffectStart = false;
		//	}
		//}


	}
}