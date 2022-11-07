using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using KSH_Lib.Object;

using GHJ_Lib;

namespace KSH_Lib
{
    public abstract class GaugedObj : MonoBehaviourPun, IPunObservable, IInteractable
    {

        /*--- Serialize Fields ---*/
        [SerializeField] protected string prompt = "Press G to Interact";
        [SerializeField] protected CastingSystem castingSystem;
        [field: SerializeField] public bool IsExorcistInteracting { get; protected set; }

        [Header("Gauge Settings")]
        [SerializeField] protected float RateOfGauge;
        [field: SerializeField] public float MaxGauge { get; protected set; }
        [field: SerializeField] public float DecreaseRate { get; protected set; }
        [field: SerializeField] public float CoolTime { get; protected set; }
        public float OriginGauge { get { return RateOfGauge * MaxGauge; } }

        /*--- Fields ---*/
        public string InteractionPrompt => prompt;
        public virtual bool CanInteract { get => !castingSystem.IsCoroutineRunning; }
        protected NetworkBaseController targetController;


        /*--- MonoBehaviour Callbacks ---*/
        protected virtual void OnEnable()
        {
            if ( castingSystem == null )
            {
                castingSystem = GHJ_Lib.StageManager.Instance.CastSystem;
                if ( castingSystem == null )
                {
                    Debug.LogError( "GuageObject.Enable: Can not find CastingSystem" );
                }
            }
        }

        protected virtual void SyncGauge( float gauge )
        {
            RateOfGauge = gauge;
            photonView.RPC( "ShareGauge", RpcTarget.AllViaServer, RateOfGauge );
        }
        protected virtual bool CheckIntractableByType( in InteractionPromptUI promptUI ) { return true; }
        bool CheckController(in Interactor interactor, in InteractionPromptUI promptUI )
        {
            var controller = interactor.gameObject.GetComponent<NetworkBaseController>();
            if ( controller == null )
            {
                promptUI.Inactivate();
                return false;
            }
            if ( !controller.IsMine )
            {
                return false;
            }
            targetController = controller;
            return true;
        }


        /*--- IInteractable Interfaces ---*/
        public virtual bool ActiveInteractPrompt( Interactor interactor, InteractionPromptUI promptUI )
        {
            if(!CheckController(interactor, promptUI))
            {
                return false;
            }
            if(!CheckIntractableByType( promptUI ) )
            {
                return false;
            }
            promptUI.Activate( prompt );
            return true;
        }
        public abstract bool Interact( Interactor interactor );

        /*--- IPunObservable Interfaces ---*/
        public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info )
        {
        }

        [PunRPC]
        public void ShareGauge( float gauge )
        {
            RateOfGauge = gauge;
        }

        [PunRPC]
        void ShareExorcistInteract( bool interacting )
        {
            IsExorcistInteracting = interacting;
        }
    }
}