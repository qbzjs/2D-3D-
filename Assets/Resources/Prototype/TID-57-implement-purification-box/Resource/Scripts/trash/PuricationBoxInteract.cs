using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using GHJ_Lib;
using Photon.Pun;

namespace LSH_Lib{
    public class PuricationBoxInteract : interaction
    {
        [SerializeField]
        private bool hasDoll;
        private void Start()
        {
            initialValue();
            hasDoll = false;
        }
        private void Update()
        {
            Active();
        }
        public override void Interact(string tag, NetworkExorcistController character)
        { 
            AutoCasting(2.0f);
        }

        public override void Interact(string tag, NetworkDollController character)
        {
            Casting(character);
        }


        public void initialValue()
        {
            curGauge = 0.0f;
        }
        /*
        protected override void Casting(Character character)
        {
            curGauge += character.CastingVelocity * Time.deltaTime;
        }
        */
        protected override void Casting(NetworkExorcistController character)
        {
            curGauge += character.CastingVelocity * Time.deltaTime;
            //photonView.RPC("SendGauge", RpcTarget.All, curGauge);
        }
        protected override void Casting(NetworkDollController character)
        {
            curGauge += character.CastingVelocity * Time.deltaTime;
            //photonView.RPC("SendGauge", RpcTarget.All, curGauge);
        }

        /*
        protected override void AutoCasting(float chargeTime)
        {
            SceneManager.Instance.EnableAutoCastingBar(chargeTime);
            hasDoll = true;
        }
        */
        protected override void AutoCasting(float chargeTime)
        {
            SceneManager.Instance.EnableAutoCastingNullBar(chargeTime);
            hasDoll = true;
            //StartCoroutine("AutoCast");
        }
        /*
        protected override void AutoCasting(GameObject obj, float chargeTime)
        {
            SceneManager.Instance.EnableAutoCastingBar(obj, chargeTime);
            //StartCoroutine("AutoCast");
        }
        */
        
        private void Active()
        {
            if (GetGaugeRate >= 1.0f && canActiveToExorcist)
            {
                canActiveToExorcist = false;
                hasDoll = true;
            }

            if (canActiveToExorcist)
            {
                if (curGauge < 0)
                {
                    curGauge = 0.0f;

                }
            }
        }

    }
}
