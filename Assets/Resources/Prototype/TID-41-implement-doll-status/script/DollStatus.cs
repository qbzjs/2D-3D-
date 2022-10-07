using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
namespace GHJ_Lib
{ 
    public class DollStatus :MonoBehaviourPunCallbacks,IPunObservable
    {
        #region Public Fields
        public float MoveSpeed
        {
            get { return moveSpeed; }
        }
        public float InteractionSpeed
        {
            get { return interactionSpeed; }
        }
        public float ProjectileSpeed
        {
            get { return projectileSpeed; }
        }
        public float DollHitPoint
        {
            get { return dollHitPoint; }
        }
        public float DevilHitPoint
        {
            get { return devilHitPoint; }
        }

        #endregion	

        #region Private Fields
        [Header("All Character")]
        [SerializeField]
        private float moveSpeed;
        [SerializeField]
        private float interactionSpeed;
        [SerializeField]
        private float projectileSpeed;
        [Header("Only Doll")]
        [SerializeField]
        private float dollHitPoint;
        [SerializeField]
        private float devilHitPoint;
        #endregion

        private void Awake()
        {
            // DollType dollType = GameManager.Instance.Data.GetType
            DollType dollType = DollType.Rabbit; //юс╫ц
            switch (dollType)
            {
                case DollType.Rabbit:
                    {
                        this.moveSpeed = 6.0f;
                        this.interactionSpeed = 2.0f;
                        this.projectileSpeed = 10.0f;
                        this.dollHitPoint = 40;
                        this.devilHitPoint = 50;
                    }
                    break;
                default:
                    {
                        this.moveSpeed = 10.0f;
                        this.interactionSpeed = 1.0f;
                        this.projectileSpeed = 10.0f;
                        this.dollHitPoint = 50;
                        this.devilHitPoint = 50;
                    }
                    break;
            }
        }

        #region Public Methods

        public void Move(float moveSpeed)
        {
            this.moveSpeed=moveSpeed;
        }

        public void HitDollHP(int Damage)
        {
            dollHitPoint -= Damage;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(moveSpeed);
                stream.SendNext(interactionSpeed);
                stream.SendNext(projectileSpeed);
                stream.SendNext(dollHitPoint);
                stream.SendNext(devilHitPoint);
            }
            if (stream.IsReading)
            {
                this.moveSpeed =(float)stream.ReceiveNext();
                this.interactionSpeed = (float)stream.ReceiveNext();
                this.projectileSpeed = (float)stream.ReceiveNext();
                this.dollHitPoint = (float)stream.ReceiveNext();
                this.devilHitPoint = (float)stream.ReceiveNext();

            }
        }

        #endregion

        #region Private Methods
        #endregion

    }
}
