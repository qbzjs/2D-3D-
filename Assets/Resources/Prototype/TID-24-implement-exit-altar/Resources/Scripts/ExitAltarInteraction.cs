using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib{
    public class ExitAltarInteraction : interaction
    {
        public int playerCount = 0;
        private void Start()
        {
            curGauge = 0;
            canActiveTo = false;
        }
        void Update()
        {
            CheckPlayerCount();
        }
        public override void Interact(string tag, Character character)
        {
            if (tag == "Doll")
            {
                AutoCasting(3.0f);
            }
        }
        protected override void AutoCasting(float chargeTime)
        {
            Debug.Log("AutoCasting");
            SceneManager.Instance.EnableAutoCastingBar(chargeTime);
        }
        private void CheckPlayerCount()
        {
            if(playerCount == 2)
            {
                canActiveTo = true;
                Active();
            }
            
        }
        private void Active()
        {
            if (GetGaugeRate >= 1.0f && canActiveTo)
            {
                canActiveTo = false;
            }
        }
    }
}
