using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KSH_Lib
{
    public abstract class TriggerArea<T> : MonoBehaviour where T: MonoBehaviour
    {
        [SerializeField]
        protected T target;

        protected virtual void Start()
        {
            if(target == null)
            {
                target = transform.root.gameObject.GetComponent<T>();
                if ( target == null )
                {
                    Debug.LogError( "TriggerArea.Start: gaugedObj is null" );
                }
            }
        }
        protected virtual void OnTriggerEnter( Collider other )
        {
            if( target == null)
            {
                Debug.LogError( "TriggerArea.OnTriggerEnter: gaugedObj is null" );
                return;
            }
        }
        protected virtual void OnTriggerStay( Collider other )
        {
            if ( target == null )
            {
                Debug.LogError( "TriggerArea.OnTriggerStay: gaugedObj is null" );
                return;
            }
        }
        protected virtual void OnTriggerExit( Collider other )
        {
            if ( target == null )
            {
                Debug.LogError( "TriggerArea.OnTriggerExit: gaugedObj is null" );
                return;
            }
        }
    }
}