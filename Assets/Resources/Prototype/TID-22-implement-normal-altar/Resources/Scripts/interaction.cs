using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GHJ_Lib;
using LSH_Lib;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib
{
    public class interaction : MonoBehaviourPunCallbacks, IPunObservable
    {
        public bool CanActiveToExorcist = true;
        public bool CanActiveToDoll = true;
        public float GetGaugeRate
        {
            get { return curGauge / maxGauge; }
        }

        [SerializeField]
        protected float maxGauge = 10.0f;
        [SerializeField]
        protected float reduction = 0.5f;
        //protected bool canActive = true;
        protected float curGauge = 0;
        public void IncreaseGauge(float addGauge)
        {
            curGauge += addGauge * Time.deltaTime;
        }
        virtual public void Interact(string tag, NetworkExorcistController character)
        {
            AutoCasting(2.0f);
        }

        virtual public void Interact(string tag, NetworkDollController character)
        {
            Casting(character);
        }


        virtual protected void Casting(NetworkExorcistController character)
        {
            curGauge += character.CastingVelocity * Time.deltaTime;
            photonView.RPC("SendGauge", RpcTarget.All,curGauge);
        }
        virtual protected void Casting(NetworkDollController character)
        {
            curGauge += character.CastingVelocity * Time.deltaTime;
            photonView.RPC("SendGauge", RpcTarget.All, curGauge);
        }
        virtual protected void Casting(float chargeVelocity)
        {
            curGauge += chargeVelocity * Time.deltaTime;
            photonView.RPC("SendGauge", RpcTarget.All, curGauge);
        }



        virtual protected void AutoCasting(float chargeTime)
        {
            SceneManager.Instance.EnableAutoCastingNullBar(chargeTime);
            StartCoroutine("AutoCast");
        }
        virtual protected void AutoCasting(GameObject obj,float chargeTime)
        {
            SceneManager.Instance.EnableAutoCastingBar(obj,chargeTime);
            StartCoroutine("AutoCast");
        }
        virtual protected void Immediate(NetworkExorcistController character)
        {
            this.gameObject.transform.SetParent(character.transform);
        }
        virtual protected void Immediate(NetworkDollController character)
        {
            this.gameObject.transform.SetParent(character.transform);
        }

        [PunRPC]
        public void SendGauge(float gauge)
        {
            curGauge = gauge;
        }

        protected virtual IEnumerator AutoCast()
        {
            if (SceneManager.Instance.IsCoroutine)
            {
                yield break;
            }
            while (true)
            {
                photonView.RPC("SendGauge", RpcTarget.All, curGauge);
                yield return new WaitForEndOfFrame();
                if (!SceneManager.Instance.IsCoroutine)
                {
                    break;
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }
    }
}

