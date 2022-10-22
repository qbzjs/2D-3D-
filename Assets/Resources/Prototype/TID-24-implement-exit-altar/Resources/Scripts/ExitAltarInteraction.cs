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
            CanActiveToExorcist = false;
            CanActiveToDoll = false;
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
        private void OnGUI()
        {
            if (ExitAltarModel.activeInHierarchy)
            { 
                GUI.Box(new Rect(0, 0, 150, 30), "!!ExitAltar ON!!");
            }
        }
        private void CheckPlayerCount()
        {
            Debug.Log(GameEndManager.Instance.DollCount);
            if(GameEndManager.Instance.DollCount <= 2)
            {
                ExitAltarModel.SetActive(true);
                CanActiveToExorcist = true;
                CanActiveToDoll = true;
                GuageCheck();
            }
            
        }
        private void GuageCheck()
        {
            if (GetGaugeRate >= 1.0f)
            {
                CanActiveToExorcist = false;
                CanActiveToDoll = false;
            }
        }
        [PunRPC]
        private void ExitFromGame()
        {
            GameEndManager.Instance.DollCountDecrease();
        }

        protected override IEnumerator AutoCast()
        {
            while (true)
            {
                Debug.Log("curGauge : "+ curGauge);
                photonView.RPC("SendGauge", RpcTarget.All, curGauge);
                yield return new WaitForEndOfFrame();
                Debug.Log("SceneManager.Instance.IsCoroutine : " + SceneManager.Instance.IsCoroutine);
                if (!SceneManager.Instance.IsCoroutine)
                {
                    if (doll != null)
                    {
                        photonView.RPC("ExitFromGame", RpcTarget.All);
                        GameEndManager.Instance.EndGameDoll(doll);
                        doll = null;
                    }
                    break;
                }
            }
        }

    }
}
