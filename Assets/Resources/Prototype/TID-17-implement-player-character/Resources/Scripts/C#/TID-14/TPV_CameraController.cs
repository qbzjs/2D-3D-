using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace LSH_Lib
{
    public class TPV_CameraController : MonoBehaviour
    {
        #region Public Fields

        [Header( "Object Setting" )]
        [SerializeField]
        protected GameObject camTarget;
        [SerializeField]
        protected CinemachineVirtualCamera virtualCam;

        [Header( "Speed Setting" )]
        [SerializeField]
        float mouseSpeed = 1.5f;
        [SerializeField]
        float mouseAccelerationTime = 0.15f;
        [SerializeField]
        float zoomSpeed = 3.0f;
        [SerializeField]
        float zoomAccelerationTime = 0.15f;

        [Header( "Limit Setting" )]
        [SerializeField]
        float minAngleVertical = -40.0f;
        [SerializeField]
        float maxAngleVertical = 40.0f;
        [SerializeField]
        float minZoomDistance = 2.0f;
        [SerializeField]
        float maxZoomDistance = 5.0f;

        [Header( "Invert Setting" )]
        [SerializeField]
        bool InvertHorizontal = false;
        [SerializeField]
        bool InvertVertical = true;
        [SerializeField]
        bool InvertZoom = false;

        #endregion


        #region Private Fields
        float xAxis;
        float yAxis;
        float vertical;
        float horizontal;
        float wheelValRaw;
        float wheelVal;
        Vector3 angles;
        Cinemachine3rdPersonFollow cmThirdPersonFollow;
        #endregion


        #region MonoBehaviour Callbacks
        protected virtual void Start()
        {
            cmThirdPersonFollow = virtualCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            if ( cmThirdPersonFollow == null )
            {
                Debug.LogError( "Can not find Cinemachine3rdPersonFollow" );
            }
        }
        protected virtual void Update()
        {
            if (!TPV_PlayerInputManager.instance.enabled)
            {
                return;
            }

            MouseInput();
            RotateCamera();
            ZoomCamera();
        }
        #endregion


        #region Public Methods
        #endregion


        #region Private Methods
        void MouseInput()
        {
            
                
            xAxis = (InvertHorizontal ? -Input.GetAxis( "Mouse X" ) : Input.GetAxis( "Mouse X" )) * mouseSpeed;
            yAxis = (InvertVertical ? -Input.GetAxis( "Mouse Y" ) : Input.GetAxis( "Mouse Y" )) * mouseSpeed;
            wheelValRaw = (InvertZoom ? Input.GetAxis( "Mouse ScrollWheel" ) : -Input.GetAxis( "Mouse ScrollWheel" )) * zoomSpeed;

            horizontal = Mathf.SmoothStep( horizontal, xAxis, mouseAccelerationTime );
            vertical = Mathf.SmoothStep( vertical, yAxis, mouseAccelerationTime );
            wheelVal = Mathf.SmoothStep( wheelVal, wheelValRaw, zoomAccelerationTime );
        }
        void RotateCamera()
        {

            camTarget.transform.Rotate( Vector3.up, horizontal * mouseSpeed, Space.World );

            camTarget.transform.Rotate( Vector3.right, vertical * mouseSpeed, Space.Self );
            angles = camTarget.transform.eulerAngles;
            float angleVertical = angles.x;
            if ( angleVertical >= maxAngleVertical && angleVertical <= 180.0f )
            {
                camTarget.transform.eulerAngles = new Vector3( maxAngleVertical, angles.y, angles.z );
            }
            else if ( angleVertical <= 360.0f + minAngleVertical && angleVertical >= 180.0f )
            {
                camTarget.transform.eulerAngles = new Vector3( 360 + minAngleVertical, angles.y, angles.z );
            }
        }
        void ZoomCamera()
        {
            cmThirdPersonFollow.CameraDistance += wheelVal;
            if ( cmThirdPersonFollow.CameraDistance <= minZoomDistance )
            {
                cmThirdPersonFollow.CameraDistance = minZoomDistance;
            }
            else if ( cmThirdPersonFollow.CameraDistance >= maxZoomDistance )
            {
                cmThirdPersonFollow.CameraDistance = maxZoomDistance;
            }
        }
        #endregion
    }
}