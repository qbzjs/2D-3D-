using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GHJ_Lib
{
	public class AltarCount: MonoBehaviour
	{
		/*--- Public Fields ---*/
		public Text AltarCountText; 
		public static AltarCount Instance
		{
			get 
			{
				if (!instance)
				{
					Debug.LogError("Missing AltarCount");
				}
				 
				return instance; 
				
			}
		}

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/
		private static AltarCount instance;
		private int altarCount=4;
		/*--- MonoBehaviour Callbacks ---*/
		void Start()
		{
			instance = this;
			AltarCountText.text ="Need AltarCount : "+ altarCount.ToString();
		}
		void Update()
		{
		
		}


		/*--- Public Methods ---*/
		public void DecreaseAltarCount()
		{
			altarCount--;
			if (altarCount <= 0)
			{
				AltarCountText.text = "!!Active FinalAltar!!";
			}
			else
			{ 
				AltarCountText.text = "Need AltarCount : " + altarCount.ToString();
			}
			
		}

		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}