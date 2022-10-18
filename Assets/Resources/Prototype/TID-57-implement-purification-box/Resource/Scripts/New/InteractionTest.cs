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
        bool isEmpty = true;
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
            if (Input.GetKey(KeyCode.G))
            {
                if (other.gameObject.CompareTag("Exorcist"))
                {
                    Exorcist();
                    if (pv.IsMine)
                    {
                        Boxinteract(playerTag);
                    }
                    //else
                    //{
                    //    pv.RPC("Boxinteract", RpcTarget.MasterClient, playerTag);
                    //}
                }
                else
                {
                    Doll();
                    //if (pv.IsMine)
                    //{
                    //    Boxinteract(playerTag);
                    //}
                    if(!pv.IsMine)
                    {
                        pv.RPC("Boxinteract", RpcTarget.MasterClient, playerTag);
                    }
                }

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
        private void OnGUI()
        {
            GUI.Box(new Rect(0, 0, 150, 30), isEmpty.ToString());
        }
        [PunRPC]
        void Boxinteract(string tag)
        {
            if(tag == "Exorcist")
            {
                isEmpty = false;
            }
            if(tag == "Doll")
            {
                isEmpty = true;
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
            boxUI.TextInvisible();
            boxUI.Slidervisible();
            boxUI.Casting(1.0f);
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(isEmpty);
            }
            else
            {
                // Network player, receive data
                this.isEmpty = (bool)stream.ReceiveNext();
            }
        }
    }
}
