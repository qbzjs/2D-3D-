using KSH_Lib.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GHJ_Lib;
using Photon.Pun;

namespace KSH_Lib.Object
{
    public class FinalAltar : GaugedObj
    {
        public enum AltarState { Wait, CanOpen, Opened }

        [SerializeField] float dollInteractCostTime = 30.0f;
        [SerializeField] public AltarState altarState { get; private set; }
        [SerializeField] public bool IsInteracting { get; private set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            StageManager.Instance.SetAltar( this );
        }
        protected override bool CheckAdditionalCondition( in InteractionPromptUI promptUI )
        {
            if ( altarState != AltarState.CanOpen )
            {
                return false;
            }
            if (IsInteracting)
            {
                return false;
            }

            if (!targetController.gameObject.CompareTag(GameManager.DollTag))
            {
                return false;
            }
            return true;
        }

        public override bool Interact( Interactor interactor )
        {
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Interact );
            IsInteracting = true;
            photonView.RPC( "ShareInteractingInFinalAltar_RPC", RpcTarget.AllViaServer, IsInteracting );

            castingSystem.StartCasting( CastingSystem.Cast.CreateByTime( dollInteractCostTime ),
                new CastingSystem.CastFuncSet( RunningCondition: targetController.IsInteractionKeyHold, PauseAction: PauseAction, FinishAction: FinishCasting )
                );
            return true;
        }

        void PauseAction()
        {
            castingSystem.ResetCasting();
            IsInteracting = false;
            photonView.RPC( "ShareInteractingInFinalAltar_RPC", RpcTarget.AllViaServer, IsInteracting );
        }
        void FinishCasting()
        {
            photonView.RPC( "OpenDoorRPC", RpcTarget.AllViaServer );
        }
        public void SetDoorState( AltarState state )
        {
            altarState = state;
        }

        [PunRPC]
        void ShareInteractingInFinalAltar_RPC( bool isInteracting )
        {
            IsInteracting = isInteracting;
        }

        [PunRPC]
        void OpenDoorRPC()
        {
            if ( this.transform.position.y < -this.transform.localScale.y )
            {
                return;
            }
            this.transform.position -= new Vector3( 0, Time.deltaTime, 0 );
            SetDoorState( AltarState.Opened );
        }
    }
}