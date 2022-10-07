using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using KSH_Lib;
using TID42;
public class Network_FPV_CameraController : FPV_CameraController
{
    #region Private Fields
    [SerializeField]
    protected CinemachineVirtualCamera virtualCam;
    private bool canUpdate = false;
    #endregion

    #region Public Fields
    #endregion

    #region MonoBehaviour CallBacks
    protected override void Awake()
    {
        //base.Awake();
    }
    protected override void Start()
    {
        //base.Start();
    }

    void Update()
    {
        
    }
    #endregion	

    #region Private Methods
    #endregion	

    #region Public Methods
    public void SetModeFPV(GameObject camTarget)
    {
        virtualCam.Follow = camTarget.transform;
        virtualCam.AddCinemachineComponent<CinemachineHardLockToTarget>();
        virtualCam.AddCinemachineComponent<CinemachinePOV>();
        if (GameObject.Find("Exorcist(Clone)"))
        {
            GameObject Exorcist = GameObject.Find("Exorcist(Clone)");
            virtualCam.LookAt = camTarget.transform.GetChild(0);

            if (Exorcist.GetComponent<FPV_CharacterController1>() == null)
            {
                Debug.LogError("Missing ExorcistController");
            }
            Exorcist.GetComponent<FPV_CharacterController1>().target = this.gameObject;
        }
        else
        {
            Debug.LogError("Missing Exorcist");
        }
       
        base.Awake();
        base.Start();
    }
    #endregion	

}
