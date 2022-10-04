using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using GHJ_Lib;
public class NetworkTPV_CharacterController : TestPlayerController,IPunObservable
{
    #region Private Fields
    protected streamVector3 sVector3;
    #endregion

    #region Protected Fields
    protected DollStatus dollStatus=null;
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
        }
            RotateToDirection();
            MoveCharacter();
    }
    #endregion

    #region Private Methods
    #endregion

    #region Protected Methods
    protected override void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //interact button in 
            dollAnimationController.PlayInteractAnimation();
            return;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            //interact button out
            dollAnimationController.CancelInteractAnimation();
            return;
        }


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
            dollAnimationController.IsRoll = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            dollAnimationController.IsRoll = false;
        }
    }

    protected override void MoveCharacter()
    {
        controller.SimpleMove(direction*moveSpeed);

    }
    #endregion

    #region Public Methods
    #endregion

    #region IPunObservable
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.IsWriting)
        {
            sVector3.x = direction.x;
            sVector3.y = direction.y;
            sVector3.z = direction.z;
            stream.SendNext(sVector3.x);
            stream.SendNext(sVector3.y);
            stream.SendNext(sVector3.z);
        }
        if (stream.IsReading)
        {
            this.sVector3.x = (float)stream.ReceiveNext();
            this.sVector3.y = (float)stream.ReceiveNext();
            this.sVector3.z = (float)stream.ReceiveNext();
            this.direction = new Vector3(sVector3.x, sVector3.y, sVector3.z);
        }
        
    }
    #endregion

}
