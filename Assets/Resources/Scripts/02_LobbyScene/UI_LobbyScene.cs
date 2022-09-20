using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

public class UI_LobbyScene : MonoBehaviourPunCallbacks
{
    #region Public Fields
    [Header("Room Option")]
    [SerializeField]
    private byte maxPlayerCount = 5;

    [Header("UI Objects")]
    [SerializeField]
    private GameObject lobbyObj;
    [SerializeField]
    private GameObject lobbyGameObj;
    [SerializeField]
    private GameObject matchingObj;

    [Header("Loading UI Setting")]
    [SerializeField]
    private TextMeshProUGUI userCntTMP; 
    [SerializeField]
    private Sprite refSpritePlayerOn;
    [SerializeField]
    private Sprite refSpritePlayerOff;
    [SerializeField]
    private Image[] images;
    [SerializeField]
    private GameObject cancelButtonObj;

    #endregion


    #region Private Fields
    int curPlayerCnt = 0;
    bool isJoinedRoom = false;
    #endregion


    #region MonoBehaviour Callbacks
    private void Start()
    {
        lobbyObj.SetActive( true );
        lobbyGameObj.SetActive( false );
        matchingObj.SetActive(false);
    }
    private void Update()
    {
        if(isJoinedRoom)
        {
            curPlayerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
            ChangePlayerImage();
            ChangePlayerCountText();

            if (curPlayerCnt == 5)
            {
                cancelButtonObj.SetActive(false);
            }
        }
    }
    #endregion


    #region MonoBehaviourPun Callbacks
    public override void OnJoinRandomFailed( short returnCode, string message )
    {
        base.OnJoinRandomFailed( returnCode, message );
        Debug.Log("OnJoinRandomFailed Called " + message );
        PhotonNetwork.CreateRoom( CreateRandomRoomName(), new RoomOptions { MaxPlayers = maxPlayerCount } );
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
        ActiveMatchingUI();
        isJoinedRoom = true;
    }
    public override void OnJoinRoomFailed( short returnCode, string message )
    {
        base.OnJoinRoomFailed( returnCode, message );
        Debug.Log( "OnJoinRoomFailed Called " + message );
    }
    #endregion


    #region Public Methods
    #endregion


    #region Private Methods
    void ResetUI()
    {
        lobbyObj.SetActive(false);
        lobbyGameObj.SetActive(false);
        matchingObj.SetActive(false);
    }
    void ActiveMatchingUI()
    {
        ResetUI();
        matchingObj.SetActive(true);
    }
    void ChangePlayerImage()
    {
        for(int i = 0; i < curPlayerCnt; ++i)
        {
            images[i].sprite = refSpritePlayerOn;
        }
        for(int i = curPlayerCnt; i < maxPlayerCount; ++i )
        {
            images[i].sprite = refSpritePlayerOff;
        }
    }
    void ChangePlayerCountText()
    {
        userCntTMP.text = $"{curPlayerCnt} / {maxPlayerCount}";
    }
    string CreateRandomRoomName(  )
    {
        return $"RandomRoom{PhotonNetwork.CountOfRooms + 1}";
    }
    #endregion


    #region Button Callbacks
    void OnPlayButton()
    {
        ResetUI();
        lobbyGameObj.SetActive(true);
    }
    void OnReturnButton()
    {
        ResetUI();
        lobbyObj.SetActive(true);
    }
    void OnMatchingCancelButton()
    {
        PhotonNetwork.LeaveRoom();
        ResetUI();
        lobbyGameObj.SetActive(true);
        isJoinedRoom = false;
    }
    void OnQuickMatchButton()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    #endregion
}
