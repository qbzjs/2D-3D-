using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceOnStart : MonoBehaviour
{

    [SerializeField]
    private Vector3 directionRelative;

    [SerializeField]
    private float magnitude;

    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(directionRelative.normalized * magnitude, ForceMode.Impulse);
    }
}
