using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GHJ_Lib;
using TID42;
using Photon;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib{
 //Is it for only test. do not use main game scene
    public class InteractionTest : MonoBehaviourPunCallbacks, IPunObservable
    {
        PhotonView pv;
        PurificationBoxUI boxUI;
        BoxManager boxManager;
        string playerTag;
        bool isEmpty = true;

        private void Start()
        {
            pv = GetComponent<PhotonView>();
            boxUI = GameObject.Find("BoxUI").GetComponent<PurificationBoxUI>();
            boxManager = GameObject.Find("BoxManager").GetComponent<BoxManager>();
            boxUI.TextInvisible();
            boxUI.SliderInvisible();
        }

        private void Update()
        {
            
        }

        private void OnTriggerStay(Collider other)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0))
            {
                if (other.gameObject.CompareTag("Exorcist"))
                {
                    boxUI.TextVisible();
                    playerTag = other.gameObject.tag;
                    if(boxManager.Doll.CurBehavior is BvGrabbed)
                    { 
                        Exorcist();
                    }
                    pv.RPC("Boxinteract", RpcTarget.All, playerTag);
                }
                if(other.gameObject.CompareTag("Doll"))
                {
                    boxUI.TextVisible();
                    playerTag = other.gameObject.tag;
                    Doll();
                    pv.RPC("Boxinteract", RpcTarget.All, playerTag);
                }

            }
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(0, 0, 150, 30), "isEmpty" + isEmpty.ToString());
        }
        [PunRPC]
        void Boxinteract(string tag)
        {
            if(tag == "Exorcist")
            {
                isEmpty = false;
                boxManager.Doll.Imprison();
                boxManager.Doll.SetPosition(this.gameObject.transform.position);
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
