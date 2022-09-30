using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class NetworkFPV_CharacterController : FPV_CharacterController
{
    #region Public Fields
    #endregion

    #region Private Fields
    #endregion

    #region MonoBehaviour CallBacks
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }


    protected override void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            base.FixedUpdate();
        }
    }
    #endregion
    
    #region Public Methods
    #endregion
    
    #region Private Methods
    #endregion
}
