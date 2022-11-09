using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantTranslation : MonoBehaviour
{
    [SerializeField]
    private Vector3 directionWorld;

    [SerializeField]
    private float magnitude;

    private void FixedUpdate()
    {
        transform.position += directionWorld.normalized * magnitude * Time.fixedDeltaTime;
    }

}
