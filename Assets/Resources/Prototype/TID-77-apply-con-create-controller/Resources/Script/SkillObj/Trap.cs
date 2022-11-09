using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class Trap: MonoBehaviour
	{
        DollController BeTrappedDoll;
        private void OnTriggerEnter(Collider other)
        {
            if (BeTrappedDoll==null)
            {
                return;                
            }
            if (other.CompareTag(GameManager.DollTag))
            {
                BeTrappedDoll = other.GetComponent<DollController>();
            }
        }
    }
}