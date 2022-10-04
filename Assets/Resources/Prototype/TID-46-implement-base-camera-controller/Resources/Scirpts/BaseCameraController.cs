using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

namespace KSH_Lib
{
    public class BaseCameraController : MonoBehaviour
    {
        #region Public Fields
        public enum CamType
        {
            FPV,
            TPV
        }
        #endregion


        #region Private Fields

        /*--- Camera Option ---*/
        [Header( "Camera Option" )]
        [SerializeField]
        CamType camType;
        [Header( "Object Setting" )]
        [SerializeField]
        protected GameObject camTarget;
        [SerializeField]
        protected CinemachineVirtualCamera virtualCam;

        [Header( "Camera Speed Setting" )]
        [SerializeField]
        float mouseSpeed = 1.5f;
        [SerializeField]
        float mouseAccel = 0.15f;
        [SerializeField]
        float zoomSpeed = 3.0f;
        [SerializeField]
        float zoomAccel = 0.15f;

        [Header( "Limit Setting" )]
        [SerializeField]
        float minAngleY = -40.0f;
        [SerializeField]
        float maxAngleY = 40.0f;
        [SerializeField]
        float minZoomLength = 2.0f;
        [SerializeField]
        float maxZoomLength = 5.0f;

        [Header( "Invert Setting" )]
        [SerializeField]
        bool isInvertHorizontal = false;
        [SerializeField]
        bool isInvertVertical = true;
        [SerializeField]
        bool isInvertZoom = false;

        Vector2 camAxisRaw;
        float zoomValRaw;
        Vector2 camAxis;
        float zoomVal;
        Vector3 angles;

        Cinemachine3rdPersonFollow cm3rdPersonFollow;

        #endregion


        #region MonoBehaviour Callbacks
        private void Start()
        {
            switch(camType)
            {
                case CamType.FPV:
                {

                }
                break;
                case CamType.TPV:
                {
                    cm3rdPersonFollow = virtualCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
                    if ( cm3rdPersonFollow == null )
                    {
                        Debug.LogError( "BaseCameraController: Can not find Cinemachine3rdPersonFollow" );
                    }
                }
                break;

                default:
                {
                    Debug.LogError( "BaseCameraController: Camera Type error" );
                }
                break;
            }
        }

        private void LateUpdate()
        {
            switch(camType)
            {
                case CamType.FPV:
                { 

                }
                break;
                case CamType.TPV:
                {
                    GetDataFromInputManager();
                    SmoothInputData();
                    RotateCamera();
                    ZoomCamera();
                }
                break;
            }
        }

        #endregion


        #region Public Methods
        #endregion


        #region Private Methods
        void GetDataFromInputManager()
        {
            camAxisRaw = mouseSpeed * BasePlayerInputManager.Instance.GetCameraLook();
            if( isInvertHorizontal )
            {
                camAxisRaw.x = -camAxisRaw.x;
            }
            if(isInvertVertical)
            {
                camAxisRaw.y = -camAxisRaw.y;
            }

            zoomValRaw = zoomSpeed * BasePlayerInputManager.Instance.GetCameraZoom();
            if(isInvertZoom)
            {
                zoomValRaw = -zoomValRaw;
            }
        }

        void SmoothInputData()
        {
            camAxis.x = Mathf.SmoothStep( camAxis.x, camAxisRaw.x, mouseAccel );
            camAxis.y = Mathf.SmoothStep( camAxis.y, camAxisRaw.y, mouseAccel );
            zoomVal = Mathf.SmoothStep( zoomVal, zoomValRaw, zoomAccel );
        }

        void RotateCamera()
        {
            camTarget.transform.Rotate( Vector3.up, camAxis.x * mouseSpeed, Space.World );
            camTarget.transform.Rotate( Vector3.right, camAxis.y * mouseSpeed, Space.Self );

            angles = camTarget.transform.eulerAngles;
            float angleVertical = angles.x;
            if ( angleVertical >= maxAngleY && angleVertical <= 180.0f )
            {
                camTarget.transform.eulerAngles = new Vector3( maxAngleY, angles.y, angles.z );
            }
            else if ( angleVertical <= 360.0f + minAngleY && angleVertical >= 180.0f )
            {
                camTarget.transform.eulerAngles = new Vector3( 360.0f + minAngleY, angles.y, angles.z );
            }
        }
        void ZoomCamera()
        {
            cm3rdPersonFollow.CameraDistance += zoomVal;
            if ( cm3rdPersonFollow.CameraDistance <= minZoomLength )
            {
                cm3rdPersonFollow.CameraDistance = minZoomLength;
            }
            else if ( cm3rdPersonFollow.CameraDistance >= maxZoomLength )
            {
                cm3rdPersonFollow.CameraDistance = maxZoomLengths;
            }
        }
        #endregion
    }
}

