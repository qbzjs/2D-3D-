using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempObject : MonoBehaviour
{
    #region Public Fields
    public float ChargeValue
    {
        get{ return chargeValue; }
        set{ chargeValue = value; }
    }
    #endregion

    #region Private Fields
    private float chargeValue;
    private float maxChargeValue = 10.0f;
    #endregion	

    #region MonoBehaviour CallBacks
    void Start()
    {
        chargeValue = 0.0f;
    }

    void Update()
    {
        
    }
    #endregion	

    #region Public Methods
    #endregion	

    #region Private Methods
    #endregion	

}
