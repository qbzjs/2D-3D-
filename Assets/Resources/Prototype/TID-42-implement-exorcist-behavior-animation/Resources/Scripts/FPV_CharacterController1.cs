using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using GHJ_Lib;
using KSH_Lib;
namespace TID42
{
    public class FPV_CharacterController1 : MonoBehaviourPunCallbacks,IPunObservable
    {
        #region Public Field
        //[SerializeField]
        //private float walkSpeed = 3.0f;
        [SerializeField]
        protected float rotSpeed;
        [SerializeField]
        protected float finalMoveSpeed;
        [SerializeField]
        protected float finalAttackSpeed;
        public ExorcistStatus exorcistStatus = new ExorcistStatus();
        public GameObject target;
        public ParticleSystem Ayra;
        #endregion

        #region Protected Fields
        //Rigidbody rd;
        protected Vector3 moveVector;
        protected CharacterController controller;
        protected PFV_CharacterAnimation animator;
        protected streamVector3 sVector3;
        protected ExorcistUI exorcistUI;
        protected float rageTime = 40.0f;
        //----BvState----//
        protected Behavior<FPV_CharacterController1> curBehavior = new Behavior<FPV_CharacterController1>();
        protected BvNormalExorcist bvNormalExorcist = new BvNormalExorcist();
        protected BvRage bvRage = new BvRage();
        protected BvFastExorcist bvFast = new BvFastExorcist();
        protected BvSlowExorcist bvSlow  = new BvSlowExorcist();
        protected BvAttackSpeedUp bvAttackSpeedUp = new BvAttackSpeedUp();
        protected BvAttackSpeedDown bvAttackSpeedDown = new BvAttackSpeedDown();
        protected BvInvisibleExorcist bvInvisibleExorcist = new BvInvisibleExorcist();
        //---------------//

        #endregion

        #region MonoBehaviour Callbacks
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
                Movement();
                Attack();
                Skill();
            }

            animator.Attack(exorcistStatus.isAttack);
            animator.Skill(exorcistStatus.isSkill);
            animator.Move(moveVector.magnitude);
            controller.SimpleMove(moveVector * finalMoveSpeed);
            curBehavior.Update(this, ref curBehavior);

            if (curBehavior == bvRage)
            {
                bvRage.RageDuration -= Time.deltaTime;
                exorcistUI.CoolTime(bvRage.RageDuration/rageTime);
            }

        }

        #endregion

        #region Public Methods
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
            StartCoroutine(MoveFast(duration,fastRatio));
        }
        public void StartCoroutineMoveSlow(float duration, float fastRatio)
        {
            StartCoroutine(MoveSlow(duration, fastRatio));
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
        #endregion

        #region Protected Methods
        protected virtual void Movement()
        {
            Vector2 currentInput = FPV_InputManager.instance.GetPlayerMove();
            Vector3 dir = new Vector3(-Camera.main.transform.right.z, 0f,Camera.main.transform.right.x);
            Vector3 movement = (dir * currentInput.y + Camera.main.transform.right * currentInput.x );
            transform.rotation = Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0);
            moveVector = movement.normalized;
            
            
        }
        protected virtual void Attack()
        {
            if(Input.GetKey(KeyCode.Mouse0))
            {
                exorcistStatus.isAttack = true;
            }
            else
            {
            
                exorcistStatus.isAttack = false;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                Rage();
            }

        }
        protected virtual void Skill()
        {
            if (Input.GetKey(KeyCode.F))
            {

                exorcistStatus.isSkill = true;
            }
            else
            {
                exorcistStatus.isSkill = false;
            }
        }

        #endregion

        #region IPunObservable
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

        #endregion

        #region IEnumrator
        IEnumerator MoveSlow(float duration,float slowRatio)
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
            yield return new WaitForSeconds(rageTime);
            Ayra.Stop();
            animator.Rage(false);
            finalAttackSpeed -= exorcistStatus.attackSpeed * 0.5f;
            finalMoveSpeed -= exorcistStatus.moveSpeed * 0.5f;
            curBehavior.PushSuccessorState(bvNormalExorcist);
        }

        #endregion
    }
}
