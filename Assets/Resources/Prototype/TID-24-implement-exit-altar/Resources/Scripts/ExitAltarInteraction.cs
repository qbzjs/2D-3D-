using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using GHJ_Lib;
using KSH_Lib;

namespace LSH_Lib{
    public class ExitAltarInteraction : interaction
    {
        public GameObject ExitAltarModel;
        public override void OnEnable()
        {
            ExitAltarModel.SetActive(false);
            canActiveToExorcist = false;
            canActiveToDoll = false;
        }

        protected GameObject doll;
        void Update()
        {
            CheckPlayerCount();
        }

        public override void Interact(string tag, NetworkExorcistController character)
        {
            AutoCasting(this.gameObject, 2.0f);
            //doll=null 을 넣으면 인형이 탈출도중 퇴마사가 막을수 있음. 안넣으면 인형이 탈출버튼 누른후 퇴마사가 막을수없음
        }
        public override void Interact(string tag, NetworkDollController character)
        {
            AutoCasting(2.0f);
            doll = character.gameObject;
        }

        protected override void Casting(NetworkExorcistController character)
        {
            curGauge += character.CastingVelocity * Time.deltaTime;
            photonView.RPC("SendGauge", RpcTarget.All, curGauge);
        }
        protected override void AutoCasting(float chargeTime)
        {
            SceneManager.Instance.EnableAutoCastingNullBar(chargeTime);
            StartCoroutine("AutoCast");
        }
        protected override void AutoCasting(GameObject obj,float chargeTime)
        {
            SceneManager.Instance.EnableAutoCastingBar(obj,chargeTime);
            StartCoroutine("AutoCast");
        }

        private void CheckPlayerCount()
        {
            if(GameEndManager.Instance.DollCount <= 2)
            {
                ExitAltarModel.SetActive(true);
                canActiveToExorcist = true;
                canActiveToDoll = true;
                GuageCheck();
            }
            
        }
        private void GuageCheck()
        {
            if (GetGaugeRate >= 1.0f)
            {
                canActiveToExorcist = false;
                canActiveToDoll = false;
            }
        }

        protected override IEnumerator AutoCast()
        {
            while (true)
            {
                photonView.RPC("SendGauge", RpcTarget.All, curGauge);
                yield return new WaitForEndOfFrame();
                if (!SceneManager.Instance.IsCoroutine)
                {
                    if (doll != null)
                    { 
                        GameEndManager.Instance.EndGame(doll);
                        doll = null;
                    }
                    break;
                }
            }
        }

    }
}
