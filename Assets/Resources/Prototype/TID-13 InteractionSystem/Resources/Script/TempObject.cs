using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObject : MonoBehaviour
{
    
    #region Public Fields

    public float GetChargeValueRate
    {
        get { return chargeValue / maxChargeValue; }
    }
    #endregion

    #region Protected Fields
    protected float chargeValue=0;
    protected float maxChargeValue = 10.0f;
    protected float reduction = 0.5f;
    protected bool isAltarEnable = false;
    #endregion	

    #region Private Fields
    #endregion	

    #region MonoBehaviour CallBacks
    #endregion

    #region Public Methods  
    virtual public void Interact(string tag,TempCharacter character,out bool isOnce )
    {
        isOnce = true;
    }


    virtual public void ChargeInteract(TempCharacter character)
    {
        return;
    }

    virtual public void ChargeInteract(float chargeVelocity)
    { }

    virtual public void ImmediateInteract(TempCharacter character)
    {
        return;
    }


    #endregion

    #region Private Methods
    #endregion
}

