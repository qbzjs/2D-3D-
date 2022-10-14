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
        public float DollHealthPoint { get; private set; }
        public float DevilHealthPoint { get; private set; }
        public float MaxDollHitPoint { get; private set; }
        public float MaxDevilHitPoint { get; private set; }
        public float CurrentRateOfDollHP { get { return DollHealthPoint / MaxDollHitPoint; } }
        public float CurrentRateOfDevilHP { get { return DevilHealthPoint / MaxDevilHitPoint; } }
        #endregion	

        #region Private Fields
        #endregion

        private void Awake()
        {
            // DollType dollType = GameManager.Instance.Data.GetType
            DollType dollType = DollType.Rabbit; //�ӽ�
            switch (dollType)
            {
                case DollType.Rabbit:
                    {
                        this.MoveSpeed = 6.0f;
                        this.InteractionSpeed = 2.0f;
                        this.ProjectileSpeed = 10.0f;
                        this.DollHealthPoint = 40;
                        this.DevilHealthPoint = 50;
                        MaxDollHitPoint = DollHealthPoint;
                        MaxDevilHitPoint = DevilHealthPoint;
                    }
                    break;
                default:
                    {
                        this.MoveSpeed = 10.0f;
                        this.InteractionSpeed = 1.0f;
                        this.ProjectileSpeed = 10.0f;
                        this.DollHealthPoint = 50;
                        this.DevilHealthPoint = 50;
                        MaxDollHitPoint = DollHealthPoint;
                        MaxDevilHitPoint = DevilHealthPoint;
                    }
                    break;
            }

         
        }

        public override void OnEnable()
        {

            if (GameManager.Instance.Data.Role == DEM.RoleType.Doll)
            {
                GameObject UIobj = GameObject.Find("DollUI");
                if (UIobj == null)
                {
                    Debug.LogError("Missin DollUI");
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
            else
            {
                GameObject UIobj = GameObject.Find("ExorcistUI");
                if (UIobj == null)
                {
                    Debug.LogError("Missin ExorcistUI");
                    return;
                }
                ExorcistUI exorcistUI = UIobj.GetComponent<ExorcistUI>();
                exorcistUI.SetDollStatus(this);


            }


        }

        #region Public Methods

        public void Move(float moveSpeed)
        {
            this.MoveSpeed=moveSpeed;
        }

        public void HitDollHP(float Damage)
        {
            DollHealthPoint -= Damage;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(MoveSpeed);
                stream.SendNext(InteractionSpeed);
                stream.SendNext(ProjectileSpeed);
                stream.SendNext(DollHealthPoint);
                stream.SendNext(DevilHealthPoint);
                stream.SendNext(MaxDollHitPoint);
                stream.SendNext(MaxDevilHitPoint);
            }
            if (stream.IsReading)
            {
                this.MoveSpeed =(float)stream.ReceiveNext();
                this.InteractionSpeed = (float)stream.ReceiveNext();
                this.ProjectileSpeed = (float)stream.ReceiveNext();
                this.DollHealthPoint = (float)stream.ReceiveNext();
                this.DevilHealthPoint = (float)stream.ReceiveNext();
                this.MaxDollHitPoint = (float)stream.ReceiveNext();
                this.MaxDevilHitPoint = (float)stream.ReceiveNext();

            }
        }

        #endregion

        #region Private Methods
        #endregion

    }
}