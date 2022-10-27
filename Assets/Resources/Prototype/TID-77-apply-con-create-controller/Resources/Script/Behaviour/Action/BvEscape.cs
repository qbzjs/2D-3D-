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
            //�ִϸ��̼��� �ִٸ� �׿����� pos�� ����.
            (actor as DollController).Escape(escapePos);
        }
        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            //�ִϸ��̼��� �ִٸ� �ִϸ��̼��� ������ new BvIdle() �� ��ȯ.
            return new BvIdle();
        }
        /*--- Private Methods ---*/
    }

}

