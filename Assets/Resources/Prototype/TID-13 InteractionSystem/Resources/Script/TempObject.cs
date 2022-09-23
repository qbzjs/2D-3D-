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
    #endregion	

    #region MonoBehaviour CallBacks
    void Start()
    {
        ChargeValue = 0.0f;
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
