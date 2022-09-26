using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPV_PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private float playerSpeed = 2.0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
       
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        controller.Move(playerVelocity * Time.deltaTime);
    }
}
