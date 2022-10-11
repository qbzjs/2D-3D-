using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
    public class BvExpose : Behavior<NetworkTPV_CharacterController>
    {
        protected override void Activate(in NetworkTPV_CharacterController actor)
        {
            actor.StartCoroutineExpose();
        }

        protected override Behavior<NetworkTPV_CharacterController> DoBehavior(in NetworkTPV_CharacterController actor)
        {
            return base.DoBehavior(actor);
        }
    }
}
