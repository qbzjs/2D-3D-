using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib{
    public class CylinderDollMove : MonoBehaviour
    {
        private void Update()
        {
            //Move();
        }
        private void Move()
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += transform.forward * 10.0f * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.Rotate(Vector3.up, 50.0f * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Rotate(Vector3.down, 50.0f * Time.deltaTime);
            }
        }
    }
}
