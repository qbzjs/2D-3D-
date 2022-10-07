using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using LSH_Lib;
public class Network_TPV_CameraController : TPV_CameraController
{
    #region Public Fields
    #endregion	

    #region Private Fields
    private bool canUpdate = false;
    #endregion	

    #region MonoBehaviour CallBacks
    protected override void Start()
    {
        //base.Start();
    }
    protected override void Update()
    {
        if (canUpdate)
        { 
            base.Update();
        }
    }
    #endregion	

    #region Public Methods
    public void SetCamTarget(GameObject cam)
    {
        camTarget = cam;
    }
    public void SetModeTPV()
    {
        virtualCam.Follow = camTarget.transform;
        virtualCam.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
        virtualCam.AddCinemachineComponent<CinemachineSameAsFollowTarget>();
        base.Start();
        canUpdate = true;

    }
    #endregion	

    #region Private Methods
    #endregion	

}
