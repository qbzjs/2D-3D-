using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using GHJ_Lib;

namespace KSH_Lib.Object
{
    public class PurificationBox : GaugedObj
    {
        public Transform CharacterPos;

        [SerializeField] float dollInteractCostTime = 30.0f;
        [SerializeField] float exorcistCastingTime = 3.0f;
        [SerializeField]
        protected DollController DollInBox = null;
        [SerializeField]
        Animator animator;
        [SerializeField] public bool IsInteracting { get; private set; }


        protected override bool CheckAdditionalCondition( in InteractionPromptUI promptUI )
        {
            if(targetController.gameObject.CompareTag(GameManager.DollTag))
            {
                if(DollInBox == null)
                {
                    promptUI.Inactivate();
                    return false;
                }
            }
            else if(targetController.gameObject.CompareTag(GameManager.ExorcistTag))
            {
                if(DollInBox != null || targetController.CurBehavior is not BvCatch)
                {
                    promptUI.Inactivate();
                    return false;
                }
            }
            return true;
        }

        public override bool Interact( Interactor interactor )
        {
            if (interactor.gameObject.CompareTag(GameManager.DollTag))
            {
                targetController.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Interact);
                IsInteracting = true;
                photonView.RPC("ShareInteractingInPurificationBox_RPC", RpcTarget.AllViaServer, IsInteracting );
                castingSystem.StartCasting( CastingSystem.Cast.CreateByTime( dollInteractCostTime, coolTime: CoolTime ),
                    new CastingSystem.CastFuncSet( RunningCondition: targetController.IsInteractionKeyHold, PauseAction: PauseAction, FinishAction: DollFinishAction)
                    );
            }
            else if(interactor.gameObject.CompareTag(GameManager.ExorcistTag))
            {
                targetController.ChangeBvToImprison();
                castingSystem.StartCasting( CastingSystem.Cast.CreateByTime( exorcistCastingTime, coolTime: CoolTime ),
                    new CastingSystem.CastFuncSet(FinishAction: ExorcistFinishAction ) );
            }
            else
            {
                Debug.LogError( "PurificationBox.Interact: No Interact Target Tags, Please check target's interactor tag" );
                return false;
            }
            return true;
        }
        void PauseAction()
        {
            castingSystem.ResetCasting();
            IsInteracting = false;
            photonView.RPC( "ShareInteractingInFinalAltar_RPC", RpcTarget.AllViaServer, IsInteracting );

            targetController.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
        }
        void ExorcistFinishAction()
        {
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Idle );
        }

        public void SetDoll(DollController doll)
        {
            castingSystem.ResetCasting();
            DollInBox = doll;
            animator.Play( "CloseDoor" );
        }

        public void DollFinishAction()
        {
            targetController.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);

            DollInBox.EscapeFrom( transform, LayerMask.NameToLayer( "Player" ) );
            DollInBox.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Escape );
            animator.Play( "OpenDoor" );
            DollInBox = null;
        }

        [PunRPC]
        void ShareInteractingInPurificationBox_RPC( bool isInteracting )
        {
            IsInteracting = isInteracting;
        }
    }
}