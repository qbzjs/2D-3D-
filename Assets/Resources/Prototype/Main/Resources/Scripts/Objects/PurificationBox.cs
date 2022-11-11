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
        [SerializeField] protected DollController DollInBox = null;
        [SerializeField] Animator animator;
        [SerializeField] public bool IsInteracting { get; private set; }

        [SerializeField] float damagePerSecond = 20.0f;



        [Header("Destroy Effect")]
        [SerializeField] KSH_Lib.Util.PhaseEffect phaseEffect;
        [SerializeField] float destroyTime = 1.5f;
        [SerializeField] float startHeight = 5.2f;

        bool isDollPurifying;

        Coroutine damageCoroutine;


        protected override void OnEnable()
        {
            base.OnEnable();

        }

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

                damageCoroutine = StartCoroutine(DamageDoll());

                if(!isDollPurifying)
                {
                    StartCoroutine(DestroyIfDollDead());
                }
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

            StopCoroutine(damageCoroutine);

            DollInBox.EscapeFrom( transform, LayerMask.NameToLayer( "Player" ) );
            DollInBox.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Escape );
            animator.Play( "OpenDoor" );
            photonView.RPC("ShareDollInBox_RPC", RpcTarget.AllViaServer);
        }

        IEnumerator DestroyIfDollDead()
        {
            isDollPurifying = true;
            while (true)
            {
                yield return null;
                if (DollInBox == null)
                {
                    yield return null;
                }
                else if (DollInBox.GetDollData.DevilHP <= 0.0f)
                {
                    Debug.Log("Start Remove");
                    phaseEffect.DoFade(startHeight, 0.0f, destroyTime);

                    yield return new WaitForSeconds(destroyTime);

                    PhotonNetwork.Destroy(gameObject);
                }
                yield return null;
            }
        }

        IEnumerator DamageDoll()
        {
            while ( true )
            {
                if ( DollInBox == null )
                {
                    break;
                }
                if ( DollInBox.GetDollData.DevilHP <= 0.0f )
                {
                    break;
                }
                DollInBox.ChangeDevilHP( damagePerSecond * Time.deltaTime );
                yield return null;
            }
        }

        [PunRPC]
        void ShareInteractingInPurificationBox_RPC( bool isInteracting )
        {
            IsInteracting = isInteracting;
        }

        [PunRPC]
        void ShareDollInBox_RPC()
        {
            DollInBox = null;
        }
    }
}