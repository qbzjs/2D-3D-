using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GHJ_Lib;
using Photon.Pun;

namespace KSH_Lib.Object
{
    public class ExitAltar : GaugedObj
    {
        public enum AltarState { Wait, CanOpen, Closed }
        public GameObject ExitAltarModel;
        [SerializeField] public float dollInteractCostTime = 10.0f;
        [SerializeField] public float exorcistInteractTime = 1.0f;
        [SerializeField] public AltarState altarState { get; private set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            StageManager.Instance.SetAltar( this );
            ExitAltarModel.SetActive( false );

            if (PhotonNetwork.CurrentRoom.PlayerCount <= 2)
            {
                EnableExitAltar();
            }
        }

        protected override bool CheckAdditionalCondition( in InteractionPromptUI promptUI )
        {
            if ( altarState != AltarState.CanOpen )
            {
                return false;
            }
            return true;
        }

        public override bool Interact( Interactor interactor )
        {
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Interact );
            if (altarState == AltarState.CanOpen )
            {
                if (targetController.gameObject.CompareTag( GameManager.DollTag ) )
                {
                    castingSystem.StartCasting( CastingSystem.Cast.CreateByTime( dollInteractCostTime ),
                        new CastingSystem.CastFuncSet ( RunningCondition: DollRunningCondition, PauseAction: PauseAction, FinishAction: DollFinishAction ) );
                }
                else if (targetController.gameObject.CompareTag( GameManager.ExorcistTag ) )
                {
                    castingSystem.StartCasting( CastingSystem.Cast.CreateByTime( exorcistInteractTime ),
                        new CastingSystem.CastFuncSet( RunningCondition: targetController.IsInteractionKeyHold, PauseAction: PauseAction, FinishAction: ExorcistFinishAction ) );
                }
                else
                {
                    Debug.LogError( "NormalAltar.Interact: No Interact Target Tags, Please check target's interactor tag" );
                }
            }
            return false;
        }
        bool DollRunningCondition()
        {
            return targetController.IsInteractionKeyHold() && !IsExorcistInteracting;
        }
        void PauseAction()
        {
            targetController.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
            castingSystem.ResetCasting();
        }
        void DollFinishAction()
        {
            photonView.RPC("ExitGameRPC", RpcTarget.AllViaServer);
        }
        void ExorcistFinishAction()
        {
            targetController.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
            photonView.RPC( "ChangeAltarStateTo_RPC", RpcTarget.AllViaServer, AltarState.Closed );
        }

        [PunRPC]
        public void ChangeAltarStateTo_RPC( AltarState state)
        {
            altarState = state;

            if ( altarState == AltarState.CanOpen)
            {
                ExitAltarModel.SetActive( true );
            }
            else
            {
                ExitAltarModel.SetActive( false );
            }
        }

        public void EnableExitAltar()
        {
            altarState = AltarState.CanOpen;
            ExitAltarModel.SetActive( true );
        }

        [PunRPC]
        void ExitGameRPC()
        {
            StageManager.Instance.DoExit(targetController);
        }

    }
}