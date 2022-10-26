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

        private void OnTriggerStay(Collider other)
        {
			if (other.CompareTag("Doll"))
			{
				other.GetComponent<DollController>().HitDamage((DataManager.Instance.PlayerDatas[0].roleData as ExorcistData).AttackPower);
				this.gameObject.SetActive(false);
			}
        }

        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}