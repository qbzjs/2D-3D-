using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class PickupArea: MonoBehaviour
	{
		/*--- Public Fields ---*/
		public NetworkExorcistController Exorcist;

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
				if (doll.CurBehavior is BvGrabbed)
				{
					Exorcist.PopPickUpList(other.gameObject);
				}
				else
				{ 
					Exorcist.AddPickUpList(other.gameObject);
				}

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