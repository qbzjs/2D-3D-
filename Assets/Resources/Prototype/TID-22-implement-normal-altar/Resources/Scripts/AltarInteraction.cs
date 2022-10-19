using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GHJ_Lib;
using Cinemachine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib
{
    public class AltarInteraction : interaction
    {
        public Image image;
        protected bool isCoolTime = false; 
        public override void OnEnable()
        {
            initialValue();
            CanActiveToExorcist = false;
            CanActiveToDoll = true;
        }

        void Update()
        {
            if (image != null)
            { 
                image.fillAmount = GetGaugeRate;
            }

            if (GetGaugeRate >= 1.0f && CanActiveToDoll)
            {
                CanActiveToDoll = false;
                CanActiveToExorcist = false;
                FinalAltarInteraction.AddCount();
                return;
            }

            if (GetGaugeRate >= 0.3f && GetGaugeRate <1.0f)
            {
                CanActiveToExorcist = true;
            }
            else
            {
                CanActiveToExorcist = false;
            }

            if (CanActiveToDoll)
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

        public override void Interact(string tag, NetworkExorcistController character)
        {
            AutoCasting(2.0f);
        }
        public override void Interact(string tag, NetworkDollController character)
        {
            Casting(character);
        }

        protected override void Casting(NetworkDollController character)
        {
            curGauge += character.CastingVelocity * Time.deltaTime;
            photonView.RPC("SendGauge", RpcTarget.All, curGauge);
        }
        protected override void AutoCasting(float chargeTime)
        {
            photonView.RPC("GaugeDown", RpcTarget.All, maxGauge/10);
            SceneManager.Instance.EnableAutoCastingNullBar(chargeTime);
        }

        public void initialValue()
        {
            curGauge = 0.0f;
        }

        [PunRPC]
        public void GaugeDown(float downGauge)
        {
            if (isCoolTime)
            {
                return;
            }
            curGauge -= downGauge;
            StartCoroutine(StartExorcistCoolTime());
        }


        IEnumerator StartExorcistCoolTime()
        {
            isCoolTime = true;
            yield return new WaitForSeconds(2);
            isCoolTime = false;
            
        }
    }
}