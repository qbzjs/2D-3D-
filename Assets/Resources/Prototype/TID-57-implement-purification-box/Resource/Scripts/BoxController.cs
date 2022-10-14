using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using GHJ_Lib;
namespace LSH_Lib{
    public class BoxController : MonoBehaviourPunCallbacks, IPunObservable
    {
        
        BoxUIController UIControll;
        PhotonView pv;
        bool canInteract = true;
        public bool isEmpty = true;
        bool isclick;
        string PlayerTag;

        private void Start()
        {
            UIControll = GameObject.Find("BoxUI").GetComponent<BoxUIController>();
            if(UIControll == null)
            {
                Debug.LogError("In BoxController : UIController is null");
            }

            UIControll.UIInvisible();

            pv = PhotonView.Get(this);
            pv.RPC("DoInteraction", RpcTarget.MasterClient, "RPC success");
            pv.RPC("DoInteraction", RpcTarget.Others, "RPC success");
        }
        private void Update()
        {
            KeyUp();
        }
        private void OnTriggerStay(Collider other)
        {
            
            if (other.CompareTag("Exorcist"))
            {
                PlayerTag = "Exorcist";
                //if (isEmpty && Exorcist.states.hasDoll)
                if (isEmpty)
                {
                    DoInteraction(false);
                }
            }

            if (other.CompareTag("Doll"))
            {
                PlayerTag = "Doll";
                if (!isEmpty)
                {
                    DoInteraction(true);
                }
            } 
            
        }
        private void OnTriggerExit(Collider other)
        {
            UIControll.TextInvisible();
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(0, 0, 150, 30), isclick.ToString());
            GUI.Box(new Rect(0, 60, 150, 30), "BoxEmpty : " + isEmpty.ToString());
        }
        [PunRPC]
        void DoInteraction(bool hasDoll)
        {
            
            if (Input.GetKey(KeyCode.Mouse0))
            {
                isclick = true;

                UIControll.Slidervisible();
                UIControll.TextInvisible();

                if (PlayerTag == "Exorcist")
                {
                    UIControll.AutoCasting(10.0f);
                    this.isEmpty = hasDoll;
                    //Exorcist.states.hasDoll = false;
                }
                if(PlayerTag == "Doll")
                {
                    UIControll.Casting(1.0f);
                    if(UIControll.slider.value.Equals(1.0f))
                    {
                        this.isEmpty = true;
                        UIControll.SliderInvisible();
                        UIControll.TextInvisible();
                    }
                }
            }
            else
            {
                UIControll.TextVisible();
                UIControll.SliderInvisible();
            }
        }
        void KeyUp()
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if(PlayerTag == "Doll")
                {
                    isclick = false;
                    UIControll.Initialized();
                }
                if(PlayerTag == "Exorcist")
                {
                    UIControll.Slidervisible();
                }
            }
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
