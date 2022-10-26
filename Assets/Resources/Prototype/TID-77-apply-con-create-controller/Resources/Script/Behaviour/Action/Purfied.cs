using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
    public class Purfied : Behavior<BasePlayerController>
    {
        /*--- Public Fields ---*/

        /*--- Protected Fields ---*/

        /*--- Private Fields ---*/

        /*--- Public Methods---*/

        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            // Sound Play?
        }

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            //Devil HP down
            //if(Devil HP<=0)
            {
                //return Ghost
            }
            return null;
        }
        /*--- Private Methods ---*/
    }

}

