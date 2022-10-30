using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KSH_Lib.UI
{
	public class UIEffector
	{
		public static IEnumerator Fade( CanvasGroup canvasGroup, float fadeTime, float dstAlpha )
		{
			canvasGroup.LeanAlpha( dstAlpha, fadeTime );
			yield return null;
		}
		public static IEnumerator PopUp( CanvasGroup canvasGroup, float fadeInTime, float waitTime, float fadeOutTime )
		{
			if ( canvasGroup.alpha <= 0.0f )
			{
				canvasGroup.LeanAlpha( 1.0f, fadeInTime );
			}
			yield return new WaitForSeconds( fadeInTime );

			if ( canvasGroup.alpha >= 1.0f )
			{
				yield return new WaitForSeconds( waitTime );
				canvasGroup.LeanAlpha( 0.0f, fadeOutTime );
			}
		}
		public static IEnumerator Fliker( CanvasGroup canvasGroup, float fadeInTime, float waitTime, float fadeOutTime, int flickerCount = -1, bool isStartWithFadeIn = true )
		{
			if ( flickerCount <= 0 )
			{
				while ( true )
				{
					yield return _Flicker( canvasGroup, fadeInTime, waitTime, fadeOutTime, isStartWithFadeIn );
				}
			}
			else
			{
				yield return _Flicker( canvasGroup, fadeInTime, waitTime, fadeOutTime, flickerCount, isStartWithFadeIn );
			}
		}
			


		static IEnumerator _Flicker( CanvasGroup canvasGroup, float fadeInTime, float waitTime, float fadeOutTime, bool isStartWithFadeIn )
		{
			if(isStartWithFadeIn)
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
			else
            {
				if ( canvasGroup.alpha >= 1.0f )
				{
					canvasGroup.LeanAlpha( 0.0f, fadeInTime );
					yield return null;
				}
				else if ( canvasGroup.alpha <= 0.0f )
				{
					yield return new WaitForSeconds( waitTime );
					canvasGroup.LeanAlpha( 1.0f, fadeOutTime );
					yield return null;
				}
			}
		}

		static IEnumerator _Flicker( CanvasGroup canvasGroup, float fadeInTime, float waitTime, float fadeOutTime, int flickerCount, bool isStartWithFadeIn )
		{
			for ( int i = 0; i < flickerCount; )
			{
				if(isStartWithFadeIn)
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
						++i;
						yield return null;
					}
				}
				else
				{
					if ( canvasGroup.alpha >= 1.0f )
					{
						canvasGroup.LeanAlpha( 0.0f, fadeInTime );
						yield return null;
					}
					else if ( canvasGroup.alpha <= 0.0f )
					{
						yield return new WaitForSeconds( waitTime );
						canvasGroup.LeanAlpha( 1.0f, fadeOutTime );
						++i;
						yield return null;
					}
				}
				yield return null;
			}
		}
	}
}