using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using GHJ_Lib;
public class NetworkTPV_CharacterController : TestPlayerController
{
    #region Private Fields

    #endregion

    #region Protected Fields
    protected DollStatus dollStatus = null;
    protected DollAnimationController dollAnimationController;
    #endregion


    #region Public Fields
    #endregion

    #region MonoBehaviour CallBacks
    protected override void Start()
    {
        dollStatus = new DollStatus(DollType.Rabbit);
        dollAnimationController = GetComponent<DollAnimationController>();
        dollAnimationController.SetStatus(dollStatus);
        moveSpeed = dollStatus.MoveSpeed; //최종 스피드는 이동속도*상태*디버프 
        base.Start();   
    }


    protected override void Update()
    {
        if (photonView.IsMine)
        {
            PlayerInput();
            SetDirection();
            RotateToDirection();
            MoveCharacter();
        }
    }
    #endregion

    #region Private Methods
    #endregion

    #region Protected Methods
    protected override void PlayerInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (vertical != 0||horizontal!=0)
        {
            dollAnimationController.IsMove = true;
        }
        else
        {
            dollAnimationController.IsMove = false;
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            dollAnimationController.IsRolle = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            dollAnimationController.IsRolle = false;
        }
    }

    protected override void MoveCharacter()
    {
        controller.SimpleMove(direction*moveSpeed);

    }
    #endregion

    #region Public Methods
    #endregion

}
