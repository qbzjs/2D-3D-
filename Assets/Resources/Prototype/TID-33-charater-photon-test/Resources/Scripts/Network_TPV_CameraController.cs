using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
public class Network_TPV_CameraController : TPV_CameraController
{
    #region Public Fields
    #endregion	

    #region Private Fields
    #endregion	

    #region MonoBehaviour CallBacks
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }
    #endregion	

    #region Public Methods
    public void SetCamTarget(GameObject cam)
    {
        camTarget = cam;
    }
    #endregion	

    #region Private Methods
    #endregion	

}
