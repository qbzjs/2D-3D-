using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvSlowExorcist : Behavior<NetworkExorcistController>
	{
		/*--- Public Fields ---*/
		public float Ratio
		{
			get;
			set;
		}

        /*--- Protected Fields ---*/


        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkExorcistController actor)
        {
            actor.StartCoroutineMoveSlow(5, Ratio);
        }

        /*--- Private Methods ---*/
    }
}