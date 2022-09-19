using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class UI_LobbyScene : MonoBehaviour
{
    #region Public Fields

    [SerializeField]
    private GameObject lobbyObj;
    [SerializeField]
    private GameObject lobbyGameObj;


    #endregion


    #region Private Fields
    #endregion


    #region MonoBehaviour Callbacks
    private void Start()
    {
        lobbyObj.SetActive( true );
        lobbyGameObj.SetActive( false );
    }
    private void Update()
    {
	
    }
    #endregion


    #region Public Methods
    #endregion


    #region Private Methods
    void OnPlayButton()
    {
        lobbyObj.SetActive( false );
        lobbyGameObj.SetActive( true );
    }

    void OnReturnButton()
    {
        lobbyObj.SetActive( true );
        lobbyGameObj.SetActive( false );
    }

    void OnQuickMatchButton()
    {

    }


    #endregion
}
