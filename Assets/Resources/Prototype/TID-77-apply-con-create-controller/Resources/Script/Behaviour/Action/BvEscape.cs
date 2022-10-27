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
        protected Transform escapePos;
        /*--- Private Fields ---*/

        /*--- Public Methods---*/
        public void SetEscapePos(Transform transform)
        {
            escapePos = transform;
        }
        /*--- Protected Methods ---*/
        protected override void Activate(in BasePlayerController actor)
        {
            if (escapePos == null)
            {
                Debug.LogError("you Do not SetEscapePos");
                return;
            }
            //애니매이션이 있다면 그에따라 pos를 변경.
            (actor as DollController).Escape(escapePos);
        }
        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            //애니매이션이 있다면 애니매이션이 끝날때 new BvIdle() 를 반환.
            return new BvIdle();
        }
        /*--- Private Methods ---*/
    }

}

