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
        Rigidbody rd;
        protected CharacterController controller;
        protected PFV_CharacterAnimation animator;
        protected streamVector3 sVector3;
        #endregion

        #region MonoBehaviour Callbacks
        protected virtual void Awake()
        {
            rd = GetComponent<Rigidbody>();
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

            animator.Move(rd.velocity.magnitude);
        }
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        protected virtual void Movement()
        {
            Vector2 currentInput = FPV_InputManager.instance.GetPlayerMove() * exorcistStatus.moveSpeed;
            Vector3 dir = new Vector3(-Camera.main.transform.right.z, 0f,Camera.main.transform.right.x);
            Vector3 movement = (dir * currentInput.y + Camera.main.transform.right * currentInput.x + Vector3.up * rd.velocity.y);
            transform.rotation = Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0);
            rd.velocity = movement;
            
            
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
                sVector3.x = rd.velocity.x;
                sVector3.y = rd.velocity.y;
                sVector3.z = rd.velocity.z;
                stream.SendNext(sVector3);
            }

            if (stream.IsReading)
            {
                this.exorcistStatus.isSkill = (bool)stream.ReceiveNext();
                this.exorcistStatus.isAttack = (bool)stream.ReceiveNext();
                this.sVector3.x = (float)stream.ReceiveNext();
                this.sVector3.y = (float)stream.ReceiveNext();
                this.sVector3.z = (float)stream.ReceiveNext();

                this.rd.velocity = new Vector3(sVector3.x, sVector3.y, sVector3.z);
            }
        }

        #endregion

    }
}
