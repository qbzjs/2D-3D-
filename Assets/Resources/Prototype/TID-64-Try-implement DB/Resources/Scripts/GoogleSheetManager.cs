using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetManager : MonoBehaviour
{
	/*--- Public Fields ---*/


	/*--- Protected Fields ---*/


	/*--- Private Fields ---*/
	const string URL = "https://script.google.com/macros/s/AKfycbxNucd3Oy9gzABgI2Pub7q2Pd-OjWeRQ6x_SbIODFmXC5iEHuAmsCBkNneAMxYJNEIr/exec";


    /*--- MonoBehaviour Callbacks ---*/
    private IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequest.Get( URL );
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;
        print( data );
    }


    /*--- Public Methods ---*/


    /*--- Protected Methods ---*/


    /*--- Private Methods ---*/
}