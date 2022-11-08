using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace KSH_Lib
{
	public class FPV_CameraController : BaseCameraController
	{
        [Header("Limit Rotate Speed")   ]
        [SerializeField]
        protected float maxSpeedX = 5.0f;
        [SerializeField]
        protected float maxSpeedY = 5.0f;

        [field: SerializeField] public GameObject camIK { get; protected set; }


        /*--- Monobehviour Callbacks ---*/
        protected override void LateUpdate()
        {
            if(canUpdate)
            {
                base.LateUpdate();
            }
        }

        protected override void RotateCamera()
        {
            if(camTarget == null)
            {
                return;
            }
            camAxis *= mouseSpeed;

            if(camAxis.x > maxSpeedX)
            {
                camAxis.x = maxSpeedX;
            }
            else if(camAxis.x < -maxSpeedX)
            {
                camAxis.x = -maxSpeedX;
            }
            if(camAxis.y > maxSpeedY)
            {
                camAxis.y = maxSpeedY;
            }
            else if (camAxis.y < -maxSpeedY)
            {
                camAxis.y = -maxSpeedY;
            }

            //camTarget.transform.Rotate(Vector3.up, camAxis.x, Space.World);
            //camTarget.transform.Rotate(Vector3.right, camAxis.y, Space.Self);

            //camIK.transform.RotateAround(camTarget.transform.position, camTarget.transform.up, camAxis.x);



            //camIK.transform.LookAt(camTarget.transform);
            camTarget.transform.LookAt(camIK.transform);

            angles = camTarget.transform.eulerAngles;
            float angleVertical = angles.x;
            if (angleVertical > maxAngleY && angleVertical < 180.0f)
            {
                camTarget.transform.eulerAngles = new Vector3(maxAngleY, angles.y, angles.z);
                camIK.transform.RotateAround(camTarget.transform.position, camTarget.transform.right, -1f);
            }
            else if (angleVertical < 360.0f + minAngleY && angleVertical > 180.0f)
            {
                camTarget.transform.eulerAngles = new Vector3(360.0f + minAngleY, angles.y, angles.z);
                camIK.transform.RotateAround(camTarget.transform.position, camTarget.transform.right, 1f);
            }
            else
            {
                camIK.transform.RotateAround(camTarget.transform.position, Vector3.up, camAxis.x);
                camIK.transform.RotateAround(camTarget.transform.position, camTarget.transform.right, camAxis.y);
            }
            // https://answers.unity.com/questions/1370422/limit-y-axis-transformrotatearound.html
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