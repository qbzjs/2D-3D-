using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TempCharacterUI : MonoBehaviour
{
    #region Public Fields
    #endregion	

    #region Private Fields
    [SerializeField]
    Slider ChargeBar;
    #endregion	

    #region MonoBehaviour CallBacks
    void Start()
    {
        ChargeBar = GetComponentInChildren<Slider>();
        if (ChargeBar == null)
        {
            Debug.LogError("Missing Slider");
        }
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
