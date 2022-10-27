using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
    public class BvEscape : Behavior<BasePlayerController>
    {
        /*--- Public Fields ---*/

        /*--- Protected Fields ---*/

        /*--- Private Fields ---*/

        /*--- Public Methods---*/

        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            
        }
        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            return new BvIdle();
        }
        /*--- Private Methods ---*/
    }

}

