using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using TID42;
namespace GHJ_Lib
{
	public class PickupArea: MonoBehaviour
	{
		/*--- Public Fields ---*/
		public FPV_CharacterController1 Exorcist;

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/

		/*--- MonoBehaviour Callbacks ---*/


		private void OnTriggerStay(Collider other)
		{
			if (!other.CompareTag("Doll"))
			{
				return;
			}

			NetworkTPV_CharacterController doll = other.GetComponent<NetworkTPV_CharacterController>();
			if (doll.CurBehavior is BvFall)
			{
				Exorcist.AddPickUpList(other.gameObject);

			}

		}

		private void OnTriggerExit(Collider other)
        {
			if (!other.CompareTag("Doll"))
			{
				return;
			}
			Exorcist.PopPickUpList(other.gameObject);
			
			
		}
        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/


        /*--- Private Methods ---*/
    }
}