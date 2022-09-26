using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SceneManger : MonoBehaviour
{
    #region Public Fields
    public static SceneManger Instance;
    public GameObject UIPrefab;
    public bool IsCoroutine = false;
    #endregion

    #region Private Fields
    private List<GameObject> objList;
    private TempCharacterUI playerUI;
    #endregion

    #region MonoBehaviour CallBacks
    void Start()
    {
        Instance = this;
        if (UIPrefab == null)
        {
            Debug.LogError("Missing Prefab");
        }

        playerUI = Instantiate(UIPrefab).GetComponent<TempCharacterUI>();
    }

    void Update()
    {
        
    }
    #endregion	

    #region Public Methods
    public void EnableInteractionUI()
    {
        playerUI.ActiveInteractionText();
    }

    public void DisableInteractionUI()
    {
        playerUI.DeactiveInteractionText();
    }

    public void EnableBarUI(GameObject obj)
    {
        playerUI.ActiveChargeBar(obj);   
    }

    public void DisableBarUI()
    {
        playerUI.DeactiveChargeBar();
    }

    public void EnableOnceChargeBarUI(float chargeTime)
    {
        playerUI.ActiveFixedChargeBar(chargeTime);
    }

  

    #endregion

    #region Private Methods
    #endregion

    #region Interface Interaction





    #endregion
}
