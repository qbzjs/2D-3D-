using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObj : MonoBehaviour
{
    #region Public Fields
    public bool triggerActiveToDoll = true;
    public bool triggerActiveToExorcist = true;
    public float GetChargeRate
    {
        get { return curChargeValue / maxChargeValue; }
    }
    #endregion


    #region Protected Fields
    [SerializeField]
    protected float maxChargeValue = 10.0f;
    [SerializeField]
    protected float reduction = 0.5f;
    //protected bool canActive = true;
    protected float curChargeValue=0;
    #endregion	


    #region Public Methods  
    virtual public void Interact(string tag, TempCharacter character )
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
    #endregion

    #region Protected Methods
    virtual protected void ChargeInteract( TempCharacter character )
    {
        curChargeValue += character.ChargeVelocity * Time.deltaTime;
    }
    virtual protected void ChargeInteract( float chargeVelocity )
    {
        curChargeValue += chargeVelocity * Time.deltaTime;
    }
    virtual protected void OnceChargeInteract( float chargeTime )
    {
        SceneManger.Instance.EnableOnceChargeBarUI( chargeTime );
    }
    virtual protected void ImmediateInteract( TempCharacter character )
    {
        this.gameObject.transform.SetParent( character.transform );
    }
    #endregion
}

