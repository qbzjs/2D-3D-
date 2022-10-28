using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace GHJ_Lib
{
    public class ActSkillWolf : Behavior<BasePlayerController>
    {
        /*--- Public Fields ---*/

        /*--- Protected Fields ---*/

        /*--- Private Fields ---*/

        /*--- Public Methods---*/

        /*--- Protected Methods ---*/

        protected override Behavior<BasePlayerController> DoBehavior(in BasePlayerController actor)
        {
            (actor as DollController).DoActiveSkill();
            return null;
        }
        /*--- Private Methods ---*/
    }
}

