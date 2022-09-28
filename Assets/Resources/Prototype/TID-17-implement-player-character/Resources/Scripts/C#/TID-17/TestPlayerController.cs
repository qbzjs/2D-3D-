using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    #region Public Fields

    [Header("Character Object Setting")]

    [Tooltip( "The Object has CharacterController Component")]
    [SerializeField]
    GameObject characterObj;

    [Tooltip( "Actual Model of Character" )]
    [SerializeField]
    GameObject characterModel;

    [Tooltip("Camera Target for Camera, This is used for rotation of Camera")]
    [SerializeField]
    GameObject camTarget;


    [Header( "Character Status Setting" )]
    [SerializeField]
    float rotateSpeed = 600.0f;
    [SerializeField]
    float moveSpeed = 6.0f;

    #endregion


    #region Protected Fields

    /*---- Player Movement Factors ----*/
    protected CharacterController controller;
    protected Vector3 forward;
    protected Vector3 direction;
    protected float horizontal;
    protected float vertical;

    /*---- Camera Vectors ----*/
    protected Vector3 camForward;
    protected Vector3 camProjToPlane;
    protected Vector3 camRight;

    #endregion


    #region Private Fields


    #endregion


    #region MonoBehaviour Callbacks
    protected virtual void Start()
    {
        controller = characterObj.GetComponent<CharacterController>();
        if(controller == null)
        {
            Debug.LogError( "Missing Controller" );
        }
    }
    protected virtual void Update()
    {
        PlayerInput();
        SetDirection();
        RotateToDirection();
        MoveCharacter();
    }
    #endregion


    #region Public Methods

    #endregion


    #region Protected Methods
    protected virtual void PlayerInput()
    {
        horizontal = Input.GetAxis( "Horizontal" );
        vertical = Input.GetAxis( "Vertical" );
    }
    protected virtual void SetDirection()
    {
        camForward = camTarget.transform.forward;
        camProjToPlane = Vector3.ProjectOnPlane( camForward, Vector3.up );
        camRight = camTarget.transform.right;
        direction = (horizontal * camRight + vertical * camProjToPlane).normalized;
    }
    protected virtual void RotateToDirection()
    {
        if ( direction.sqrMagnitude > 0.01f )
        {
            forward = Vector3.Slerp( characterModel.transform.forward, direction,
                rotateSpeed * Time.deltaTime / Vector3.Angle( characterModel.transform.forward, direction ) );
            characterModel.transform.LookAt( characterModel.transform.position + forward );
        }
    }
    protected virtual void MoveCharacter()
    {
        controller.Move( moveSpeed * Time.deltaTime * direction );
    }

    #endregion


    #region Private Methods
    #endregion
}
