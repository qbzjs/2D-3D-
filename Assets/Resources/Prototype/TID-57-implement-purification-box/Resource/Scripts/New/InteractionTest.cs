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
        PurificationBoxUI boxUI;
        string playerTag;
        bool number;
        private void Start()
        {
            pv = GetComponent<PhotonView>();
            boxUI = GameObject.Find("BoxUI").GetComponent<PurificationBoxUI>();
            boxUI.TextInvisible();
            boxUI.SliderInvisible();
        }
        private void Update()
        {
            //if(Input.GetKeyUp(KeyCode.G))
            //{
            //    boxUI.TextInvisible();
            //    boxUI.SliderInvisible();
            //}
        }
        private void OnTriggerStay(Collider other)
        {
            playerTag = other.ToString();
            boxUI.TextVisible();
            if(Input.GetKey(KeyCode.G))
            {
                Exorcist();
            }
            //{
            //    if (pv.IsMine)
            //    {
            //        Boxinteract(playerTag);
            //    }
            //    else
            //    {
            //        pv.RPC("Boxinteract", RpcTarget.MasterClient, playerTag);
            //    }
            //}
        }
        
        [PunRPC]
        void Boxinteract(string tag)
        {
            if(tag == "Exorcist")
            {
                if(Input.GetKey(KeyCode.G))
                {
                    Exorcist();
                }
            }
            if(tag == "Doll")
            {
                Doll();
            }
        }
        void Exorcist()
        {
            boxUI.TextInvisible();
            boxUI.Slidervisible();
            boxUI.AutoCasting(1.0f);
        }
        void Doll()
        {

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
                this.number = (bool)stream.ReceiveNext();
            }
        }
    }
}
