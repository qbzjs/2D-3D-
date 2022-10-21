using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;
using LSH_Lib;

namespace GHJ_Lib
{
	public class NetworkExorcistController: MonoBehaviourPunCallbacks, IPunObservable
    {
        /*--- Public Fields ---*/
        public ExorcistStatus exorcistStatus = new ExorcistStatus();
        public GameObject target;
        public ParticleSystem Ayra;
        public AnimationCallbackSet animationCallbackSet;
        public GameObject[] GrabObj;
        public Behavior<NetworkExorcistController> CurBehavior { get { return curBehavior; } }
        /*--- Protected Fields ---*/
        protected float rotSpeed;
        protected float finalMoveSpeed;
        protected float finalAttackSpeed;
        protected Vector3 moveVector;
        protected CharacterController controller;
        protected PFV_CharacterAnimation animator;
        protected streamVector3 sVector3;
        protected ExorcistUI exorcistUI;
        protected float rageTime = 40.0f;
        protected bool isRage = false;
        protected List<GameObject> PickUpList = new List<GameObject>();
        protected PhotonTransformViewClassic photonTransformView;
        protected GameObject inHandDoll = null;
        //----BvState----//
        protected Behavior<NetworkExorcistController> curBehavior = new Behavior<NetworkExorcistController>();
        protected BvNormalExorcist bvNormalExorcist = new BvNormalExorcist();
        protected BvRage bvRage = new BvRage();
        protected BvFastExorcist bvFast = new BvFastExorcist();
        protected BvSlowExorcist bvSlow = new BvSlowExorcist();
        protected BvAttackSpeedUp bvAttackSpeedUp = new BvAttackSpeedUp();
        protected BvAttackSpeedDown bvAttackSpeedDown = new BvAttackSpeedDown();
        protected BvInvisibleExorcist bvInvisibleExorcist = new BvInvisibleExorcist();
        protected BvGrab bvGrab = new BvGrab();
        /*--- Private Fields ---*/


        //---interaction Fields---//
        public float CastingVelocity
        {
            get { return castingVelocity; }
        }
        protected float castingVelocity = 2.0f;
        
        protected bool canInteract = false;
        protected bool isInteract = false;
        //

        /*--- MonoBehaviour Callbacks ---*/
        protected virtual void Awake()
        {
            //rd = GetComponent<Rigidbody>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        public override void OnEnable()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<PFV_CharacterAnimation>();
            curBehavior.PushSuccessorState(bvNormalExorcist);
            finalMoveSpeed = exorcistStatus.moveSpeed;
            photonTransformView = GetComponent<PhotonTransformViewClassic>();
            GameObject exUI = GameObject.Find("ExorcistUI");
            if (exUI == null)
            {
                return;
            }
            exorcistUI = exUI.GetComponent<ExorcistUI>();
            if (exorcistUI == null)
            {
                Debug.LogError("Missing ExorcistUI");
            }
        }
        protected virtual void Update()
        {
            if (photonView.IsMine)
            {
                InteractObj();
                Movement();
                Attack();
                Skill();
                if (isRage)
                {
                    bvRage.RageDuration -= Time.deltaTime;
                    exorcistUI.CoolTime(bvRage.RageDuration / rageTime);
                }
                var velocity = controller.velocity;
                var turnSpeed = rotSpeed;
                photonTransformView.SetSynchronizedValues(velocity, turnSpeed);
            }


            animator.Attack(exorcistStatus.isAttack);
            animator.Skill(exorcistStatus.isSkill);
            animator.Move(moveVector.magnitude);
            controller.SimpleMove(moveVector * finalMoveSpeed);
            curBehavior.Update(this, ref curBehavior);

            

        }

        // >> interaction 
        private void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if (animationCallbackSet.AttackArea[3].activeInHierarchy)
            {
                return;
            }

