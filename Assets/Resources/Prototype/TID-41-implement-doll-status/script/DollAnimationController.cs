using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
namespace GHJ_Lib
{ 
    public class DollAnimationController : MonoBehaviourPunCallbacks,IPunObservable
    {

        #region Public Fields
        public bool IsMove
        {
            get { return isMove; }
            set { isMove = value; }
        }
        public bool IsRolle
        {
            get { return isRoll; }
            set { isRoll = value; }
        }
        #endregion

        #region Private Fields
        private Animator animator;
        private DollStatus dollStatus=null;
        private bool isMove=false;
        private bool isRoll=false;
        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            if (dollStatus == null)
            {
                return;
            }

            animator.SetBool("Walk", isMove);
            animator.SetBool("Roll", isRoll);
        }
        #endregion


        #region MonoBehaviour Pun CallBacks


        #endregion

        #region Public Methods
        public void SetStatus(DollStatus dollStatus)
        {
            this.dollStatus = dollStatus;
        }
        #endregion

        #region Private Methods
        #endregion

        #region IPunObservable
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(isMove);
                stream.SendNext(isRoll);
            }
            if (stream.IsReading)
            {
                this.isMove = (bool)stream.ReceiveNext();
                this.isRoll = (bool)stream.ReceiveNext();
            }
        }
        #endregion
    }
}
