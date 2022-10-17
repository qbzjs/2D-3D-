using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GHJ_Lib;
using KSH_Lib;

using Photon.Pun;
using Photon.Realtime;

namespace LSH_Lib{
    public class FinalAltarInteraction : interaction
    {
        public static int altarCount = 0;
        public override void OnEnable()
        {
            initialValue();
            canActiveToExorcist = false;
            canActiveToDoll = true;
        }

        void Update()
        {
            StateCheck();
            
        }
        public override void Interact(string tag, NetworkDollController character)
        {
            Casting(character);
        }

        public void initialValue()
        {
            curGauge = 0.0f;
        }

        public static void AddCount()
        {
            altarCount++;
        }

        protected override void Casting(NetworkDollController character)
        {
            curGauge += character.CastingVelocity * Time.deltaTime;
            photonView.RPC("SendGauge", RpcTarget.All, curGauge);
        }

        protected override void AutoCasting(float chargeTime)
        {
            SceneManager.Instance.EnableAutoCastingBar(this.gameObject,chargeTime);
        }
        
        private void StateCheck()
        {
            if (altarCount == 4)
            {
                canActiveToDoll = true;
                Active();
            }
        }
        private void Active()
        {
            //Debug.Log("isActive");
            if (GetGaugeRate >= 1.0f && canActiveToDoll)
            {
                canActiveToDoll = false;
                //open Door
            }

            if (canActiveToDoll)
            {
                if (curGauge > 0)
                {
                    curGauge -= reduction * Time.deltaTime;

                }
                if (curGauge < 0)
                {
                    curGauge = 0.0f;

                }
            }
        }
    }
}