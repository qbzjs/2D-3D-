using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
    public class BvBlind<T> : Behavior<T> where T : NetworkTPV_CharacterController
    {
        protected override Behavior<T> DoBehavior(in T actor)
        {
            return null;
        }
    }
}