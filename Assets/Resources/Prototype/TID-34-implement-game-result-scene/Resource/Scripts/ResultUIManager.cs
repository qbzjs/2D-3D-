using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ResultUIManager : MonoBehaviour
{
    #region Public Fields
    public string NextSceneName;
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
        SceneManager.LoadScene(NextSceneName);
    }
    #endregion

    #region Private Methods
    #endregion
}
