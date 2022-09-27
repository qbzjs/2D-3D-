using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPV_PlayerController : MonoBehaviour
{
    #region Public Field
    [SerializeField]
    private float walkSpeed = 3.0f;
    [SerializeField]
    private float rotSpeed = 2.0f;
    #endregion

    #region Private Fields
    Rigidbody rd;
    private CharacterController controller;
    private float playerSpeed = 10.0f;
    #endregion

    #region MonoBehaviour Callbacks
    private void Awake()
    {
        rd = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void FixedUpdate()
    {
        Movement();
        //Rotation();
        //ControllMove();
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void Movement()
    {
        Vector2 currentInput = TPV_PlayerInputManager.instance.GetTPVPlayerMove() * walkSpeed;
        //Debug.Log(currentInput);
        Vector3 dir = new Vector3(-Camera.main.transform.right.z, 0, Camera.main.transform.right.x);
        Vector3 moveDirection = (dir * currentInput.y + Camera.main.transform.right * currentInput.x + Vector3.up * rd.velocity.y);
        rd.velocity = moveDirection;
    }
    private void Rotation()
    {
        Vector2 currentRot = TPV_PlayerInputManager.instance.GetTPVPlayerLook() * Time.deltaTime * rotSpeed;
        Vector3 rot = new Vector3(currentRot.y, currentRot.x, 0.0f);
        transform.Rotate(0.0f, rot.y, 0.0f);
    }
    private void ControllMove()
    {
        Vector2 currentInput = PlayerInputManager.instance.GetPlayerMove() * playerSpeed;
        Debug.Log(currentInput);
        Vector3 dir = new Vector3(currentInput.x, 0f, currentInput.y);
        //dir = Camera.main.transform.forward * dir.z + Camera.main.transform.right * dir.x;
        if (dir != Vector3.zero)
        {
            transform.forward = dir;
        }
        rd.velocity = dir;
    }
    #endregion
}
