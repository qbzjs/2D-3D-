using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyUI_Manager : MonoBehaviour
{

    #region Public Fields
    [Header("Canvases")]
    [SerializeField]
    Canvas mainLobbyCanvas;
    [SerializeField]
    Canvas characterSelectCanvas;
    [SerializeField]
    Canvas matchingCanvas;
    [SerializeField]
    Canvas customRoomCanvas;
    [SerializeField]
    Canvas customRoomLobbyCanvas;

    [Header( "MainLobby Buttons Panel" )]
    [SerializeField]
    GameObject mainLobbyButtons;
    [SerializeField]
    GameObject playButtons;
    [SerializeField]
    GameObject quickMatchButtons;

    [Header( "Character Select" )]
    [SerializeField]
    TextMeshProUGUI roleText;
    #endregion


    #region Private Fields
    #endregion


    #region MonoBehaviour Callbacks
    private void Start()
    {
        EnableCanvasObjects();
        DisableCanvasesAll();
        mainLobbyCanvas.enabled = true;
        EnableMainButtonsPanel();
    }
    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
        {
            mainLobbyCanvas.enabled = true;
        }
        if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
        {
            mainLobbyCanvas.enabled = false;
        }
    }
    #endregion


    #region Public Methods
    public void EnableCharacterSelectCanvas( string roleName )
    {
        DisableCanvasesAll();
        characterSelectCanvas.enabled = true;
        roleText.text = roleName;
    }
    #endregion


    #region Private Methods
    void EnableCanvasObjects()
    {
        mainLobbyCanvas.gameObject.SetActive( true );
        characterSelectCanvas.gameObject.SetActive( true );
        matchingCanvas.gameObject.SetActive( true );
        customRoomCanvas.gameObject.SetActive( true );
        customRoomLobbyCanvas.gameObject.SetActive( true );
    }
    void DisableCanvasesAll()
    {
        mainLobbyCanvas.enabled = false;
        characterSelectCanvas.enabled = false;
        matchingCanvas.enabled = false;
        customRoomCanvas.enabled = false;
        customRoomLobbyCanvas.enabled = false;
    }
    void DisableMainLobbyPanelAll()
    {
        mainLobbyButtons.SetActive( false );
        playButtons.SetActive( false );
        quickMatchButtons.SetActive( false );
    }

    void EnableMainButtonsPanel()
    {
        DisableMainLobbyPanelAll();
        mainLobbyButtons.SetActive( true );
    }
    void EnablePlayButtonsPanel()
    {
        DisableMainLobbyPanelAll();
        playButtons.SetActive( true );
    }
    void EnableQuickMatchButtonsPanel()
    {
        DisableMainLobbyPanelAll();
        quickMatchButtons.SetActive( true );
    }
    void EnableMainLobbyCanvas()
    {
        DisableCanvasesAll();
        mainLobbyCanvas.enabled = true;
    }

    #endregion

}
