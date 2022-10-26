using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib
{
    public abstract class Item : MonoBehaviourPunCallbacks, IPunObservable
    {
        ItemData data;

        virtual protected void Start()
        {
            InitItemData();
        }
        protected abstract void InitItemData();

        virtual protected void DoAction(){ }

        public abstract void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
    }
}
