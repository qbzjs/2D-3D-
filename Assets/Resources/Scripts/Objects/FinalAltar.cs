using KSH_Lib.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GHJ_Lib;
using LSH_Lib;
using Photon.Pun;

namespace KSH_Lib.Object
{
    public class FinalAltar : GaugedObj
    {
        public enum AltarState { Wait, CanOpen, Opened }

        [SerializeField] public AltarState altarState { get; private set; }
        [SerializeField] public bool IsInteracting { get; private set; }

        [SerializeField] Animator animator;

        public AudioPlayer AudioPlayer;
        protected override void OnEnable()
        {
            base.OnEnable();
            StageManager.Instance.SetAltar( this );
        }
        protected override bool CheckAdditionalCondition( in InteractionPromptUI promptUI )
        {
            if (targetController.CurBehavior is not BvIdle)
            {
                return false;
            }

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
            targetController.SetInteractType(GaugedObjType.FinalAltar);
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Interact );
            IsInteracting = true;
            photonView.RPC( "ShareInteractingInFinalAltar_RPC", RpcTarget.AllViaServer, IsInteracting );

            castingSystem.StartCasting( CastingSystem.Cast.CreateByRatio( targetController.InteractionSpeed / MaxGauge ),
                new CastingSystem.CastFuncSet( RunningCondition: RunningCondition, PauseAction: PauseAction, FinishAction: FinishCasting )
                );
            AudioPlayer.Play("DollNormalAltar",AudioManager.PlayTarget.Doll);
            return true;
        }

        bool RunningCondition()
        {
            return targetController.IsInteractionKeyHold();
        }

        void PauseAction()
        {
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Idle );
            castingSystem.ResetCasting();
            IsInteracting = false;
            AudioPlayer.Stop("DollNormalAltar");
            photonView.RPC( "ShareInteractingInFinalAltar_RPC", RpcTarget.AllViaServer, IsInteracting );
        }
        void FinishCasting()
        {
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Idle );
            AudioPlayer.Stop("DollNormalAltar");
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
            animator.SetBool( "IsDoorOpen", true );
            AudioPlayer.Play("FinalOpen");
        }
    }
}