            if (other.CompareTag("interactObj"))
            {
                

                if (SceneManager.Instance.IsCoroutine)
                {
                    return;
                }

                LSH_Lib.interaction obj = other.GetComponent<LSH_Lib.interaction>();
                if (Vector3.Dot(Vector3.ProjectOnPlane(obj.transform.position - this.transform.position,Vector3.up), this.transform.forward) < 90.0f
                    && obj.CanActiveToExorcist)
                {
                    canInteract = true;
                   // Debug.Log(obj.name);
                    //Debug.Log("canInteract: "+ canInteract);
                    SceneManager.Instance.EnableInteractionText();
                    SceneManager.Instance.DisableCastingBar();
                }
                else
                {
                    canInteract = false;
                    //Debug.Log("canInteract: " + canInteract);
                    SceneManager.Instance.DisableInteractionText();
                    SceneManager.Instance.DisableCastingBar();
                    return;
                }

                if (isInteract)
                {
                    SceneManager.Instance.DisableInteractionText();
                    SceneManager.Instance.EnableCastingBar(other.gameObject);
                    obj.Interact(gameObject.tag, this);
                }
                else
                {
                    
                    SceneManager.Instance.EnableInteractionText();
                    SceneManager.Instance.DisableCastingBar();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (other.CompareTag("interactObj"))
            {

                canInteract = false;
                SceneManager.Instance.DisableInteractionText();
                SceneManager.Instance.DisableCastingBar();
                
            }
        }
        // <<

        /*--- Public Methods ---*/
        public void Fast(float fastRatio)
        {
            bvFast.Ratio = fastRatio;
            curBehavior.PushSuccessorState(bvFast);
        }
        public void Slow(float slowRatio)
        {
            bvSlow.Ratio = slowRatio;
            curBehavior.PushSuccessorState(bvSlow);
        }
        [PunRPC]
        public void Rage()
        {
            if (curBehavior == bvRage)
            {
                return;
            }

            curBehavior.PushSuccessorState(bvRage);
        }
        public void StartCoroutineMoveFast(float duration, float fastRatio)
        {
            StartCoroutine(MoveFast(duration, fastRatio));
        }
        public void StartCoroutineMoveSlow(float duration, float slowRatio)
        {
            StartCoroutine(MoveSlow(duration, slowRatio));
        }
        public void StartCoroutineOnRage()
        {
            StartCoroutine(OnRage());
        }
        public void StartCoroutineAttackSpeedUp()
        {
            StartCoroutine("AttackSpeedUp", 5);

        }
        public void StartCoroutineAttackSpeedDown()
        {
            StartCoroutine("AttackSpeedDown", 5);
        }
        public void StartCoroutineInvisibleExorcist()
        { }
        public void AddPickUpList(GameObject doll)
        {
            if (PickUpList.Contains(doll))
            {
                return;
            }
            Debug.Log("Add " + doll);
            PickUpList.Add(doll);
        }

        public void PopPickUpList(GameObject doll)
        {

            Debug.Log("Remove " + doll);
            PickUpList.Remove(doll);
        }
        public void MissDoll()
        {

            photonView.RPC("BecomeNormal", RpcTarget.All);
        }


        public void ActiveGrabObj(int i)
        {
            GrabObj[i].SetActive(true);
            inHandDoll = GrabObj[i];
        }

        public void InActive()
        {
            if (inHandDoll)
            {
                inHandDoll.SetActive(false);
                inHandDoll = null;
            }
        }
            /*---IPunObseve---*/
            public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
            {
                if (stream.IsWriting)
                {
                    stream.SendNext(exorcistStatus.isSkill);
                    stream.SendNext(exorcistStatus.isAttack);

                    sVector3.x = moveVector.x;
                    sVector3.y = moveVector.y;
                    sVector3.z = moveVector.z;
                    stream.SendNext(sVector3.x);
                    stream.SendNext(sVector3.y);
                    stream.SendNext(sVector3.z);
                }

                if (stream.IsReading)
                {
                    this.exorcistStatus.isSkill = (bool)stream.ReceiveNext();
                    this.exorcistStatus.isAttack = (bool)stream.ReceiveNext();
                    this.sVector3.x = (float)stream.ReceiveNext();
                    this.sVector3.y = (float)stream.ReceiveNext();
                    this.sVector3.z = (float)stream.ReceiveNext();
                    this.moveVector = new Vector3(sVector3.x, sVector3.y, sVector3.z);

                }
            }


        /*--- Protected Methods ---*/
        protected virtual void Movement()
        {
            Vector2 currentInput = FPV_InputManager.instance.GetPlayerMove();
            Vector3 dir = new Vector3(-Camera.main.transform.right.z, 0f, Camera.main.transform.right.x);
            Vector3 movement = (dir * currentInput.y + Camera.main.transform.right * currentInput.x);
            transform.rotation = Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0);
            moveVector = movement.normalized;


        }
        protected virtual void Attack()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                exorcistStatus.isAttack = true;
            }
            else
            {

                exorcistStatus.isAttack = false;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                photonView.RPC("Rage", RpcTarget.All);
            }

        }
        protected virtual void Skill() //스킬 x 들어올리기 행동
        {
            if (PickUpList.Count == 0)
            {
                return;
            }

            if (Input.GetKey(KeyCode.F))
            {
                photonView.RPC("SetPickUpDoll", RpcTarget.All);
                exorcistStatus.isSkill = true;
            }
            else
            {
                exorcistStatus.isSkill = false;
            }
        }

        protected virtual void InteractObj()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                isInteract = true;// << interaction
                return;
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                isInteract = false; // << interaction
                return;
            }
        }

        [PunRPC]
        protected void BecomeNormal()
        {
            curBehavior.PushSuccessorState(bvNormalExorcist);
        }

        [PunRPC]
        protected void SetPickUpDoll()
        {
            animationCallbackSet.PickUpDoll = FindNearestFallDownDoll();
            curBehavior.PushSuccessorState(bvGrab);
        }
        protected GameObject FindNearestFallDownDoll()
        {
            GameObject pickUpDoll = null;
            foreach (GameObject FallDoll in PickUpList)
            {
                if (pickUpDoll == null)
                {
                    pickUpDoll = FallDoll;
                }
                else
                {
                    if ((this.transform.position - pickUpDoll.transform.position).sqrMagnitude >
                        (this.transform.position - FallDoll.transform.position).sqrMagnitude)
                    {
                        pickUpDoll = FallDoll;
                    }

                }
            }

            return pickUpDoll;
        }

        /*--- Private Methods ---*/

        /*---Enumerator---*/
        IEnumerator MoveSlow(float duration, float slowRatio)
        {
            finalMoveSpeed -= exorcistStatus.moveSpeed * slowRatio;

            yield return new WaitForSeconds(duration);
            finalMoveSpeed += exorcistStatus.moveSpeed * slowRatio;
            curBehavior.PushSuccessorState(bvNormalExorcist);
        }
        IEnumerator MoveFast(float duration, float fastRatio)
        {
            finalMoveSpeed += exorcistStatus.moveSpeed * fastRatio;
            yield return new WaitForSeconds(duration);
            finalMoveSpeed -= exorcistStatus.moveSpeed * fastRatio;
            curBehavior.PushSuccessorState(bvNormalExorcist);
        }
        IEnumerator AttackSpeedUp(float duration)
        {
            finalAttackSpeed += exorcistStatus.attackSpeed * 0.2f;
            yield return new WaitForSeconds(duration);
            finalAttackSpeed -= exorcistStatus.attackSpeed * 0.2f;
            curBehavior.PushSuccessorState(bvNormalExorcist);
        }
        IEnumerator AttackSpeedDown(float duration)
        {
            finalAttackSpeed -= exorcistStatus.attackSpeed * 0.2f;
            yield return new WaitForSeconds(duration);
            finalAttackSpeed += exorcistStatus.attackSpeed * 0.2f;
            curBehavior.PushSuccessorState(bvNormalExorcist);
        }

        IEnumerator OnRage()
        {
            Ayra.Play();
            animator.Rage(true);
            finalAttackSpeed += exorcistStatus.attackSpeed * 0.5f;
            finalMoveSpeed += exorcistStatus.moveSpeed * 0.5f;
            bvRage.RageDuration = rageTime;
            isRage = true;
            animationCallbackSet.EnableAttackSkillArea();
            yield return new WaitForSeconds(5.0f);
            animationCallbackSet.DisableAttackSkillArea();
            yield return new WaitForSeconds(rageTime - 5.0f);
            Ayra.Stop();
            animator.Rage(false);
            finalAttackSpeed -= exorcistStatus.attackSpeed * 0.5f;
            finalMoveSpeed -= exorcistStatus.moveSpeed * 0.5f;
            curBehavior.PushSuccessorState(bvNormalExorcist);
            isRage = false;
        }
    }
}