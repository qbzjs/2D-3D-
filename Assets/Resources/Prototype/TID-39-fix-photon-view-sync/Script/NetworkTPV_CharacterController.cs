using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class NetworkTPV_CharacterController : TestPlayerController
{
    #region Private Fields
    #endregion
    
    #region Public Fields
    #endregion
    
    #region MonoBehaviour CallBacks
    protected override void Start()
    {
        base.Start();   
    }


    protected override void Update()
    {
        if (photonView.IsMine)
        {
            PlayerInput();
            SetDirection();
            RotateToDirection();
        }
        MoveCharacter();
    }
    #endregion
    
    #region Private Methods
    #endregion
    
    #region Public Methods
    #endregion
}
