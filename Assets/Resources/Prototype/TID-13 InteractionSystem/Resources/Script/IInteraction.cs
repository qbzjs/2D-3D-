using System.Collections.Generic;
using System.Collections;
using UnityEngine;

interface IInteraction 
{
    public void ChargeWithObj(GameObject obj,float chargeVelocity);
    public void ImmediateWithObj( GameObject obj, float chargeVelocity);
    public void ChargeWithOther(float interactionDistance,string tag, float maxChargeValue, float currentValue);
    public void ImmediateWithOther(float interactionDistance, string tag);
}
