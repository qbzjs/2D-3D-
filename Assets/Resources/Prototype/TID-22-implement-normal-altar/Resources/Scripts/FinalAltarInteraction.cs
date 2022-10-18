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
            canActiveToDoll = false;
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

            if (GetGaugeRate >= 1.0f && canActiveToDoll)
            {
                canActiveToDoll = false;
                StartCoroutine("OpenDoor");
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

        
        IEnumerator OpenDoor()
        {
            while (true)
            {
                transform.position = transform.position - new Vector3(0.0f, 2.0f * Time.deltaTime, 0.0f);
                yield return new WaitForSeconds(0.2f);
                if (transform.position.y < -transform.localScale.y / 2)
                {
                    yield break;
                }
            }
            
        }
    }
}