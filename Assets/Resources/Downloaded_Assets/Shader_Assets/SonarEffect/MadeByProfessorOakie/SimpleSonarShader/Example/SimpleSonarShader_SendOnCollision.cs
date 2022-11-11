// SimpleSonarShader scripts and shaders were written by Drew Okenfuss.

using System.Collections;
using UnityEngine;


public class SimpleSonarShader_SendOnCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Start sonar ring from the contact point
        SimpleSonarShader_SonarSender.Instance.StartSonarRing(collision.contacts[0].point, collision.impulse.magnitude);
    }
}
