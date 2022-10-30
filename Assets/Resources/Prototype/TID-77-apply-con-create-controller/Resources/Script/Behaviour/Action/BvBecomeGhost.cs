using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
    public class BvBecomeGhost : Behavior<NetworkBaseController>
    {
        /*--- Public Fields ---*/

        /*--- Protected Fields ---*/
        
        /*--- Private Fields ---*/

        /*--- Public Methods---*/
      
        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkBaseController actor)
        {
            actor.SetMoveInput(true);
        }
    
        /*--- Private Methods ---*/
    }

}

