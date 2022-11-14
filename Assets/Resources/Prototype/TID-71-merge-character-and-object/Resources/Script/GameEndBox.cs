using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using LSH_Lib;

namespace GHJ_Lib
{
	public class GameEndBox: MonoBehaviour
	{
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag(GameManager.DollTag))
            {
                collision.gameObject.GetComponent<DollController>().ExitCharacterTo_RPC();
            }
        }
   
    }
}