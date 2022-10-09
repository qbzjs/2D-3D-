using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace KSH_Lib
{
	public class FPV_CameraController : BaseCameraController
	{
        /*--- Monobehviour Callbacks ---*/
        protected override void LateUpdate()
        {
            if(canUpdate)
            {
                base.LateUpdate();
            }
        }
    }
}