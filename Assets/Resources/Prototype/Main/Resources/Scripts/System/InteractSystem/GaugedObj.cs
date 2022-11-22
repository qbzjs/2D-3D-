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

        [field: SerializeField] public float MaxGauge { get; protected set; }
        [field: SerializeField] public float DecreaseRate { get; protected set; }
        [field: SerializeField] public float CoolTime { get; protected set; }

        [SerializeField] MeshRenderer _meshRenderer;
        [SerializeField] Material outlineMaterialPrefab;

        [Header( "Debug Only" )]
        [SerializeField] protected float RateOfGauge;
        public float OriginGauge { get { return RateOfGauge * MaxGauge; } }

        /*--- Fields ---*/
        public string InteractionPrompt => prompt;
        public GameObject GetGameObject => gameObject;
        public virtual bool CanInteract { get => !castingSystem.IsCoroutineRunning; }
        protected NetworkBaseController targetController;
        int defaultMatrialCounts;


        /*--- MonoBehaviour Callbacks ---*/
        private void Start()
        {
            defaultMatrialCounts = _meshRenderer.materials.Length;
        }
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
            photonView.RPC( "ShareRate", RpcTarget.All, RateOfGauge );
        }
        protected virtual bool CheckAdditionalCondition( in InteractionPromptUI promptUI )
        {
            return true;
        }

        bool CheckController(in Interactor interactor, in InteractionPromptUI promptUI )
        {
            if(!CanInteract)
            {
                promptUI.Inactivate();
                ActiveOutlineEffect( false );
                return false;
            }

            var controller = interactor.gameObject.GetComponentInParent<NetworkBaseController>();
            if ( controller == null )
            {
                promptUI.Inactivate();
                ActiveOutlineEffect( false );
                return false;
            }
            if ( !controller.IsMine )
            {
                return false;
            }
            //if ( !controller.IsWatching( gameObject ) )
            //{
            //    promptUI.Inactivate();
            //    return false;
            //}
            targetController = controller;
            return true;
        }


        /*--- IInteractable Interfaces ---*/
        public virtual bool ActiveInteractPrompt( Interactor interactor, InteractionPromptUI promptUI )
        {
            if(!CheckController(interactor, promptUI))
            {
                ActiveOutlineEffect( false );
                return false;
            }
            if(!CheckAdditionalCondition( promptUI ) )
            {
                ActiveOutlineEffect( false );
                return false;
            }
            promptUI.Activate( prompt );
            ActiveOutlineEffect( true );
            return true;
        }
        public void InactiveInteractPrompt( InteractionPromptUI promptUI )
        {
            promptUI.Inactivate();
            ActiveOutlineEffect( false );
        }
        public abstract bool Interact( Interactor interactor );

        void AddOutlineEffect()
        {
            if( _meshRenderer.materials.Length == defaultMatrialCounts )
            {
                Material[] tmpMats = new Material[defaultMatrialCounts + 1];
                for ( int i = 0; i < defaultMatrialCounts; ++i )
                {
                    tmpMats[i] = _meshRenderer.materials[i];
                }
                tmpMats[defaultMatrialCounts] = outlineMaterialPrefab;
                _meshRenderer.materials = tmpMats;
            }
        }
        void ActiveOutlineEffect(bool isActive)
        {
            if(isActive)
            {
                AddOutlineEffect();
                _meshRenderer.materials[defaultMatrialCounts].SetFloat( "_Enabled", 1 );
            }
            else
            {
                if( _meshRenderer.materials.Length != defaultMatrialCounts)
                {
                    _meshRenderer.materials[defaultMatrialCounts].SetFloat( "_Enabled", 0 );
                }
            }
        }


        /*--- IPunObservable Interfaces ---*/
        public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info )
        {
        }

        [PunRPC]
        public void ShareRate( float gauge )
        {
            RateOfGauge = gauge;
        }

        [PunRPC]
        public void ShareExorcistInteract( bool interacting )
        {
            IsExorcistInteracting = interacting;
        }

    }
}