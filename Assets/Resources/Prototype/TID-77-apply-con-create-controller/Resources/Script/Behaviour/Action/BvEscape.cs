using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
    public class BvEscape : Behavior<NetworkBaseController>
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
        protected override void Activate(in NetworkBaseController actor)
        {
            if (escapePos == null)
            {
                Debug.LogError("you Do not SetEscapePos");
                return;
            }
            //�ִϸ��̼��� �ִٸ� �׿����� pos�� ����.
            (actor as DollController).Escape(escapePos,0); //Default layer = 0;
        }
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            //�ִϸ��̼��� �ִٸ� �ִϸ��̼��� ������ �Լ����� �� ��ȯ.
            return new BvIdle();
        }
        /*--- Private Methods ---*/
    }

}

