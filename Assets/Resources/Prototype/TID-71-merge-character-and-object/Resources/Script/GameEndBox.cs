using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using LSH_Lib;

namespace GHJ_Lib
{
	public class GameEndBox: MonoBehaviour
	{
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.CompareTag(GameManager.DollTag))
            {
                collider.gameObject.GetComponent<DollController>().ExitCharacter();
            }
        }
   
    }
}