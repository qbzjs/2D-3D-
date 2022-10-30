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
        
        /*--- Private Fields ---*/

        /*--- Public Methods---*/
      
        /*--- Protected Methods ---*/
        protected override void Activate(in NetworkBaseController actor)
        {
            //actor.Escape(escapePos,0); //Default layer = 0;
            actor.SetMoveInput(false);
        }
        protected override Behavior<NetworkBaseController> DoBehavior(in NetworkBaseController actor)
        {
            //�ִϸ��̼��� �ִٸ� �ִϸ��̼��� ������ �Լ����� �� ��ȯ.
            return new BvIdle();
        }
        /*--- Private Methods ---*/
    }

}

