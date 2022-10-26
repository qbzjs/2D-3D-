using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class AttackBox: MonoBehaviour
	{
		/*--- Public Fields ---*/
		

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/

        private void OnTriggerStay(Collider other)
        {
			if (other.CompareTag("Doll"))
			{
				other.GetComponent<DollController>().HitDamage(10);
				this.gameObject.SetActive(false);
			}
        }

        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}