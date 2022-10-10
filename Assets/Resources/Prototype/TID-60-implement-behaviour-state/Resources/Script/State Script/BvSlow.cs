using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;

namespace GHJ_Lib
{
    public class BvSlow<T> : Behavior<T> where T : NetworkTPV_CharacterController
    {
        protected override void Activate(in T actor)
        {
            Debug.Log("Slow Rabbit");
            actor.StartCoroutineSlow();
        }

        protected override Behavior<T> DoBehavior(in T actor)
        {
            Debug.Log("Slow Do~");
            return null;
        }
    }
}
