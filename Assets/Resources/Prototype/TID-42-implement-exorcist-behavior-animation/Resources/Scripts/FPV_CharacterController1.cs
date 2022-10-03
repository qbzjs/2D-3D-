using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;

namespace TID42
{
    public class FPV_CharacterController1 : MonoBehaviourPunCallbacks
    {
        #region Public Field
        //[SerializeField]
        //private float walkSpeed = 3.0f;
        [SerializeField]
        private float rotSpeed = 2.0f;
        public ExorcistStatus exorcistStatus = new ExorcistStatus();
        #endregion

        #region Private Fields
        Rigidbody rd;
        private CharacterController controller;
        private Vector2 turn;
        private Animator animator;
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
            animator = GetComponent<Animator>();
        }
        private void Update()
        {
            Attack();
            Skill();
        }
        protected virtual void FixedUpdate()
        {
            
            Movement();
            mouseRotate();
            
        }
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        private void Movement()
        {
            Vector2 currentInput = FPV_InputManager.instance.GetPlayerMove() * exorcistStatus.moveSpeed;
            Vector3 dir = new Vector3(currentInput.x, 0f, currentInput.y);
            dir = Camera.main.transform.forward * dir.z + Camera.main.transform.right * dir.x;
            rd.velocity = dir;
            animator.SetFloat("MoveSpeed", rd.velocity.magnitude);
        }
        private void mouseRotate()
        {
            turn.x += Input.GetAxis("Mouse X") * rotSpeed;
            turn.y += Input.GetAxis("Mouse Y") * rotSpeed;
            this.gameObject.transform.localRotation = Quaternion.Euler(0, turn.x, 0);
        }
        private void Attack()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("GetKeyDown");
                exorcistStatus.isAttack = true;
                animator.SetTrigger("Attack");
            }
            exorcistStatus.isAttack = false;
        }
        private void Skill()
        {
            if(Input.GetKey(KeyCode.F))
            {
                exorcistStatus.isSkill = true;
                animator.SetBool("isSkill", exorcistStatus.isSkill);
            }
            else
            {
                exorcistStatus.isSkill = false;
                animator.SetBool("isSkill", exorcistStatus.isSkill);
            }
            
        }
        #endregion
    }
}
