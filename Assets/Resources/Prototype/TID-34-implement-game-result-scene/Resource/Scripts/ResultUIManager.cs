using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ResultUIManager : MonoBehaviour
{
    #region Public Fields
    #endregion

    #region Private Fields
    #endregion

    #region MonoBehaviour Callbacks
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    #endregion

    #region Public Methods
    public void ReturnToHome()
    {
        SceneManager.LoadScene("01_MainLobbyScene");
    }
    #endregion

    #region Private Methods
    #endregion
}
