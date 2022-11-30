using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GHJ_Lib;
using LSH_Lib;
using Photon.Pun;

namespace KSH_Lib.Object
{
    public class ExitAltar : GaugedObj
    {
        public enum AltarState { Wait, CanOpen, Closed }
        public GameObject ExitAltarModel;
        [SerializeField] public float exorcistMaxGauge = 20.0f;
        [SerializeField] public AltarState altarState { get; private set; }

        [Header( "Destroy Effect" )]
        [SerializeField] KSH_Lib.Util.PhaseEffect phaseEffect;
        [SerializeField] float destroyTime = 1.5f;
        [SerializeField] float startHeight = 1.2f;

        public AudioPlayer AudioPlayer;
        protected override void OnEnable()
        {
            base.OnEnable();
            StageManager.Instance.SetAltar( this );
            ExitAltarModel.SetActive( false );

            if (PhotonNetwork.CurrentRoom.PlayerCount <= 2)
            {
                photonView.RPC( "ChangeAltarStateTo_RPC", RpcTarget.AllViaServer, AltarState.CanOpen );
            }
        }

        protected override void Start()
        {
            base.Start();
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
            return true;
        }

        public override bool Interact( Interactor interactor )
        {
            targetController.SetInteractType(GaugedObjType.ExitAltar);
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Interact );
            if (altarState == AltarState.CanOpen )
            {
                if (targetController.gameObject.CompareTag( GameManager.DollTag ) )
                {
                    castingSystem.StartCasting( CastingSystem.Cast.CreateByRatio( targetController.InteractionSpeed / MaxGauge ),
                        new CastingSystem.CastFuncSet ( RunningCondition: DollRunningCondition, PauseAction: PauseAction, FinishAction: DollFinishAction ) );


                    photonView.RPC( "PlayDollEffects_RPC", RpcTarget.All );
                    AudioPlayer.Play("DollExitAltar");//, AudioManager.PlayTarget.Doll);
                }
                else if (targetController.gameObject.CompareTag( GameManager.ExorcistTag ) )
                {
                    castingSystem.StartCasting( CastingSystem.Cast.CreateByRatio( targetController.InteractionSpeed / exorcistMaxGauge ),
                        new CastingSystem.CastFuncSet( RunningCondition: targetController.IsInteractionKeyHold, PauseAction: PauseAction, FinishAction: ExorcistFinishAction ) );
                    
                    photonView.RPC( "PlayExorcistEffects_RPC", RpcTarget.All );
                    AudioPlayer.Play("HitAltar");
                
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

            
            photonView.RPC( "StopDollEffects_RPC", RpcTarget.All );
            AudioPlayer.Stop("DollExitAltar");
        }
        void DollFinishAction()
        {
            photonView.RPC( "StopDollEffects_RPC", RpcTarget.All );
            AudioPlayer.Stop( "DollExitAltar" );

            photonView.RPC("ExitGameRPC", RpcTarget.AllViaServer);

        }
        void ExorcistFinishAction()
        {
            photonView.RPC( "StopExorcistEffects_RPC", RpcTarget.All );
            AudioPlayer.Stop( "HitAltar" );

            targetController.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
            photonView.RPC( "ChangeAltarStateTo_RPC", RpcTarget.AllViaServer, AltarState.Closed );

        }

        public void OpenExitAltar()
        {
            photonView.RPC( "ChangeAltarStateTo_RPC", RpcTarget.AllViaServer, AltarState.CanOpen );
        }

        [PunRPC]
        public void ChangeAltarStateTo_RPC( AltarState state)
        {
            altarState = state;

            if ( altarState == AltarState.CanOpen)
            {
                ExitAltarModel.SetActive( true );
                phaseEffect.DoFade( 0.0f, startHeight, destroyTime );
            }
            else
            {
                StartCoroutine( DestroyExitAltar() );
            }
        }

        [PunRPC]
        void ExitGameRPC()
        {
            StageManager.Instance.ExitGame(targetController);
        }
        IEnumerator DestroyExitAltar()
        {
            phaseEffect.DoFade( startHeight, 0.0f, destroyTime );
            yield return new WaitForSeconds( destroyTime );
            ExitAltarModel.SetActive( false );
        }
    }
}