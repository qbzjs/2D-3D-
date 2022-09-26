using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObjectAltar : TempObject
{
    #region Public Fields
    #endregion

    #region Private Fields
    #endregion	

    #region MonoBehaviour CallBacks
    void Start()
    {
        initialValue();
    }

    void Update()
    {
        
        if (GetChargeValueRate >= 1.0f)
        {
            isAltarEnable = true;
           
        }

        if (!isAltarEnable)
        { 
                
            if (chargeValue > 0)
            {
                chargeValue -= reduction * Time.deltaTime;
                
            }
            if (chargeValue < 0)
            {
                chargeValue = 0.0f;
                
            }
        }     
    }
    #endregion	

    #region Public Methods   

    public override void Interact(string tag, TempCharacter character, out bool isOnce)
    {
        bool isOncebehave=false;
        
        if (tag == "Exorcist")
        {
            ChargeInteract(5.0f);
            isOncebehave = true;
        }
        else if (tag == "Doll")
        {
            ChargeInteract(character);
            isOncebehave = false;
        }

        isOnce = isOncebehave;
    }

    public override void ChargeInteract(TempCharacter character)
    {
        chargeValue += character.ChargeVelocity * Time.deltaTime;
    }

    public override void ChargeInteract(float chargeTime)
    {
        SceneManger.Instance.EnableOnceChargeBarUI(chargeTime);
    }

    public override void ImmediateInteract(TempCharacter character)
    {
        
    }
    public void ChargeByDoll(float chargeVelocity)
    {
        chargeValue += chargeVelocity * Time.deltaTime;
    }

    public void initialValue()
    {
        chargeValue = 0.0f;
    }
    #endregion	

    #region Private Methods
    #endregion	

}
