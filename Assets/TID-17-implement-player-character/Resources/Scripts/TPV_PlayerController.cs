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
        ControllMove();
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    
    private void ControllMove()
    {
        Vector2 currentInput = PlayerInputManager.instance.GetPlayerMove() * playerSpeed;
        Debug.Log(currentInput);
        Vector3 dir = new Vector3(currentInput.x, 0f, currentInput.y);
        Vector3 move = Camera.main.transform.forward * dir.z + Camera.main.transform.right * dir.x;
        if (dir != Vector3.zero)
        {
            transform.forward = dir;
        }
        rd.velocity = dir;
    }
    #endregion
}
