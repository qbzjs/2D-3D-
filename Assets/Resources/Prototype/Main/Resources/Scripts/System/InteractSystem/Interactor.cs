using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib.Object
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] protected Transform interactionPoint;
        [SerializeField] protected float interactionPointRadius = 0.5f;
        [SerializeField] protected LayerMask interactableMask;
        [Range(1, 10)][SerializeField] protected int findCapacity = 3;

        [SerializeField] protected int foundCount;
        [SerializeField] protected KeyCode interactionKey;

        [SerializeField] protected InteractionPromptUI interactionPromptUI;

        protected Collider[] colliders;
        IInteractable interactable;


        protected virtual void Awake()
        {
            if(interactionPoint == null)
            {
                interactionPoint = transform;
            }
            colliders = new Collider[findCapacity];
        }


        protected virtual void Update()
        {
            foundCount = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactableMask);

            if(foundCount > 0)
            {
                interactable = colliders[0].GetComponent<IInteractable>();

                if(interactable != null)
                {
                    if(interactable.IsInteractNow)
                    {

                    }

                    interactionPromptUI.Activate(interactable.InteractionPrompt);
                }

                if(interactable != null && Input.GetKeyDown(interactionKey))
                {
                    interactable.Interact(this);
                }
            }
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if(interactionPoint == null)
            {
                return;
            }
            Gizmos.color = new Color32(0, 0, 255, 100);
            Gizmos.DrawSphere(interactionPoint.position, interactionPointRadius);
        }
    }
}
