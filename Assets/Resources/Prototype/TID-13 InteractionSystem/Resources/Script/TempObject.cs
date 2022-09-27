using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObject : MonoBehaviour
{
    #region Public Fields
    public bool triggerActiveToDoll = true;
    public bool triggerActiveToExorcist = true;
    public float GetChargeValueRate
    {
        get { return chargeValue / maxChargeValue; }
    }
    #endregion

    #region Protected Fields
    protected float chargeValue=0;
    protected float maxChargeValue = 10.0f;
    protected float reduction = 0.5f;
    #endregion	

    #region Private Fields
    #endregion	

    #region MonoBehaviour CallBacks
    #endregion

    #region Public Methods  
    virtual public void Interact(string tag,TempCharacter character,out bool isOnce )
    {
        bool isOncebehave = false;

        if (tag == "Exorcist")
        {
            OnceChargeInteract(2.0f);
            isOncebehave = true;
        }
        else if (tag == "Doll")
        {
            ChargeInteract(character);
            isOncebehave = false;
        }

        isOnce = isOncebehave;

    }


    virtual public void ChargeInteract(TempCharacter character)
    {
        chargeValue += character.ChargeVelocity * Time.deltaTime;
    }

    virtual public void ChargeInteract(float chargeVelocity)
    {
        chargeValue += chargeVelocity * Time.deltaTime;
    }

    virtual public void OnceChargeInteract(float chargeTime)
    {
        SceneManger.Instance.EnableOnceChargeBarUI(chargeTime);
    }

    virtual public void ImmediateInteract(TempCharacter character)
    {
        this.gameObject.transform.SetParent(character.transform);
    }


    #endregion

    #region Private Methods
    #endregion
}

