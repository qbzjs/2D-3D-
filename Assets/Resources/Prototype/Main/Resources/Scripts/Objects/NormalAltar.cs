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
        [SerializeField] float dollInteractTime = 50.0f;
        public override bool CanInteract
        {
            get => !castingSystem.IsCoroutineRunning;
        }

        void DollPauseAction()
        {
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Idle );
        }
        void DollFinishAction()
        {
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Idle );
            castingSystem.ResetCasting();
        }
        void ExorcistFinishAction()
        {
            RateOfGauge += exorcistInteractRate;
            photonView.RPC( "ShareRate", RpcTarget.AllViaServer, RateOfGauge );
            photonView.RPC( "ShareExorcistInteract", RpcTarget.AllViaServer, IsExorcistInteracting );
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Idle );
        }


        protected override bool CheckIntractableByType( in InteractionPromptUI promptUI )
        {
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
        public override bool ActiveInteractPrompt( Interactor interactor, InteractionPromptUI promptUI )
        {
			if( !base.ActiveInteractPrompt( interactor, promptUI ) )
            {
                return false;
            }
            promptUI.Activate(prompt);
            return true;
        }

        public override bool Interact( Interactor interactor )
        {
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Interact );

            if ( interactor.gameObject.CompareTag(GameManager.DollTag))
            {
                castingSystem.ForceSetRatioTo( RateOfGauge );
                castingSystem.StartCasting( CastingSystem.Cast.CreateByTime( dollInteractTime, coolTime: CoolTime ),
                    new CastingSystem.CastFuncSet( SyncGauge, targetController.IsInteractionKeyHold, DollPauseAction, DollFinishAction )
                    );
            }
            else if(interactor.gameObject.CompareTag(GameManager.ExorcistTag))
            {
                IsExorcistInteracting = true;
                photonView.RPC( "ShareExorcistInteract", RpcTarget.AllViaServer, IsExorcistInteracting );

                castingSystem.StartCasting( CastingSystem.Cast.CreateByTime( exorcistCastingTime, coolTime: CoolTime ),
                    new CastingSystem.CastFuncSet( FinishAction: ExorcistFinishAction )
                    );
            }
            else
            {
                Debug.LogError( "NormalAltar.Interact: No Interact Target Tags, Please check target's interactor tag" );
            }
            return false;
		}
	}
}