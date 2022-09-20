using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainScene : MonoBehaviour
{
    #region Public Fields
    #endregion


    #region Private Fields
    #endregion


    #region MonoBehaviour Callbacks
    private void Start()
    {
	
    }
    private void Update()
    {
	
    }
    #endregion


    #region Public Methods

    #endregion


    #region Private Methods
    void OnGameStart()
    {
        GameManager.Instance.LoadScene( "02_LobbyScene" );
    }
    #endregion
}
