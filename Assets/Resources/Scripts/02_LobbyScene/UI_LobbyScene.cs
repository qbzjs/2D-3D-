using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class UI_LobbyScene : MonoBehaviourPunCallbacks
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

    #region MonoBehaviourPun Callbacks
    public override void OnJoinRandomFailed( short returnCode, string message )
    {
        base.OnJoinRandomFailed( returnCode, message );
        Debug.Log("OnJoinRandomFailed Called " + message );
        PhotonNetwork.CreateRoom( CreateRandomRoomName(), new RoomOptions { MaxPlayers = 5 } );
    }
    public override void OnCreatedRoom()
    {
        Debug.Log( $"OnCreatedRoom Called, Room Name: {PhotonNetwork.CurrentRoom.Name}" );
        GameManager.Instance.InputPlayerFaction( true );
    }
    public override void OnCreateRoomFailed( short returnCode, string message )
    {
        base.OnCreateRoomFailed( returnCode, message );
        Debug.Log( "OnCreateRoomFailed Called " + message );
    }

    public override void OnJoinedRoom()
    {
        Debug.Log( "OnJoindRoom Called" );
        GameManager.Instance.InputPlayerFaction( false );
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
        PhotonNetwork.JoinRandomRoom();

    }
    string CreateRandomRoomName(  )
    {
        return $"RandomRoom{PhotonNetwork.CountOfRooms + 1}";
    }

    #endregion
}
