using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TempCharacterUI : MonoBehaviour
{
    #region Public Fields
    public Text NotificationGButtonText
    {
        get { return notificationGButtonText; }
        set { notificationGButtonText = value; }
    }
    #endregion	

    #region Private Fields
    [SerializeField]
    Slider chargeBar;
    [SerializeField]
    Text notificationGButtonText;
    #endregion	

    #region MonoBehaviour CallBacks
    void Start()
    {
        chargeBar = GetComponentInChildren<Slider>();
        chargeBar.gameObject.SetActive(false);
        notificationGButtonText = GetComponentInChildren<Text>();
        notificationGButtonText.gameObject.SetActive(false);
        if (chargeBar == null)
        {
            Debug.LogError("Missing Slider");
        }
            Debug.Log("sds");

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
