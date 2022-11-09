using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSonarRingColor : MonoBehaviour
{
    [SerializeField]
    private float secondsPerColor = 1;

    [SerializeField]
    private Color[] colors;

    private int colorIndex = 0;

    private bool colorChangingStarted = false;

    // Start is called before the first frame update
    public void StartColorChanging()
    {
        colorChangingStarted = true;
        InvokeRepeating("ChangeColor", 0, secondsPerColor);
    }

    void ChangeColor()
    {
        Shader.SetGlobalColor("_RingColorModified", colors[colorIndex]);
        colorIndex = (colorIndex + 1) % colors.Length;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!colorChangingStarted) StartColorChanging();
    }

}
