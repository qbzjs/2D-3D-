using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib{
 //Is it for only test. do not use main game scene
    public class InteractionTest : MonoBehaviourPunCallbacks, IPunObservable
    {
        PhotonView pv;
        int number;
        private void Start()
        {
            pv = GetComponent<PhotonView>();
        }
        private void OnTriggerStay(Collider other)
        {
            if(Input.GetKey(KeyCode.Mouse0))
            {
                if(photonView.IsMine)
                {
                    AddFive();
                }
                else
                {
                    pv.RPC("AddFive", RpcTarget.MasterClient);
                }
            }
        }
        private void OnGUI()
        {
            GUI.Box(new Rect(0, 0, 300, 60), number.ToString());
        }
        void AddFive()
        {
            number += 5;
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(number);
            }
            else
            {
                // Network player, receive data
                this.number = (int)stream.ReceiveNext();
            }
        }
    }
}
