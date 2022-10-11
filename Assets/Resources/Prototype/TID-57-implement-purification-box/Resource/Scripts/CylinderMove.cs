using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ExorcistStates
{
    public bool hasDoll;
    public float castingSpeed;
}
namespace LSH_Lib{
    
    public class CylinderMove : MonoBehaviour
    {
        public ExorcistStates states;
        private void Start()
        {
            states.hasDoll = false;
            states.castingSpeed = 1.0f;
        }
        private void Update()
        {
            Move();
        }
        void Move()
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += transform.forward * 10.0f * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Rotate(Vector3.up, 50.0f * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Rotate(Vector3.down, 50.0f * Time.deltaTime);
            }
        }
    }
}
