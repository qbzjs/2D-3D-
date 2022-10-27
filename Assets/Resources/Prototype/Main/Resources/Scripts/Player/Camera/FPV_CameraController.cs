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

            camTarget.transform.Rotate(Vector3.up, camAxis.x, Space.World);
            camTarget.transform.Rotate(Vector3.right, camAxis.y, Space.Self);

            angles = camTarget.transform.eulerAngles;
            float angleVertical = angles.x;
            if (angleVertical >= maxAngleY && angleVertical <= 180.0f)
            {
                camTarget.transform.eulerAngles = new Vector3(maxAngleY, angles.y, angles.z);
            }
            else if (angleVertical <= 360.0f + minAngleY && angleVertical >= 180.0f)
            {
                camTarget.transform.eulerAngles = new Vector3(360.0f + minAngleY, angles.y, angles.z);
            }
        }
    }
}