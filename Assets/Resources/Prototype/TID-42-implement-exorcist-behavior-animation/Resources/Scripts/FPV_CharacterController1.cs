using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using GHJ_Lib;
namespace TID42
{
    public class FPV_CharacterController1 : MonoBehaviourPunCallbacks,IPunObservable
    {
        #region Public Field
        //[SerializeField]
        //private float walkSpeed = 3.0f;
        [SerializeField]
        protected float rotSpeed;
        public ExorcistStatus exorcistStatus = new ExorcistStatus();
        public GameObject target;
        #endregion

        #region Protected Fields
        //Rigidbody rd;
        protected Vector3 moveVector;
        protected CharacterController controller;
        protected PFV_CharacterAnimation animator;
        protected streamVector3 sVector3;
        #endregion

        #region MonoBehaviour Callbacks
        protected virtual void Awake()
        {
            //rd = GetComponent<Rigidbody>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        protected virtual void Start()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponent<PFV_CharacterAnimation>();
        }
        protected virtual void Update()
        {
            if (photonView.IsMine)
            {
                Attack();
                Skill();
            }

            animator.Attack(exorcistStatus.isAttack);
            animator.Skill(exorcistStatus.isSkill);
        }
        protected virtual void FixedUpdate()
        {
            if (photonView.IsMine)
            { 
                Movement();
            }
            controller.SimpleMove(moveVector * exorcistStatus.moveSpeed);
            animator.Move(moveVector.magnitude);
        }


        #endregion

        #region Public Methods
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

    }
}
