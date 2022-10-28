using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
	public class BvCaught: Behavior<NetworkBaseController>
	{
        /*--- Public Fields ---*/


        /*--- Protected Fields ---*/
        protected float resistGauge = 0.0f;

        /*--- Private Fields ---*/


        /*--- Public Methods ---*/


        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkBaseController actor)
        {
            
            resistGauge = 0.0f;

        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {

            Behavior<NetworkBaseController> Bv =  PassIfHasSuccessor();
            if (Bv is BvPurified)
            {
                return Bv;
            }
            if (resistGauge > 1.0f)
            {
                //escape
            }

            return null;
        }
        /*--- Private Methods ---*/
    }
}