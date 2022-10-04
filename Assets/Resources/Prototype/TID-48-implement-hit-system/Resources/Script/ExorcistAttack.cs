using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TID42;
using Photon.Pun;

namespace GHJ_Lib
{ 
    public class ExorcistAttack : FPV_CharacterController1
    {
        #region Public Fields
        public GameObject[] AttackArea;
        #endregion

        #region Protected Fields
        protected bool isAttacking=false;
        #endregion


        #region Private Fields

        #endregion



        #region MonoBehaviour CallBacks
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            base.Update();
            isAttacking=animator.GetIsAttack();

        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
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

        #endregion

        #region Protected
        #endregion

        #region Private Methods

        #endregion

        #region IPunObservable
        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            base.OnPhotonSerializeView(stream, info);
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
