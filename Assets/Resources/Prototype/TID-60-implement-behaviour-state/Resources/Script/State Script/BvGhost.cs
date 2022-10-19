using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
	public class BvGhost : Behavior<NetworkDollController>
	{
		/*--- Public Fields ---*/


		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/
		bool isActive = false;

		/*--- MonoBehaviour Callbacks ---*/

		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/

		protected override Behavior<NetworkDollController> DoBehavior(in NetworkDollController actor)
		{
			if (!isActive)
			{
				DoOnce( actor);
			}
			return null;
		}

		/*--- Private Methods ---*/

		void DoOnce(in NetworkDollController actor)
        {
			
			actor.EscapeGrab();
			actor.BecomeGhost();
			isActive = true;
        }
	}
}