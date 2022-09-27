using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FPV_CharacterController : MonoBehaviour
{
    #region Public Field
    [SerializeField]
    private float walkSpeed = 3.0f;
    [SerializeField]
    private float rotSpeed = 2.0f;
    public Vector2 turn;
    
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
        //ControllMove();
        //Rotation();
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void Movement()
    {
        Vector2 currentInput = FPV_InputManager.instance.GetPlayerMove() * walkSpeed;
        //Debug.Log(currentInput);
        Vector3 dir = new Vector3(-Camera.main.transform.right.z, 0, Camera.main.transform.right.x);
        Vector3 moveDirection = (dir * currentInput.y + Camera.main.transform.right * currentInput.x + Vector3.up * rd.velocity.y);
        rd.velocity = moveDirection;
    }
    private void Rotation()
    {
        Vector2 currentRot = FPV_InputManager.instance.GetPlayerLook() * Time.deltaTime * rotSpeed;
        Vector3 rot = new Vector3(0.0f, currentRot.y,0.0f);
        transform.Rotate(0.0f, rot.y, 0.0f);
    }
    private void ControllMove() 
    {
        Vector2 currentInput = FPV_InputManager.instance.GetPlayerMove() * playerSpeed;
        Vector3 dir = new Vector3(currentInput.x, 0f, currentInput.y);
        Vector3 move = Camera.main.transform.forward * dir.z + Camera.main.transform.right * dir.x;
        if (move != Vector3.zero)
        {
            transform.forward = move;
        }
        rd.velocity = move;
    }
    #endregion
}