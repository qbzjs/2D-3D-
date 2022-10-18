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

            pv = GetComponent<PhotonView>();
            
            
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
                if (isEmpty)
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        if(pv.IsMine)
                        {
                            DoInteraction(PlayerTag);
                        }
                    }
                    
                }
            }

            if (other.CompareTag("Doll"))
            {
                PlayerTag = "Doll";
                if (!isEmpty)
                {
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        //DoInteraction(PlayerTag);
                        if(!pv.IsMine)
                        {
                            pv.RPC("DollInteraction", RpcTarget.MasterClient,"Doll");
                        }
                    }
                    //else
                    //{
                    //    UIControll.TextVisible();
                    //    UIControll.SliderInvisible();
                    //}

                }
            }

        }
        private void OnTriggerExit(Collider other)
        {
            UIControll.TextInvisible();
            UIControll.SliderInvisible();
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(0, 0, 150, 30), isclick.ToString());
            GUI.Box(new Rect(0, 60, 150, 30), "BoxEmpty : " + isEmpty.ToString());
        }
        

        [PunRPC]
        public void DoInteraction(string PlayerTag)
        {   
            //if (Input.GetKey(KeyCode.Mouse0))
            {
                isclick = true;
                
                if (PlayerTag == "Exorcist")
                {
                    UIControll.Slidervisible();
                    UIControll.TextInvisible();
                    UIControll.AutoCasting(10.0f);
                    this.isEmpty = false;
                }

                if (PlayerTag == "Doll")
                {
                    UIControll.Slidervisible();
                    UIControll.TextInvisible();
                    UIControll.Casting(1.0f);
                    if (UIControll.CheckValue())
                    {
                        this.isEmpty = true;
                        UIControll.SliderInvisible();
                        UIControll.TextInvisible();
                    }
                }
            }
        }
        void KeyUp()
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                isclick = false;
                UIControll.Initialized();
                UIControll.Slidervisible();
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
