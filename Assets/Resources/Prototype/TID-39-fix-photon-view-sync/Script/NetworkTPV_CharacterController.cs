using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;
using GHJ_Lib;

public class NetworkTPV_CharacterController : TestPlayerController,IPunObservable
{
    #region Private Fields
    protected streamVector3 sVector3;
    //protected Behavior<NetworkTPV_CharacterController> behavior = new Behavior<NetworkTPV_CharacterController>();
    public BvStatus<NetworkTPV_CharacterController> IdleBehavior = new BvStatus<NetworkTPV_CharacterController>();
    protected BvSlow<NetworkTPV_CharacterController> bvSlow = new BvSlow<NetworkTPV_CharacterController>();
    protected BvBlind<NetworkTPV_CharacterController> bvBlind = new BvBlind<NetworkTPV_CharacterController>();
    protected BvExpose<NetworkTPV_CharacterController> bvExpose = new BvExpose<NetworkTPV_CharacterController>();
    #endregion

    #region Protected Fields
    public DollStatus dollStatus=null;
    #endregion


    #region Public Fields
    public DollAnimationController dollAnimationController;
    #endregion

    #region MonoBehaviour CallBacks
    protected void Strat()
    {
        dollStatus = GetComponent<DollStatus>();
        if (dollStatus==null)
        {
            Debug.LogError("Missing DollStatus");
        }
        if (dollAnimationController == null)
        {
            Debug.LogError("MIssing DollAnimationController");
        }
        //dollAnimationController.SetStatus(dollStatus);
        
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
        Debug.Log("MoveSpeed : " + moveSpeed);
        IdleBehavior.Update(this, IdleBehavior);
        
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
            dollAnimationController.CancelAnimation();
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
    public DollStatus GetStatus()
    {
        if (dollStatus == null)
        {
            return null;
        }
        else
        {
            return dollStatus;
        }
    }

    public void Hit(float Power)
    {
        IdleBehavior.PushSuccessorState(bvSlow);
        dollAnimationController.PlayHitAnimation();
    }
    #endregion

    public void StartCoroutineSlow()
    {
        StartCoroutine("Slow", 5);
    }

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



    IEnumerator Slow(float slowTime)
    {
        
        float curTime = Time.time;

        while (true)
        {
            moveSpeed *= 0.8f;
            yield return new WaitForSeconds(slowTime);
            if (Time.time - curTime >= slowTime)
            {
                moveSpeed = dollStatus.MoveSpeed;
                break;
            }
        }
    }

    IEnumerator Bleeding(float bleedingTime)
    {
        float curTime = Time.time;
        while (true)
        {
            
            yield return new WaitForSeconds(bleedingTime);
            if (Time.time - curTime >= bleedingTime)
            {
                break;
            }
        }
    }

}
