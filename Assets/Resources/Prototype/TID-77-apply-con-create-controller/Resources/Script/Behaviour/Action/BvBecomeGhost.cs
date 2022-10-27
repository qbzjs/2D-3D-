using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
    public class BvBecomeGhost : Behavior<BasePlayerController>
    {
        /*--- Public Fields ---*/

        /*--- Protected Fields ---*/
        protected Transform initGhostPos;
        /*--- Private Fields ---*/

        /*--- Public Methods---*/
        public void SetInitGhostPos(Transform transform)
        {
            initGhostPos = transform;
        }
        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            DollController doll = (actor as DollController);
            doll.Escape(initGhostPos,8); //ghost layer = 8;
            doll.BecomeGhost();
        }
    
        /*--- Private Methods ---*/
    }

}

