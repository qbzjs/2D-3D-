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
        public int playerCount = 0;
        public override void OnEnable()
        {
            canActiveToExorcist = false;
            canActiveToDoll = false;
        }
        void Update()
        {
            CheckPlayerCount();
        }

        public override void Interact(string tag, NetworkExorcistController character)
        {
            AutoCasting(this.gameObject, 2.0f);
        }
        public override void Interact(string tag, NetworkDollController character)
        {
            AutoCasting(2.0f);
        }

        protected override void Casting(NetworkExorcistController character)
        {
            curGauge += character.CastingVelocity * Time.deltaTime;
            photonView.RPC("SendGauge", RpcTarget.All, curGauge);
        }
        protected override void AutoCasting(float chargeTime)
        {
            SceneManager.Instance.EnableAutoCastingNullBar(chargeTime);
        }
        protected override void AutoCasting(GameObject obj,float chargeTime)
        {
            SceneManager.Instance.EnableAutoCastingBar(obj,chargeTime);
        }

        private void CheckPlayerCount()
        {
            if(playerCount == 2)
            {
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
    }
}
