using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SceneManger : MonoBehaviour
{
    #region Public Fields
    public static SceneManger Instance;
    public GameObject UIPrefab;
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
    public void EnableGButton()
    {
        playerUI.NotificationGButtonText.gameObject.SetActive(true);
    }

    public void DisableGButton()
    {
        playerUI.NotificationGButtonText.gameObject.SetActive(false);
    }
    #endregion	

    #region Private Methods
    #endregion	

}
