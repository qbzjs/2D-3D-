using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

using DEM;

public class LobbyUI_Manager : MonoBehaviourPunCallbacks
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

    [Header( "MainLobby Buttons UI" )]
    [SerializeField]
    GameObject mainLobbyButtons;
    [SerializeField]
    GameObject playButtons;
    [SerializeField]
    GameObject quickMatchButtons;

    [Header( "Character Select UI" )]
    [SerializeField]
    TextMeshProUGUI roleText;

    [Header( "Matching UI" )]
    [SerializeField]
    private string loadSceneName = "02_MainGameScene";
    [SerializeField]
    private TextMeshProUGUI userCntTMP;
    [SerializeField]
    private Sprite refPlayerOnSprite;
    [SerializeField]
    private Sprite refPlayerOffSprite;
    [SerializeField]
    private Image[] playerLoadImgs;
    [SerializeField]
    private GameObject cancleButtonObj;
    [SerializeField]
    private GameObject skipButtonObj;

    [Header( "CustomRoom UI" )]
    [SerializeField]
    TextMeshProUGUI customRoomTypeTMP;
    [SerializeField]
    TextMeshProUGUI actionButtonTMP;
    #endregion


    #region Private Fields
    int curPlayerCnt = 0;
    bool isJoinedRoom = false;
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
        if(isJoinedRoom)
        {
            curPlayerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
            ChangePlayerImage();
            ChangePlayerCountText();

            if ( curPlayerCnt == 5 )
            {
                LoadRoomScene();
            }

            if(PhotonNetwork.IsMasterClient)
            {
                if(skipButtonObj.activeInHierarchy==false)
                {
                    skipButtonObj.SetActive( true );
                }
            }
        }
    }
    #endregion


    #region MonoBehaviourPun Callbacks
    public override void OnJoinRandomFailed( short returnCode, string message )
    {
        base.OnJoinRandomFailed( returnCode, message );
        Debug.Log( "OnJoinRandomFailed Called " + message );
        // Need To Implement fail to Join popup message
    }
    public override void OnCreatedRoom()
    {
        Debug.Log( $"OnCreatedRoom Called, Room Name: {PhotonNetwork.CurrentRoom.Name}" );
    }
    public override void OnCreateRoomFailed( short returnCode, string message )
    {
        base.OnCreateRoomFailed( returnCode, message );
        Debug.Log( "OnCreateRoomFailed Called " + message );
    }
    public override void OnJoinedRoom()
    {
        Debug.Log( "OnJoindRoom Called" );
        EnableMatchingCanvas();
        isJoinedRoom = true;
    }
    public override void OnJoinRoomFailed( short returnCode, string message )
    {
        base.OnJoinRoomFailed( returnCode, message );
        Debug.Log( "OnJoinRoomFailed Called " + message );
    }
    #endregion


    #region Public Methods
    public void EnableCharacterSelectCanvas( string roleName )
    {
        DisableCanvasesAll();
        characterSelectCanvas.enabled = true;
        roleText.text = roleName;

        switch (roleName)
        {
            case "Doll":
            {
                GameManager.Instance.Data.ChangeRole( RoleType.Doll );
            }
            break;
            case "Exorcist":
            {
                GameManager.Instance.Data.ChangeRole( RoleType.Exorcist );
            }
            break;
            default:
            {
                GameManager.Instance.Data.ChangeRole( RoleType.Null );
            }
            break;
        }
    }
    #endregion


    #region Private Methods
    void LoadRoomScene()
    {
        cancleButtonObj.SetActive( false );
        GameManager.Instance.LoadPhotonScene( loadSceneName  );
    }
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
    void ChangePlayerImage()
    {
        for ( int i = 0; i < curPlayerCnt; ++i )
        {
            playerLoadImgs[i].sprite = refPlayerOnSprite;
        }
        for ( int i = curPlayerCnt; i < GameManager.Instance.MaxPlayerCount; ++i )
        {
            playerLoadImgs[i].sprite = refPlayerOffSprite;
        }
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
    void EnableMatchingCanvas()
    {
        DisableCanvasesAll();
        matchingCanvas.enabled = true;
    }
    void EnableCustomRoomCanvas()
    {
        DisableCanvasesAll();
        customRoomCanvas.enabled = true;
    }
    void EnableCustomRoomLobbyCanvas()
    {
        DisableCanvasesAll();
        customRoomLobbyCanvas.enabled = true;
    }

    void ChangePlayerCountText()
    {
        userCntTMP.text = $"{curPlayerCnt} / {GameManager.Instance.MaxPlayerCount}";
    }
    string CreateRandomRoomName()
    {
        return $"RandomRoom{PhotonNetwork.CountOfRooms + 1}";
    }
    void OnMatchingStartButton()
    {
        switch(GameManager.Instance.Data.Role)
        {
            case RoleType.Doll:
            {
                PhotonNetwork.JoinRandomRoom();
            }
            break;
            case RoleType.Exorcist:
            {
                PhotonNetwork.CreateRoom( CreateRandomRoomName(), new RoomOptions { MaxPlayers = GameManager.Instance.MaxPlayerCount } );
            }
            break;
            default:
            {
                Debug.LogError( "LobbyUI_Manager: No Player Role Set, Role is Null." );
            }
            break;
        }
    }
    void OnMatchingCancleButton()
    {
        PhotonNetwork.LeaveRoom();
        isJoinedRoom = false;
        EnableMainLobbyCanvas();
    }
    #endregion


    #region Debug Only
    void OnSkipButtonClicked()
    {
        LoadRoomScene();
    }
    #endregion
}
