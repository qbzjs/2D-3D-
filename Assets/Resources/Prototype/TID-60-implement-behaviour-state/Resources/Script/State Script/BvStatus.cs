using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
public class BvStatus<T> : Behavior<T> where T : NetworkTPV_CharacterController
{
    public Behavior<T> IdleBehavior;
    protected override void Activate(in T actor)
    {
        if (HasSuccessors())
        {
            PassState();
        }
    }

    protected override Behavior<T> DoBehavior(in T actor)
    {
        if (actor.IdleBehavior.HasSuccessors())
        {
            return PassState();
        }
        return null;
    }
}
