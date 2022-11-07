using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GHJ_Lib;
namespace KSH_Lib.Object
{
    public class Interactor : MonoBehaviour
    {
        public IInteractable Interactable { get; private set; }
        [SerializeField] protected Transform interactionPoint;
        [SerializeField] protected float interactionPointRadius = 0.5f;
        [SerializeField] protected LayerMask interactableMask;
        [Range(1, 10)][SerializeField] protected int findCapacity = 3;

        [SerializeField] protected int foundCount;
        [SerializeField] protected KeyCode interactionKey;

        [SerializeField] protected InteractionPromptUI interactionPromptUI;
        [SerializeField] NetworkBaseController controller;

        protected Collider[] colliders;
        

        protected virtual void OnEnable()
        {
            if ( interactionPromptUI == null )
            {
                interactionPromptUI = GHJ_Lib.StageManager.Instance.InteractionPrompt;
                if ( interactionPromptUI == null )
                {
                    Debug.LogError( "GuageObject.Enable: Can not find textUI" );
                }
            }
            if (interactionPoint == null)
            {
                interactionPoint = transform;
            }
            colliders = new Collider[findCapacity];
        }
        protected virtual void Update()
        {
            if(controller.IsMine)
            {
                TryInteract();
            }
        }
        protected virtual void OnDrawGizmosSelected()
        {
            if ( interactionPoint == null )
            {
                return;
            }
            Gizmos.color = new Color32( 0, 0, 255, 100 );
            Gizmos.DrawSphere( interactionPoint.position, interactionPointRadius );
        }

        protected virtual void TryInteract()
        {
            foundCount = Physics.OverlapSphereNonAlloc( interactionPoint.position, interactionPointRadius, colliders, interactableMask );

            if ( foundCount > 0 )
            {
                Interactable = colliders[0].GetComponentInParent<IInteractable>();

                if ( Interactable != null )
                {
                    bool canInteract = Interactable.ActiveInteractPrompt( this, interactionPromptUI );

                    if ( canInteract && Input.GetKeyDown( interactionKey ) )
                    {
                        Interactable.Interact( this );
                    }
                }
                else
                {
                    interactionPromptUI.Inactivate();
                }
            }
            else
            {
                Interactable = null;
                interactionPromptUI.Inactivate();
            }
        }
    }
}
