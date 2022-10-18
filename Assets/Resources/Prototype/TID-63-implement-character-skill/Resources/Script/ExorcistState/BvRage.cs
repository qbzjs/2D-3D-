using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvRage: Behavior<NetworkExorcistController>
	{
		/*--- Public Fields ---*/
        public float RageDuration
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
            actor.StartCoroutineOnRage();
        }

        /*--- Private Methods ---*/
    }
}