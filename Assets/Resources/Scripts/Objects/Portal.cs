using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GHJ_Lib;


namespace KSH_Lib.Object
{
    public class Portal : MonoBehaviour
    {
        private void OnTriggerEnter( Collider other )
        {
            if(other.gameObject.CompareTag(GameManager.DollTag))
            {
                StageManager.Instance.ExitGame( other.gameObject.GetComponent<DollController>() );
            }
        }
    }
}