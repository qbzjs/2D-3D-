using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
    public class BvExpose : Behavior<NetworkDollController>
    {
        protected override void Activate(in NetworkDollController actor)
        {
            actor.StartCoroutineExpose();
        }

        protected override Behavior<NetworkDollController> DoBehavior(in NetworkDollController actor)
        {
            return base.DoBehavior(actor);
        }
    }
}
