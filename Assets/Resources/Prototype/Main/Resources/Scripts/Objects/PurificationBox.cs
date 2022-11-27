using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using GHJ_Lib;
using LSH_Lib;
namespace KSH_Lib.Object
{
    public class PurificationBox : GaugedObj
    {
        public Transform CharacterPos;

        [SerializeField] protected DollController DollInBox = null;
        [SerializeField] float exorcistMaxGauge = 20.0f;
        [SerializeField] Animator animator;
        [SerializeField] public bool IsInteracting { get; private set; }
        [SerializeField] public float Damage { get; private set; }


        [Header("Destroy Effect")]
        [SerializeField] KSH_Lib.Util.PhaseEffect phaseEffect;
        [SerializeField] float destroyTime = 1.5f;
        [SerializeField] float startHeight = 5.2f;
        [SerializeField] ParticleSystem particle;
        ParticleSystem.EmissionModule emission;

        bool isDollPurifying;


        protected override void OnEnable()
        {
            base.OnEnable();
            emission = particle.emission;
            emission.enabled = false;
        }

        protected override bool CheckAdditionalCondition( in InteractionPromptUI promptUI )
        {
            if(targetController.gameObject.CompareTag(GameManager.DollTag))
            {
                if (targetController.CurBehavior is not BvIdle)
                {
                    promptUI.Inactivate();
                    return false;
                }

                if (DollInBox == null)
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
            if (targetController.gameObject.CompareTag(GameManager.DollTag))
            {
                targetController.InteractType = GaugedObjType.PurificationBox;
                targetController.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Interact);
                IsInteracting = true;
                photonView.RPC("ShareInteractingInPurificationBox_RPC", RpcTarget.AllViaServer, IsInteracting );
                castingSystem.StartCasting( CastingSystem.Cast.CreateByRatio( targetController.InteractionSpeed / MaxGauge, coolTime: CoolTime ),
                    new CastingSystem.CastFuncSet( RunningCondition: DollRunningAction, PauseAction: PauseAction, FinishAction: DollFinishAction)
                    );
            }
            else if(targetController.gameObject.CompareTag(GameManager.ExorcistTag))
            {
                targetController.ChangeBvToImprison();
                castingSystem.StartCasting( CastingSystem.Cast.CreateByRatio( targetController.InteractionSpeed / exorcistMaxGauge, coolTime: CoolTime ),
                    new CastingSystem.CastFuncSet(FinishAction: ExorcistFinishAction ) );
                AudioManager.instance.Play("ExorcistBox");
                if (!isDollPurifying)
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
        bool DollRunningAction()
        {
            return targetController.IsInteractionKeyHold();
        }
        void PauseAction()
        {
            castingSystem.ResetCasting();
            IsInteracting = false;
            photonView.RPC( "ShareInteractingInPurificationBox_RPC", RpcTarget.AllViaServer, IsInteracting );
            
            targetController.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);
        }
        void ExorcistFinishAction()
        {
            targetController.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Idle );
            photonView.RPC( "ShareDustEffect", RpcTarget.All, true );
            
        }

        public void SetDoll(DollController doll)
        {
            castingSystem.ResetCasting();
            DollInBox = doll;
            animator.Play( "CloseDoor" );
        }

        public void DollFinishAction()
        {
            photonView.RPC( "ShareDustEffect", RpcTarget.All, false );
            AudioManager.instance.Stop("BoxActive");
            targetController.ChangeBehaviorTo(NetworkBaseController.BehaviorType.Idle);

            DollInBox.EscapeFrom( transform, LayerMask.NameToLayer( "Player" ) );
            DollInBox.ChangeBehaviorTo( NetworkBaseController.BehaviorType.Escape );
            AudioManager.instance.Play("DollBox");
            photonView.RPC("ShareDollInBox_RPC", RpcTarget.AllViaServer);
        }

        IEnumerator DestroyIfDollDead()
        {
            isDollPurifying = true;
            photonView.RPC( "ShareDustEffect", RpcTarget.All, false );

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
                    AudioManager.instance.Stop("BoxActive");
                    phaseEffect.DoFade(startHeight, 0.0f, destroyTime);
                    AudioManager.instance.Play("BoxDestroy");
                    yield return new WaitForSeconds(destroyTime);

                    PhotonNetwork.Destroy(gameObject);
                }
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
            animator.Play( "OpenDoor" );
        }

        [PunRPC]
        void ShareDustEffect(bool isEnabled)
        {
            emission.enabled = isEnabled;
        }
    }
}