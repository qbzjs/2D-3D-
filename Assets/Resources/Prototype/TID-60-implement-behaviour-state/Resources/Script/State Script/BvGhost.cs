using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvGhost : Behavior<NetworkTPV_CharacterController>
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/
		bool isActive = false;

		/*--- MonoBehaviour Callbacks ---*/

		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/

		protected override Behavior<NetworkTPV_CharacterController> DoBehavior(in NetworkTPV_CharacterController actor)
		{
			if (!isActive)
			{
				DoOnce( actor);
			}
			return null;
		}

		/*--- Private Methods ---*/

		void DoOnce(in NetworkTPV_CharacterController actor)
        {
			actor.BecomeGhost();
			isActive = true;
        }
	}
}