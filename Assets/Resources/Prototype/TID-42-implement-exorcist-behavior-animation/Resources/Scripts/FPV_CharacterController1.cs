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
        private float rotSpeed;
        public ExorcistStatus exorcistStatus = new ExorcistStatus();
        public GameObject target;
        #endregion

        #region Private Fields
        Rigidbody rd;
        private CharacterController controller;
        
        private PFV_CharacterAnimation animator;
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
        private void Update()
        {
            Attack();
            Skill();
        }
        protected virtual void FixedUpdate()
        {
            
            Movement();
            Rotation();
        }
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        private void Movement()
        {
            Vector2 currentInput = FPV_InputManager.instance.GetPlayerMove() * exorcistStatus.moveSpeed;
            Vector3 dir = new Vector3(-Camera.main.transform.right.z, 0f,Camera.main.transform.right.x);
            Vector3 movement = (dir * currentInput.y + Camera.main.transform.right * currentInput.x + Vector3.up * rd.velocity.y);
            transform.rotation = Quaternion.Euler(0, target.transform.rotation.eulerAngles.y, 0);
            rd.velocity = movement;
            
            animator.Move(rd.velocity.magnitude);
        }
        private void Rotation()
        {

        }
        private void Attack()
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                exorcistStatus.isAttack = true;
                animator.Attack(exorcistStatus.isAttack);
            }
            else
            {
                exorcistStatus.isAttack = false;
                animator.Attack(exorcistStatus.isAttack);
            }
        }
        private void Skill()
        {
            if (Input.GetKey(KeyCode.F))
            {
                exorcistStatus.isSkill = true;
                animator.Skill(exorcistStatus.isSkill);
            }
            else
            {
                exorcistStatus.isSkill = false;
                animator.Skill(exorcistStatus.isSkill);
            }
        }
        #endregion
    }
}
