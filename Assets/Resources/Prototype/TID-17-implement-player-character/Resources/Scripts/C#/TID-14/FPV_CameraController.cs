using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FPV_CameraController : CinemachineExtension
{

    #region Public Fields
    //[SerializeField]
    //private float clampAngleX = 35.0f;
    [SerializeField]
    private float clampAngleY = 35.0f;
    [SerializeField]
    private float horizontalSpeed = 10.0f;
    [SerializeField]
    private float verticalSpeed = 10.0f;

    #endregion

    #region Private Fields
    private FPV_InputManager inputManager;
    private Vector3 startingRotation = new Vector3();
    
    #endregion

    #region MonoBehaviour Callbacks
    protected override void Awake()
    {
        inputManager = FPV_InputManager.instance;
        base.Awake();
    }
    protected virtual void Start()
    {
        if (startingRotation == null)
        {
            startingRotation = transform.localRotation.eulerAngles;
        }
    }
    void Update()
    {
        
    }
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    #endregion

    #region Protected Methods
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                Vector2 deltaInput = inputManager.GetPlayerLook();
                startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngleY, clampAngleY);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0.0f);
            }
        }
    }
    #endregion
}
