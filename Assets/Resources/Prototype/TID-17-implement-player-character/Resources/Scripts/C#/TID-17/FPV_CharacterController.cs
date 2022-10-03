using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
public class FPV_CharacterController : MonoBehaviourPunCallbacks
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
    private Vector2 turn;
    private Animator animator;
    #endregion

    #region MonoBehaviour Callbacks
    protected virtual void Awake()
    {
        rd = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    protected virtual void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    protected virtual void FixedUpdate()
    {
        
        Movement();
        mouseRotate();
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void Movement()
    {
        Vector2 currentInput = FPV_InputManager.instance.GetPlayerMove() * walkSpeed;
        Vector3 dir = new Vector3(currentInput.x, 0f, currentInput.y);
        dir = Camera.main.transform.forward * dir.z + Camera.main.transform.right * dir.x;
        rd.velocity = dir;
        animator.SetFloat("MoveSpeed", rd.velocity.magnitude);
    }
    private void mouseRotate()
    {
        turn.x += Input.GetAxis("Mouse X") * rotSpeed;
        turn.y += Input.GetAxis("Mouse Y") * rotSpeed;
        this.gameObject.transform.localRotation = Quaternion.Euler(0,turn.x,0);
    }
    #endregion
}