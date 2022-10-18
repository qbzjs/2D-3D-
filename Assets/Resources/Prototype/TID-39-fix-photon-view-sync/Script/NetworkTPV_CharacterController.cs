using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;
using GHJ_Lib;
using LSH_Lib;
using Cinemachine;
public class NetworkTPV_CharacterController : TestPlayerController,IPunObservable
{
    #region Public Fields
    public DollAnimationController dollAnimationController;
    public DollStatus dollStatus;
    public CinemachineVirtualCamera cinemachineVirtual;
    public Network_TPV_CameraController network_TPV_CameraController;
    public Behavior<NetworkTPV_CharacterController> CurBehavior { get { return curBehavior; } }
    public GameObject CamTarget;
    public GameObject CharacterModel;
    #endregion

    #region Private Fieldss
    protected streamVector3 sVector3;
    protected Behavior<NetworkTPV_CharacterController> curBehavior = new Behavior<NetworkTPV_CharacterController>();
    protected BvNormal bvNormal = new BvNormal();
    protected BvSlow bvSlow = new BvSlow();
    protected BvBlind bvBlind = new BvBlind();
    protected BvExpose bvExpose = new BvExpose();
    protected BvGhost bvGhost = new BvGhost();
    protected BvFall bvFall = new BvFall();
    protected BvGrabbed bvGrabbed = new BvGrabbed();
    protected BvImprison bvImplement = new BvImprison();
    [SerializeField]
    protected SkinnedMeshRenderer skinnedMeshRenderer;
    protected Material originMaterial;

    protected bool isCanMove = true;
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
        GameObject virtualCamera = GameObject.Find("VirtualPlayerCamera");
        if (virtualCamera == null)
        {
            Debug.LogError("MIssing virtualCamera");
        }
        cinemachineVirtual = virtualCamera.gameObject.GetComponent<CinemachineVirtualCamera>();

        network_TPV_CameraController = virtualCamera.GetComponent<Network_TPV_CameraController>();

        moveSpeed = dollStatus.MoveSpeed; //최종 스피드는 이동속도*상태*디버프 


        curBehavior.PushSuccessorState(bvNormal);
        base.Start();
    }


    protected override void Update()
    {
        if (curBehavior is BvGrabbed)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                photonView.RPC("EscapeGrab", RpcTarget.All);
            }
        }

        if (photonView.IsMine&&isCanMove)
        {
            PlayerInput();
            SetDirection();
        }
            RotateToDirection();
            MoveCharacter();
        Debug.Log("MoveSpeed : " + moveSpeed);

        curBehavior.Update(this, ref curBehavior);
        dollAnimationController.UpdateHP_Rate();
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
        //curBehavior.PushSuccessorState(bvSlow);

        if (curBehavior is BvFall )
        {
            return;
        }

        dollAnimationController.PlayHitAnimation();
        dollStatus.HitDollHP(Power);

        if (dollStatus.DollHealthPoint <= 0)
        {
            curBehavior.PushSuccessorState(bvFall);
        }

        if (dollStatus.DevilHealthPoint <= 0)
        {
            curBehavior = bvGhost;
        }   

    }
   
    public void ExposedByExorcist()
    {
        if (curBehavior is BvNormal)
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
            StartCoroutine("Expose", 40);
        }
    }

    public void BecomeGhost()
    {
        if (GameManager.Instance.Data.Role == DEM.RoleType.Doll)
        {
            skinnedMeshRenderer.material = Resources.Load<Material>("Materials/Ghost");
        }
        else
        {
            skinnedMeshRenderer.material = Resources.Load<Material>("Materials/Invisible");
        }

    }

    public void FallDown()
    {
        isCanMove = false;
        dollAnimationController.PlayFallDownAnimation();
    }

    public void Grabbed(GameObject exorcistCamTarget)
    {
        Debug.Log("Grabbed");
        //photonView.ObservedComponents.Remove(photonTransform);
        //controller.enabled = false;
        if (GameManager.Instance.Data.Role == DEM.RoleType.Doll)
        { 
            cinemachineVirtual.Follow = exorcistCamTarget.transform;
            network_TPV_CameraController.SetCamTarget(exorcistCamTarget);
        }

        CharacterModel.SetActive(false);
        curBehavior.PushSuccessorState(bvGrabbed);
    }
    public void Imprison()
    {
        Debug.Log("The doll imprisons");
        isCanMove = false;
        float Power = 5.0f;
        CharacterModel.SetActive(true);

        if (GameManager.Instance.Data.Role == DEM.RoleType.Doll)
        {
            cinemachineVirtual.Follow = CamTarget.transform;
            network_TPV_CameraController.SetCamTarget(CamTarget);
        }

        dollAnimationController.PlayHitAnimation();
        dollStatus.HitDollHP(Power);

        curBehavior.PushSuccessorState(bvImplement);
    }
    public void SetPosition(Vector3 position)
    {
        this.gameObject.transform.position = position;
    }
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
        if (controller.enabled == false)
        {
            return;
        }
        controller.SimpleMove(direction * moveSpeed);

    }

    [PunRPC]
    protected void EscapeGrab()
    {
        
        isCanMove = true;
        if (GameManager.Instance.Data.Role == DEM.RoleType.Doll)
        {
            cinemachineVirtual.Follow = CamTarget.transform;
            network_TPV_CameraController.SetCamTarget(CamTarget);
        }
        CharacterModel.SetActive(true);
        curBehavior.PushSuccessorState(bvNormal);
        
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
        moveSpeed *= 0.8f;
        yield return new WaitForSeconds(slowTime);
        moveSpeed = dollStatus.MoveSpeed;
        curBehavior.PushSuccessorState(bvNormal);
    }

    IEnumerator Expose(float ExposeTime)
    {
        originMaterial = skinnedMeshRenderer.material;
        skinnedMeshRenderer.material= Resources.Load<Material>("Materials/Always Visible");
        yield return new WaitForSeconds(ExposeTime);
        skinnedMeshRenderer.material = originMaterial;
        curBehavior.PushSuccessorState(bvNormal);
    }

}
