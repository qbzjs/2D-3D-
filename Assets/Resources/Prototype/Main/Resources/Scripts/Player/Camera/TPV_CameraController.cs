using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

namespace KSH_Lib
{
    public class TPV_CameraController : BaseCameraController
    {
        /*--- Inspector ---*/
        [Header ( "Camera Speed Setting" )]
        [SerializeField]
        float zoomSpeed = 0.001f;
        [SerializeField]
        float zoomAccel = 0.15f;
        
        [Header ("Limit Setting")]
        [SerializeField]
        float minZoomLength = 2.0f;
        [SerializeField]
        float maxZoomLength = 5.0f;

        [Header("Invert Setting")]
        [SerializeField]
        bool isInvertZoom = false;


        /*--- Private Field ---*/
        public Cinemachine3rdPersonFollow Cm3rdPersonFollow { get; private set; }
        float zoomValRaw;
        float zoomVal;


        /*--- Monobehaviour Callbacks ---*/
        protected override void Start()
        {
            base.Start();
            Cm3rdPersonFollow = virtualCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            if ( Cm3rdPersonFollow == null )
            {
                Debug.LogError( "BaseCameraController: Can not find Cinemachine3rdPersonFollow" );
            }
        }
        protected override void LateUpdate()
        {
            if(!canUpdate)
            {
                return;
            }

            base.LateUpdate();
            ZoomCamera();
        }


        /*--- Protected Methods ---*/
        protected override void GetDataFromInputManager()
        {
            base.GetDataFromInputManager();

            zoomValRaw = zoomSpeed * BasePlayerInputManager.Instance.GetCameraZoom();
            if ( isInvertZoom )
            {
                zoomValRaw = -zoomValRaw;
            }
        }
        protected override void SmoothInputData()
        {
            base.SmoothInputData();
            zoomVal = Mathf.SmoothStep( zoomVal, zoomValRaw, zoomAccel );
        }


        /*--- Private Methods ---*/
        void ZoomCamera()
        {
            Cm3rdPersonFollow.CameraDistance += zoomVal;
            if ( Cm3rdPersonFollow.CameraDistance <= minZoomLength )
            {
                Cm3rdPersonFollow.CameraDistance = minZoomLength;
            }
            else if ( Cm3rdPersonFollow.CameraDistance >= maxZoomLength )
            {
                Cm3rdPersonFollow.CameraDistance = maxZoomLength;
            }
        }
    }
}