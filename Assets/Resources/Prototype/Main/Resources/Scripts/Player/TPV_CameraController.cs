using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

namespace KSH_Lib
{
    public class TPV_CameraController : BaseCameraController
    {
        #region Private Fields
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

        float zoomValRaw;
        float zoomVal;

        Cinemachine3rdPersonFollow cm3rdPersonFollow;
        #endregion


        #region MonoBehaviour Callbacks
        protected override void Start()
        {
            base.Start();
            cm3rdPersonFollow = virtualCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
            if ( cm3rdPersonFollow == null )
            {
                Debug.LogError( "BaseCameraController: Can not find Cinemachine3rdPersonFollow" );
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            ZoomCamera();
        }
        #endregion

        #region Public Methods
        public override void InitCam( GameObject camTarget )
        {
            this.camTarget = camTarget;

            virtualCam.Follow = this.camTarget.transform;
            virtualCam.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
            virtualCam.AddCinemachineComponent<CinemachineSameAsFollowTarget>();

            base.Start();
            canUpdate = true;
        }
        #endregion

        #region Protected Methods
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
        #endregion


        #region Private Methods
        void ZoomCamera()
        {
            cm3rdPersonFollow.CameraDistance += zoomVal;
            if ( cm3rdPersonFollow.CameraDistance <= minZoomLength )
            {
                cm3rdPersonFollow.CameraDistance = minZoomLength;
            }
            else if ( cm3rdPersonFollow.CameraDistance >= maxZoomLength )
            {
                cm3rdPersonFollow.CameraDistance = maxZoomLength;
            }
        }
        #endregion
    }
}