using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
	public class AttackBox: MonoBehaviour
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/

        /*--- Private Fields ---*/


        /*--- MonoBehaviour Callbacks ---*/

        protected virtual void OnTriggerStay(Collider other)
        {
			if (other.CompareTag("Doll"))
			{
				other.GetComponent<DollController>().HitFrom();
				this.gameObject.SetActive(false);
			}
        }

        /*--- Public Methods ---*/
  
        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}