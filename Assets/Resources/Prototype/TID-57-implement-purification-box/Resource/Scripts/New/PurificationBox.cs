using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GHJ_Lib;
using KSH_Lib;
using Photon;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib{
 //Is it for only test. do not use main game scene
    public class PurificationBox : MonoBehaviourPunCallbacks, IPunObservable
    {
        public GameObject[] DollModels;
        public GameObject ImprisonDoll = null;
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
            boxUI.UIInvisible();
        }

        

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Exorcist"))
            {
                if (ImprisonDoll)
                {
                    return;
                }
                NetworkExorcistController controller = other.GetComponent<NetworkExorcistController>();
                if (!(controller.CurBehavior is BvGrab))
                {
                    return;
                }
                boxUI.IsMine = controller.photonView.IsMine;

                boxUI.TextVisible();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    playerTag = other.gameObject.tag;
                    if(boxManager.Doll.CurBehavior is BvGrabbed && 
                        boxManager.Exorcist.CurBehavior is BvGrab)
                    { 
                        ExorcistInteract();
                    }
                    controller.MissDoll();
                }
               
            }

            if(other.gameObject.CompareTag("Doll"))
            {
                if (!ImprisonDoll)
                {
                    return;
                }


                playerTag = other.gameObject.tag;
                //if(boxManager.Doll.CurBehavior is BvNormal)
                {

                    NetworkDollController controller = other.GetComponent<NetworkDollController>();
                    boxUI.IsMine = controller.photonView.IsMine;
                    boxUI.TextVisible();
                    if (Input.GetKey(KeyCode.Mouse0))
                    {
                        DollInteract();
                    }
                }
            }

        }

        private void OnTriggerExit(Collider other)
        {

            boxUI.TextInvisible();
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(0, 0, 150, 30), "box is empty : " + isEmpty.ToString());
        }

        [PunRPC]
        void Boxinteract(string tag)
        {
            if(tag == "Exorcist")
            {
                isEmpty = false;
                boxManager.Doll.Imprison(this.gameObject);
                DollModels[0].SetActive(true);
                ImprisonDoll = DollModels[0];
                Animator animator = ImprisonDoll.GetComponent<Animator>();
                animator.enabled = true;
                animator.Play("Fear");
                //SetPosition(this.gameObject.transform.position);
            }
            if(tag == "Doll")
            {
                isEmpty = true;
                ImprisonDoll.SetActive(false);
                ImprisonDoll = null;
                boxManager.Doll.Released(this.transform);
            }
        }
        
        void SetPosition(Vector3 position)
        {
            boxManager.Doll.SetPosition(position);
        }

        void ExorcistInteract()
        {
            boxUI.TextInvisible();
            boxUI.Slidervisible();
            boxUI.AutoCasting(1.0f);
            pv.RPC("Boxinteract", RpcTarget.All, playerTag);
        }

        void DollInteract()
        {
            boxUI.TextInvisible();
            boxUI.Slidervisible();
            boxUI.Casting(1.0f);
            if (boxUI.CheckValue())
            {
                boxUI.TextInvisible();
                boxUI.SliderInvisible();
                pv.RPC("Boxinteract", RpcTarget.All, playerTag);
            }
        }

        public void DieToGhost()
        {
            if (!ImprisonDoll)
            {
                return;
            }
            GameEndManager.Instance.DollCountDecrease();
            string tag = "Doll";
            pv.RPC("Boxinteract", RpcTarget.All, tag);

            if (GameEndManager.Instance.DollCount <= 1)
            {
                GameEndManager.Instance.EndAllUser();
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
