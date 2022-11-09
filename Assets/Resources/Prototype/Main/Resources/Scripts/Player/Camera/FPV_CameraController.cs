using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using KSH_Lib.Util;

namespace KSH_Lib
{
	public class FPV_CameraController : BaseCameraController
	{
        [Header("Limit Rotate Speed")   ]
        [SerializeField]
        protected float maxSpeedX = 5.0f;
        [SerializeField]
        protected float maxSpeedY = 5.0f;
        
        [field: SerializeField] public GameObject camAim { get; protected set; }
        //Vector3 nextCamAimPos;
        Vector3 initialVector;
        float angleY;

        /*--- Monobehviour Callbacks ---*/
        private void OnEnable()
        {
            initialVector = camAim.transform.position - camTarget.transform.position;
            initialVector.x = 0.0f;
        }
        protected override void LateUpdate()
        {
            if(canUpdate)
            {
                base.LateUpdate();
            }
        }
        protected override void RotateCamera()
        {
            if ( camTarget == null )
            {
                return;
            }
            camAxis *= mouseSpeed;

            camAxis.x = Mathf.Clamp( camAxis.x, -maxSpeedX, maxSpeedX );
            camAxis.y = Mathf.Clamp( camAxis.y, -maxSpeedY, maxSpeedY );

            angleY += camAxis.y;

            camAim.transform.RotateAround( camTarget.transform.position, Vector3.up, camAxis.x );
            if ( angleY < maxAngleY && angleY > minAngleY )
            {
                camAim.transform.RotateAround( camTarget.transform.position, camTarget.transform.right, camAxis.y );
            }
            else
            {
                angleY -= camAxis.y;
            }
            camTarget.transform.LookAt( camAim.transform ); 
        }

        public override void InitCam(GameObject camTarget)
        {
            if (camTarget == null)
            {
                Debug.LogError("BaseCameraController.InitCam(): No camTarget Inited");
                return;
            }

            this.camTarget = camTarget;

            virtualCam.AddCinemachineComponent<CinemachineHardLockToTarget>();
            virtualCam.AddCinemachineComponent<CinemachineSameAsFollowTarget>();
            virtualCam.Follow = this.camTarget.transform;

            canUpdate = true;
        }

        public float GetCamAxisX()
        {
            return camAxis.x;
        }
    }
}