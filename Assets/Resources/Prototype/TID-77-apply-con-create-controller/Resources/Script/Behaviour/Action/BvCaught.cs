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
            actor.SetMoveInput(false);
        }

        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            if (actor.photonView.IsMine)
            {
                if (actor.DoResist())
                {
                    resistGauge += 1.0f;
                }
            }

            if (resistGauge > 10.0f)
            {
                //actor.Escape
            }

            Behavior<NetworkBaseController> Bv =  PassIfHasSuccessor();
            if (Bv is BvPurified)
            {
                return Bv;
            }

            return null;
        }
        /*--- Private Methods ---*/
    }
}