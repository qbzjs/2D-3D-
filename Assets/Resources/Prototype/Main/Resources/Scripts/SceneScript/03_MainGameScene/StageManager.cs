using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib
{
	public class StageManager : MonoBehaviour
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/
		void Start()
		{
			DataManager.Instance.StartGame();
		}


		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}