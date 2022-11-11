using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantVelocity : MonoBehaviour
{

    [SerializeField]
    private Vector3 directionWorld;

    [SerializeField]
    private float magnitude;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().velocity = directionWorld.normalized * magnitude;
    }

}
