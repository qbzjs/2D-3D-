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
                    castingSystem.StartManualCasting( CastingSystem.Cast.CreateByTime( dollInteractTime ), IsInputNow, SyncGauge );
                }
                break;
                case InteractTarget.Exorcist:
                {
                    castingSystem.StartAutoCasting( CastingSystem.Cast.CreateByTime( exorcistCastingTime, coolTime: 0.1f ), SyncGauge );
                }
                break;
                default:
                {
                    Debug.LogError( "NormalAltar.TjryInteract: Interact Error Happend, No Interact Target Set" );
                }
                break;
            }
        }

        bool IsInputNow()
        {
            return Input.GetKey( KeyCode.G );
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

                CanInteract = other.GetComponent<NetworkBaseController>().IsWatching( gameObject.tag );
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

                ExorcistController exorcist = other.GetComponent<ExorcistController>();
                interactTarget = InteractTarget.Exorcist;
                CanInteract = exorcist.IsWatching( gameObject.tag );
                ActivateText( CanInteract );
            }
        }
    }
}