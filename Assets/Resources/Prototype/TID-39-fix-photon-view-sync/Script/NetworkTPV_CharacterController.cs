using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;
using GHJ_Lib;

public class NetworkTPV_CharacterController : TestPlayerController,IPunObservable
{
    #region Public Fields
    public DollAnimationController dollAnimationController;
    public DollStatus dollStatus;
    #endregion

    #region Private Fieldss
    protected streamVector3 sVector3;
    protected Behavior<NetworkTPV_CharacterController> curBehavior = new Behavior<NetworkTPV_CharacterController>();
    protected BvNormal bvNormal = new BvNormal();
    protected BvSlow bvSlow = new BvSlow();
    protected BvBlind bvBlind = new BvBlind();
    protected BvExpose bvExpose = new BvExpose();
    [SerializeField]
    protected SkinnedMeshRenderer skinnedMeshRenderer;
    protected Material originMaterial;
    #endregion

    #region Protected Fields
    #endregion



    #region MonoBehaviour CallBacks
    public override void OnEnable()
    {
        //dollStatus = GetComponent<DollStatus>();
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


        curBehavior.PushSuccessorState(bvNormal);
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

        curBehavior.Update(this, ref curBehavior);

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
        curBehavior.PushSuccessorState(bvSlow);
        dollAnimationController.PlayHitAnimation();
    }
    #endregion
    public void ExposedByExorcist()
    {
        if (curBehavior == bvNormal)
        { 
            curBehavior.PushSuccessorState(bvExpose);
            dollAnimationController.PlayHitAnimation();
        }
    }
    public void StartCoroutineSlow()
    {
        StartCoroutine("Slow", 5);
    }

    public void StartCoroutineExpose()
    {
        if (GameManager.Instance.Data.Role == DEM.RoleType.Exorcist)
        {
            StartCoroutine("Expose", 10);
        }
    }

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

        if (vertical != 0 || horizontal != 0)
        {
            dollAnimationController.IsMove = true;
        }
        else
        {
            dollAnimationController.IsMove = false;
        }



        if (Input.GetKeyDown(KeyCode.LeftShift))
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

        controller.SimpleMove(direction * moveSpeed);

    }

    #endregion


    #region Private Methods
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
                curBehavior.PushSuccessorState(bvNormal);
                break;
            }
        }
    }

    IEnumerator Expose(float ExposeTime)
    {
        float curTime = Time.time;
        while (true)
        {
            originMaterial = skinnedMeshRenderer.material;
            skinnedMeshRenderer.material= Resources.Load<Material>("Materials/Always Visible");
            yield return new WaitForSeconds(ExposeTime);
            if (Time.time - curTime >= ExposeTime)
            {
                skinnedMeshRenderer.material = originMaterial;
                skinnedMeshRenderer.material.shader.name = "Universal Render Pipeline/Lit";
                curBehavior.PushSuccessorState(bvNormal);
                break;
            }
        }
        
    }

}
