using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
namespace LSH_Lib{
    public class BvReleased : Behavior<NetworkTPV_CharacterController>
    {
        protected override void Activate(in NetworkTPV_CharacterController actor)
        {
            actor.Released();
        }
    }
}
