using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class FPSMoniter : MonoBehaviour
{
    /*--- Public Fields ---*/
	public bool activeMonitor = false;

	public TextMeshProUGUI FpsText;

	float pollingTime = 1.0f;
	float time;
	int frameCount;

    /*--- Protected Fields ---*/


    /*--- Private Fields ---*/


    /*--- MonoBehaviour Callbacks ---*/
    private void Awake()
    {
		DontDestroyOnLoad( gameObject );
    }
	void Update()
	{
		if( !activeMonitor )
        {
			return;
        }

		time += Time.deltaTime;

		frameCount++;

		if(time >= pollingTime)
        {
			int framRate = Mathf.RoundToInt( frameCount / time );
			FpsText.text = $"{framRate} FPS";

			time -= pollingTime;
			frameCount = 0;
		}
	}


	/*--- Public Methods ---*/


	/*--- Protected Methods ---*/


	/*--- Private Methods ---*/
}