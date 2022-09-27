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
    
    #endregion
}