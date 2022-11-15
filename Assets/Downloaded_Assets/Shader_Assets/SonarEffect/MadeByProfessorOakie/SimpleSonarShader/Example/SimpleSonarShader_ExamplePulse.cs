using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSonarShader_ExamplePulse : MonoBehaviour
{
    [SerializeField]
    private float startTime = 0.0f;

    [SerializeField]
    private float pulseRepeatRate = 1.0f;

    [SerializeField]
    [Tooltip("Set to 0 to go forever")]
    private float durationTillEnd = 20.0f;

    [SerializeField]
    private float ringIntensity = 1.0f;

    [SerializeField]
    private int ringPassIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnRing", startTime, pulseRepeatRate);
        
        if(durationTillEnd > 0)
        {
            StartCoroutine(CancelPulse());
        }
    }

    private void SpawnRing()
    {
        SimpleSonarShader_SonarSender.Instance.StartSonarRing(transform.position, ringIntensity, ringPassIndex);
    }

    private IEnumerator CancelPulse()
    {
        yield return new WaitForSeconds(startTime + durationTillEnd);
        CancelInvoke("SpawnRing");
    }

}
