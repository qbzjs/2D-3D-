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
        public enum GaugedObjType
        {
            Null,
            NormalAltar,
            FinalAltar,
            ExitAltar,
            PurificationBox,
            Cross,
            Trap,
            Door,
        }

        /*--- Serialize Fields ---*/
        [SerializeField] protected string prompt = "Press G to Interact";
        [SerializeField] protected CastingSystem castingSystem;
        [field: SerializeField] public bool IsExorcistInteracting { get; protected set; }

        [field: SerializeField] public float MaxGauge { get; protected set; }
        [field: SerializeField] public float DecreaseRate { get; protected set; }
        [field: SerializeField] public float CoolTime { get; protected set; }

        [SerializeField] MeshRenderer _meshRenderer;
        [SerializeField] Material outlineMaterialPrefab;

        [Header("Effects")]
        [SerializeField] ParticleSystem[] dollParticles;
        [SerializeField] ParticleSystem[] exorcistParticles;

        [Header( "Debug Only" )]
        [SerializeField] protected float RateOfGauge;
        public float OriginGauge { get { return RateOfGauge * MaxGauge; } }

        [SerializeField] Material[] originMats;
        [SerializeField] Material[] outlinedMats;
        [SerializeField] int defaultMatrialCounts;
        [SerializeField] bool isOutlined = false;

        /*--- Fields ---*/
        public string InteractionPrompt => prompt;
        public GameObject GetGameObject => gameObject;
        public virtual bool CanInteract { get => !castingSystem.IsCoroutineRunning; }
        protected NetworkBaseController targetController;

        /*--- MonoBehaviour Callbacks ---*/
        private void Start()
        {
            InitMaterials();
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
            StopDollEffects();
            StopExorcistEffects();
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

        void InitMaterials()
        {
            defaultMatrialCounts = _meshRenderer.materials.Length;

            originMats = _meshRenderer.materials;
            outlinedMats = new Material[defaultMatrialCounts + 1];
            for ( int i = 0; i < defaultMatrialCounts; ++i )
            {
                outlinedMats[i] = _meshRenderer.materials[i];
            }
            outlinedMats[defaultMatrialCounts] = outlineMaterialPrefab;
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

        public void PlayDollEffects()
        {
            foreach(var effect in dollParticles )
            {
                effect.Clear();
                effect.Play();
            }
        }
        public void StopDollEffects()
        {
            foreach ( var effect in dollParticles )
            {
                effect.Stop();
            }
        }
        public void PlayExorcistEffects()
        {
            foreach ( var effect in exorcistParticles )
            {
                effect.Clear();
                effect.Play();
            }
        }
        public void StopExorcistEffects()
        {
            foreach ( var effect in exorcistParticles )
            {
                effect.Stop();
            }
        }

        void ActiveOutlineEffect(bool isActive)
        {
            if(isActive)
            {
                if( !isOutlined )
                {
                    _meshRenderer.materials = outlinedMats;
                    isOutlined = true;
                }
            }
            else
            {
                if( isOutlined )
                {
                    isOutlined = false; 
                    if (_meshRenderer == null)
                    {
                        return;
                    }
                    _meshRenderer.materials = originMats;
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

        [PunRPC]
        public virtual void PlayDollEffects_RPC()
        {
            PlayDollEffects();
        }

        [PunRPC]
        public virtual void StopDollEffects_RPC()
        {
            StopDollEffects();
        }

        [PunRPC]
        public virtual void PlayExorcistEffects_RPC()
        {
            PlayExorcistEffects();
        }

        [PunRPC]
        public virtual void StopExorcistEffects_RPC()
        {
            StopExorcistEffects();
        }

    }
}