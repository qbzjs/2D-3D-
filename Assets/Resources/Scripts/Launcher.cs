using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Public Fields
    #endregion


    #region Private Fields
    string gameVersion = "1.0";
    #endregion


    #region MonoBehaviour Callbacks
    private void Start()
    {
	
    }
    private void Update()
    {
	
    }
    #endregion


    #region MonoBehaviourPun Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log( "Launcher: OnConnectedToMaster 호출, 서버 연결 완료" );

    }
    #endregion


    #region Public Methods
    public void OnGameStartButton()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }
    #endregion


    #region Private Methods
    #endregion
}
