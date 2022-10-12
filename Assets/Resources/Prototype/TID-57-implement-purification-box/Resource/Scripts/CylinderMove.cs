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
            hasDoll();
        }
        private void OnGUI()
        {
            GUI.Box(new Rect(0,30, 150,30), "Exorcist has Doll :" + states.hasDoll.ToString());
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
        void hasDoll()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                states.hasDoll = true;
            }
        }
    }
}
