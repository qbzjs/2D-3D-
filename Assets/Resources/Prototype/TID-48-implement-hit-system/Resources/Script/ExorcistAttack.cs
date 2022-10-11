using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TID42;
using Photon.Pun;

namespace GHJ_Lib
{ 
    public class ExorcistAttack : MonoBehaviour,IPunObservable
    {
        #region Public Fields
        public GameObject[] AttackArea;
        public FPV_CharacterController1 FPV_characterController1;
        #endregion

        #region Protected Fields
        protected bool isAttacking=false;
        protected Animator animator;
        #endregion


        #region Private Fields

        #endregion



        #region MonoBehaviour CallBacks
        void Awake()
        {
            animator = GetComponent<Animator>();
            FPV_characterController1 = GetComponent<FPV_CharacterController1>();
        }
        void Update()
        {
            isAttacking=GetIsAttack();

        }

        #endregion

        #region Public Methods
        public void EnableAttackArea(int index)
        {
            AttackArea[index].SetActive(true);
        }

        public void DisableAttackArea(int index)
        {
            AttackArea[index].SetActive(false);
        }

        public void EnableAttackBoxArea()
        {
            AttackArea[2].SetActive(true);
        }

        public void DisableAttackBoxArea()
        {
            AttackArea[2].SetActive(false);
        }

        public void EnableAttackSkillArea()
        {
            AttackArea[3].SetActive(true);
        }
        public void DisableAttackSkillArea()
        {
            AttackArea[3].SetActive(false);
        }


        public bool GetIsAttack()
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Doll"))
            {
                if (AttackArea[2].gameObject.activeInHierarchy)
                { 
                Debug.Log("Attack Doll");
                other.GetComponent<NetworkTPV_CharacterController>().Hit(FPV_characterController1.exorcistStatus.offensePower);
                DisableAttackBoxArea();
                }

                if (AttackArea[3].gameObject.activeInHierarchy)
                {
                    Debug.Log("Use Skill Expose");
                    other.GetComponent<NetworkTPV_CharacterController>().ExposedByExorcist();
                }
            }
        }
        #endregion

        #region Protected
        #endregion

        #region Private Methods

        #endregion

        #region IPunObservable
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(isAttacking);
            }
            if (stream.IsReading)
            {
                this.isAttacking = (bool)stream.ReceiveNext();
            }
        }
        #endregion
    }
}
