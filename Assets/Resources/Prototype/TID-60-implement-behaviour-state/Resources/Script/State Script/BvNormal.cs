using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
    public class BvNormal : Behavior<NetworkTPV_CharacterController>
    {
        protected override void Activate(in NetworkTPV_CharacterController actor)
        {
            Debug.Log("become normal");
        }
    }
}