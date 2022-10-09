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
        public float MoveSpeed{get;private set;}
        public float InteractionSpeed { get; private set; }
        public float ProjectileSpeed { get; private set; }
        public float DollHitPoint { get; private set; }
        public float DevilHitPoint { get; private set; }
        public float MaxDollHitPoint { get; private set; }
        public float MaxDevilHitPoint { get; private set; }
        public float CurrentRateOfDollHP { get { return DollHitPoint / MaxDollHitPoint; } }
        public float CurrentRateOfDevilHP { get { return DevilHitPoint / MaxDevilHitPoint; } }
        #endregion	

        #region Private Fields
        #endregion

        private void Awake()
        {
            // DollType dollType = GameManager.Instance.Data.GetType
            DollType dollType = DollType.Rabbit; //юс╫ц
            switch (dollType)
            {
                case DollType.Rabbit:
                    {
                        this.MoveSpeed = 6.0f;
                        this.InteractionSpeed = 2.0f;
                        this.ProjectileSpeed = 10.0f;
                        this.DollHitPoint = 40;
                        this.DevilHitPoint = 50;
                        MaxDollHitPoint = DollHitPoint;
                        MaxDevilHitPoint = DevilHitPoint;
                    }
                    break;
                default:
                    {
                        this.MoveSpeed = 10.0f;
                        this.InteractionSpeed = 1.0f;
                        this.ProjectileSpeed = 10.0f;
                        this.DollHitPoint = 50;
                        this.DevilHitPoint = 50;
                        MaxDollHitPoint = DollHitPoint;
                        MaxDevilHitPoint = DevilHitPoint;
                    }
                    break;
            }

         
        }

        public void Start()
        {
            GameObject UIobj = GameObject.Find("DollUI");
            if (UIobj == null)
            {
                Debug.LogError("Missin UI");
                return;
            }
            DollUI dollUI = UIobj.GetComponent<DollUI>();

            if (photonView.IsMine)
            {
                dollUI.SetStatus(this);
            }
            else
            {
                dollUI.SetFriendStatus(this);
            }
        }

        #region Public Methods

        public void Move(float moveSpeed)
        {
            this.MoveSpeed=moveSpeed;
        }

        public void HitDollHP(int Damage)
        {
            DollHitPoint -= Damage;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(MoveSpeed);
                stream.SendNext(InteractionSpeed);
                stream.SendNext(ProjectileSpeed);
                stream.SendNext(DollHitPoint);
                stream.SendNext(DevilHitPoint);
                stream.SendNext(MaxDollHitPoint);
                stream.SendNext(MaxDevilHitPoint);
            }
            if (stream.IsReading)
            {
                this.MoveSpeed =(float)stream.ReceiveNext();
                this.InteractionSpeed = (float)stream.ReceiveNext();
                this.ProjectileSpeed = (float)stream.ReceiveNext();
                this.DollHitPoint = (float)stream.ReceiveNext();
                this.DevilHitPoint = (float)stream.ReceiveNext();
                this.MaxDollHitPoint = (float)stream.ReceiveNext();
                this.MaxDevilHitPoint = (float)stream.ReceiveNext();

            }
        }

        #endregion

        #region Private Methods
        #endregion

    }
}
