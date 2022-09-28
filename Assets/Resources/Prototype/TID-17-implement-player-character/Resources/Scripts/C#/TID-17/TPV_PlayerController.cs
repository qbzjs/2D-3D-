using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPV_PlayerController : MonoBehaviour
{
    #region Public Field
    [SerializeField]
    private float walkSpeed = 3.0f;
    [SerializeField]
    private float rotSpeed = 1000.0f;
    
    #endregion

    #region Private Fields
    Rigidbody rd;
    private CharacterController controller;
    private float playerSpeed = 10.0f;
    private Vector2 turn;
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
        Rotation();
        //ControllMove();
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void Movement()
    {
        Vector2 currentInput = TPV_PlayerInputManager.instance.GetTPVPlayerMove() * walkSpeed;
        Debug.Log(currentInput);
        Vector3 dir = new Vector3(currentInput.x, 0f, currentInput.y);
        dir = Camera.main.transform.forward * dir.z + Camera.main.transform.right * dir.x;

        rd.velocity = dir;
    }
    private void Rotation()
    {
        turn.x += Input.GetAxis("Mouse X") * rotSpeed;
        turn.y += Input.GetAxis("Mouse Y") * rotSpeed;
        this.gameObject.transform.localRotation = Quaternion.Euler(0, turn.x, 0);
    }
    private void ControllMove()
    {
        Vector2 currentInput = PlayerInputManager.instance.GetPlayerMove() * playerSpeed;
        Debug.Log(currentInput);
        Vector3 dir = new Vector3(currentInput.x, 0f, currentInput.y);
        dir = Camera.main.transform.forward * dir.z + Camera.main.transform.right * dir.x;
        if (dir != Vector3.zero)
        {
            transform.forward = dir;
        }
        rd.velocity = dir;
    }
    #endregion
}
