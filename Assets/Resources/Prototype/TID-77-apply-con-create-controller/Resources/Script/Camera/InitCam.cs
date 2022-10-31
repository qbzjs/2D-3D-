using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class InitCam: MonoBehaviour
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/
		void Awake()
		{
			NetworkBaseController[] Players = GameObject.FindObjectsOfType<NetworkBaseController>();
		}
        private void Start()
        {
            
        }
        void Update()
		{
		
		}


		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}