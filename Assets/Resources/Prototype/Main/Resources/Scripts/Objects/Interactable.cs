using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GHJ_Lib;

namespace KSH_Lib
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] protected Transform interactionPoint;
        [SerializeField] protected float interactionPointRadius = 0.5f;
        [SerializeField] protected LayerMask interactableMask;
        [Range(1, 10)][SerializeField] protected int findCapacity;

        protected  Collider[] colliders;
        [SerializeField] protected int foundCount;

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
