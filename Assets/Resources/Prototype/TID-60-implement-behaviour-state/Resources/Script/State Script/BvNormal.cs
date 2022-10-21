using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
    public class BvNormal : Behavior<NetworkDollController>
    {
        protected override void Activate(in NetworkDollController actor)
        {
            //Debug.Log("become normal");
            actor.BecomeIdle();
        }
    }
}