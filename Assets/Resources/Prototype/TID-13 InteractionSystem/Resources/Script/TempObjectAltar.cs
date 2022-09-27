using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObjectAltar : InteractableObj
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
        
        if (GetChargeRate >= 1.0f&&triggerActiveToDoll)
        {
            triggerActiveToDoll = false;
            triggerActiveToExorcist = false;
        }

        if (triggerActiveToDoll)
        { 
                
            if (curChargeValue > 0)
            {
                curChargeValue -= reduction * Time.deltaTime;
                
            }
            if (curChargeValue < 0)
            {
                curChargeValue = 0.0f;
                
            }
        }     
    }
    #endregion	

    #region Public Methods   

    public override void Interact(string tag, TempCharacter character)
    {
        if (tag == "Exorcist")
        {
            OnceChargeInteract(2.0f);
        }
        else if (tag == "Doll")
        {
            ChargeInteract(character);
        }
    }

    protected override void ChargeInteract(TempCharacter character)
    {
        curChargeValue += character.ChargeVelocity * Time.deltaTime;
    }

    protected override void OnceChargeInteract(float chargeTime)
    {
        SceneManger.Instance.EnableOnceChargeBarUI(chargeTime);
    }



    public void initialValue()
    {
        curChargeValue = 0.0f;
    }
    #endregion	

    #region Private Methods
    #endregion	

}
