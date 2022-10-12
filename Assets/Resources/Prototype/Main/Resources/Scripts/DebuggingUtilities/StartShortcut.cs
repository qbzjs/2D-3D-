using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartShortcut : MonoBehaviour
{
	/*--- Public Fields ---*/


	/*--- Protected Fields ---*/


	/*--- Private Fields ---*/


	/*--- MonoBehaviour Callbacks ---*/



	/*--- Public Methods ---*/
	public void OnStartShortcut()
    {
		GameManager.Instance.LoadSceneImmediately( "00_Launcher" );
    }

	/*--- Protected Methods ---*/


	/*--- Private Methods ---*/
}