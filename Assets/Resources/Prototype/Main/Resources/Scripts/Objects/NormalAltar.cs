using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GHJ_Lib;
using Photon.Pun;

namespace KSH_Lib.Object
{
	public class NormalAltar : GaugedObj
    {
        [Header( "Interaction Settings" )]
        [SerializeField] float exorcistInteractRate = -0.3f;
        [SerializeField] float exorcistCastingTime = 3.0f;
        [SerializeField] float dollInteractCostTime = 50.0f;
        [SerializeField] GameObject[] candleLights;
        //[SerializeField] GameObject finishLight;
        [SerializeField] Light altarLight;
        [SerializeField] float finalIntensity = 50.0f;
        [SerializeField] float increaseIntensity = 10.0f;

        public override bool CanInteract
        {
            get => !castingSystem.IsCoroutineRunning;
        }

        float distribution;

        protected override void OnEnable()
        {
            base.OnEnable();

            foreach (var light in candleLights)
            {
                light.SetActive(false);
            }
            //finishLight.SetActive(false);
            altarLight.intensity = 0.0f;
            distribution = 1.0f / (float)(candleLights.Length);
        }

        bool DollRunningCondition()
        {
            return targetController.IsInteractionKeyHold() && !IsExorcistInteracting;
        }
        void DollPauseAction()
        {
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Idle );
        }
        void DollFinishAction()
        {
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Idle );
            castingSystem.ResetCasting();
            //finishLight.SetActive(true);
            photonView.RPC("ActiveFinishLightRPC", RpcTarget.AllViaServer);
            photonView.RPC("DecreaseAltarCountTo_RPC", RpcTarget.AllViaServer);
        }
        void ExorcistFinishAction()
        {
            RateOfGauge += exorcistInteractRate;
            photonView.RPC( "ShareRate", RpcTarget.AllViaServer, RateOfGauge );
            IsExorcistInteracting = false;
            photonView.RPC( "ShareExorcistInteract", RpcTarget.AllViaServer, IsExorcistInteracting );
            ChangeCandleLightsToEveryone();

            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Idle );
        }


        protected override bool CheckAdditionalCondition( in InteractionPromptUI promptUI )
        {
            if (targetController.CurBehavior is not BvIdle)
            {
                promptUI.Inactivate();
                return false;
            }

            if ( RateOfGauge >= 1.0f )
            {
                promptUI.Inactivate();
                return false;
            }
            if( targetController.gameObject.CompareTag( GameManager.ExorcistTag ) )
            {
                if (RateOfGauge <= 0.5f)
                {
                    promptUI.Inactivate();
                    return false;
                }
            }
            return true;
        }

        public override bool Interact( Interactor interactor )
        {
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Interact );

            if ( targetController.gameObject.CompareTag(GameManager.DollTag))
            {
                castingSystem.ForceSetRatioTo( RateOfGauge );
                castingSystem.StartCasting( CastingSystem.Cast.CreateByTime( dollInteractCostTime, coolTime: CoolTime ),
                    new CastingSystem.CastFuncSet( SyncGauge, DollRunningCondition, ChangeCandleLightsToEveryone, DollPauseAction, DollFinishAction )
                    );
                return true;
            }
            else if(targetController.gameObject.CompareTag(GameManager.ExorcistTag))
            {
                IsExorcistInteracting = true;
                photonView.RPC( "ShareExorcistInteract", RpcTarget.AllViaServer, IsExorcistInteracting );

                castingSystem.StartCasting( CastingSystem.Cast.CreateByTime( exorcistCastingTime, coolTime: CoolTime ),
                    new CastingSystem.CastFuncSet( FinishAction: ExorcistFinishAction )
                    );
                return true;
            }
            else
            {
                Debug.LogError( "NormalAltar.Interact: No Interact Target Tags, Please check target's interactor tag" );
            }
            return false;
		}

        void ChangeCandleLightsLocal()
        {
            int curLightCount = (int)OriginGauge / (int)(distribution * MaxGauge);

            Debug.Log($"curLightCount: {curLightCount}");

            int curIdx = 0;
            for (int i = 0; i < candleLights.Length; ++i)
            {
                if (i < curLightCount)
                {
                    if (!candleLights[i].activeInHierarchy) candleLights[i].SetActive(true);
                    curIdx = i;
                    continue;
                }
                if (candleLights[i].activeInHierarchy) candleLights[i].SetActive(false);
            }
            altarLight.intensity = curIdx * increaseIntensity;
        }
        void ChangeCandleLightsToEveryone()
        {
            //ChangeCandleLightsLocal();
            photonView.RPC("ChangeCandleLightsRPC", RpcTarget.AllViaServer);
        }

        [PunRPC]
        public void ChangeCandleLightsRPC()
        {
            ChangeCandleLightsLocal();
        }
        [PunRPC]
        public void ActiveFinishLightRPC()
        {
            altarLight.intensity = finalIntensity;
        }
        [PunRPC]
        public void DecreaseAltarCountTo_RPC()
        {
            StageManager.Instance.DecreaseAltarCount();
        }

    }
}