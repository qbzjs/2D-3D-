using KSH_Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class NormalAltar: GaugedObject
	{
		protected bool isEnable = true;
        [SerializeField]
        float exorcistCastingTime = 3.0f;
        [SerializeField]
        float exorcistInteractRate = 0.3f;
        [SerializeField]
        float dollInteractTime = 50.0f;

        public NetworkBaseController targetController;

        protected override void OnEnable()
		{
            base.OnEnable();
			castingSystem.ResetCasting();
		}


        protected override void DoResult()
        {
            isEnable = false;
            StageManager.Instance.DecreaseAltarCount();
        }

        protected override bool ResultCondition()
        {
            return isEnable && castingSystem.IsFinshCasting;
        }

        protected override void TryInteract()
        {
            switch ( interactTarget )
            {
                case InteractTarget.Doll:
                {
                    castingSystem.StartManualCasting(
                        CastingSystem.Cast.CreateByTime( dollInteractTime ),
                        targetController.IsInteractionKeyHold,
                        SyncGauge,
                        DollFinishAction,
                        DollFinishAction
                        );
                }
                break;
                case InteractTarget.Exorcist:
                {
                    if(targetController.IsInteractionKeyDown())
                    {
                        castingSystem.StartAutoCasting( CastingSystem.Cast.CreateByTime( exorcistCastingTime, coolTime: 1.0f ), FinishAction:ExorcistFinishAction );
                    }
                }
                break;
                default:
                {
                    Debug.LogError( "NormalAltar.TjryInteract: Interact Error Happend, No Interact Target Set" );
                }
                break;
            }
        }

        void DollFinishAction()
        {
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Idle );
        }
        void ExorcistFinishAction()
        {
            RateOfGauge -= exorcistInteractRate;
            photonView.RPC( "ShareGauge", Photon.Pun.RpcTarget.AllViaServer, RateOfGauge );
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Idle );
        }


        protected override void HandleTriggerStay( Collider other )
        {
            if(other.gameObject.CompareTag(GameManager.DollTag))
            {
                if ( RateOfGauge >= 1.0f )
                {
                    CanInteract = false;
                    ActivateText( CanInteract );
                    return;
                }

                targetController = other.GetComponent<DollController>();
                CanInteract = targetController.IsWatching( gameObject.tag );
                interactTarget = InteractTarget.Doll;
                ActivateText( CanInteract );
            }
            else if ( other.gameObject.CompareTag( GameManager.ExorcistTag ) )
            {
                if ( RateOfGauge <= 0.5f )
                {
                    CanInteract = false;
                    ActivateText( CanInteract );
                    return;
                }

                targetController = other.GetComponent<ExorcistController>();
                interactTarget = InteractTarget.Exorcist;
                CanInteract = targetController.IsWatching( gameObject.tag );
                ActivateText( CanInteract );
            }
        }
        protected override void HandleTriggerExit( Collider other )
        {
            if ( other.gameObject.CompareTag( GameManager.DollTag ) || other.gameObject.CompareTag( GameManager.ExorcistTag ) )
            {
                other.GetComponent<NetworkBaseController>().CanInteract = false;
                CanInteract = false;
                ActivateText( CanInteract );
            }
        }
    }
}