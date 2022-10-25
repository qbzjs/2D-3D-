using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GHJ_Lib;

namespace KSH_Lib
{
	public class StageManager : MonoBehaviour
	{
		/*--- Public Fields ---*/
		public GameObject prefab1;
		public GameObject prefab2;

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/

		/*--- MonoBehaviour Callbacks ---*/
		void Start()
		{
			DataManager.Instance.StartGame();
			
		}

        private void Update()
        {
			
        }


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}