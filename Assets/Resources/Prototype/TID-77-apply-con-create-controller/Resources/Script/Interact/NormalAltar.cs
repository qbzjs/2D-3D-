using KSH_Lib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

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
        bool isNeedToChangeState = false;

        protected override void OnEnable()
		{
            base.OnEnable();
			//castingSystem.ResetCasting();
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
            if(!targetController.IsInteractionKeyDown())
            {
                return;
            }

            targetController.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Interact);

            switch ( targetType )
            {
                case InteractTargetType.Doll:
                {
                    castingSystem.ForceSetRatioTo( RateOfGauge );
                    castingSystem.StartCasting( CastingSystem.Cast.CreateByTime( dollInteractTime ),
                        new CastingSystem.CastFuncSet( SyncGauge, targetController.IsInteractionKeyHold, DollFinishAction, DollFinishAction )
                        );
                }
                break;
                case InteractTargetType.Exorcist:
                {
                    if ( targetController.IsInteractionKeyDown() )
                    {
                        castingSystem.StartCasting( CastingSystem.Cast.CreateByTime( exorcistCastingTime, coolTime: 1.0f ),
                            new CastingSystem.CastFuncSet( FinishAction: ExorcistFinishAction )
                            );
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

        protected override bool InteractCondition()
        {
            bool condition = CheckInteract();
            
            if(targetController != null)
            {
                targetController.SetCanInteract( condition );
            }
            return condition;
        }
        bool CheckInteract()
        {
            if ( targetController == null )
            {
                return false;
            }

            if ( !IsInRange )
            {
                return false;
            }

            if ( targetType == InteractTargetType.Doll )
            {
                if ( RateOfGauge >= 1.0f )
                {
                    return false;
                }
            }
            else if ( targetType == InteractTargetType.Exorcist )
            {
                if ( RateOfGauge <= 0.5f )
                {
                    return false;
                }
                if ( RateOfGauge >= 1.0f )
                {
                    return false;
                }
            }

            return targetController.IsWatching( gameObject.tag );
        }


        protected override void HandleTriggerStay( Collider other )
        {
            if ( other.gameObject.CompareTag( GameManager.DollTag ) )
            {
                IsInRange = true;
                targetController = other.GetComponent<DollController>();
                targetType = InteractTargetType.Doll;

                ActivateText( CheckInteract() );
            }
            else if ( other.gameObject.CompareTag( GameManager.ExorcistTag ) )
            {
                IsInRange = true;
                targetController = other.GetComponent<ExorcistController>();
                targetType = InteractTargetType.Exorcist;

                ActivateText( CheckInteract() );
            }

            //if(other.gameObject.CompareTag(GameManager.DollTag))
            //{
            //    if ( RateOfGauge >= 1.0f )
            //    {
            //        CanInteract = false;
            //        ActivateText( CanInteract );
            //        targetController.SetCanInteract( CanInteract );
            //        return;
            //    }

            //    targetController = other.GetComponent<DollController>();
            //    interactTarget = InteractTarget.Doll;
            //    CanInteract = targetController.IsWatching( gameObject.tag ) && castingSystem.WasReset;
            //    ActivateText( CanInteract );
            //    targetController.SetCanInteract( CanInteract );

            //    var Controller = other.GetComponent<DollController>();


            //}
            //if ( other.gameObject.CompareTag( GameManager.ExorcistTag ) )
            //{
            //    if ( RateOfGauge <= 0.5f )
            //    {
            //        CanInteract = false;
            //        ActivateText( CanInteract );
            //        targetController.SetCanInteract( CanInteract );
            //        return;
            //    }

            //    targetController = other.GetComponent<ExorcistController>();
            //    interactTarget = InteractTarget.Exorcist;
            //    CanInteract = targetController.IsWatching( gameObject.tag ) && castingSystem.WasReset;
            //    ActivateText( CanInteract );
            //    targetController.SetCanInteract( CanInteract );
            //}
        }
        protected override void HandleTriggerExit( Collider other )
        {
            if ( other.gameObject.CompareTag( GameManager.DollTag ) && targetController is DollController )
            {
                IsInRange = false;
                targetController = null;
                ActivateText( false );
            }
            else if( other.gameObject.CompareTag( GameManager.ExorcistTag ) && targetController is ExorcistController )
            {
                IsInRange = false;
                targetController = null;
                ActivateText( false );
            }


            //if ( other.gameObject.CompareTag( GameManager.DollTag ) || other.gameObject.CompareTag( GameManager.ExorcistTag ) )
            //{
            //    other.GetComponent<NetworkBaseController>().CanInteract = false;
            //    CanInteract = false;
            //    ActivateText( CanInteract );
            //    targetController.SetCanInteract( CanInteract );
            //}
        }

    }
}