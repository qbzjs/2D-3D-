using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Pun;
using Photon.Realtime;

using KSH_Lib;
using KSH_Lib.Data;
using KSH_Lib.UI;
using LSH_Lib;

using DEM;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace KSH_Lib
{
    public class LobbyUI_Manager : MonoBehaviourPunCallbacks
    {
        public enum RoomType { Null, QuickMatch, Custom }

        /*--- Serialized Fields ---*/
        [field: SerializeField] public string LoadSceneName{get; private set;}

        [Header( "Canvases" )]
        [SerializeField]
        Canvas mainLobbyCanvas;
        [SerializeField]
        Canvas matchingCanvas;
        [SerializeField]
        Canvas customRoomCanvas;
        [SerializeField]
        Canvas searchCustomCanvas;
        [SerializeField]
        Canvas informationCanvas;
        [SerializeField] CanvasGroup mainCanvasGroup;


        [Header( "MainLobby Buttons UI" )]
        [SerializeField]
        GameObject mainLobbyButtons;
        //[SerializeField]
        //GameObject playButtons;
        [SerializeField]
        GameObject quickMatchButtons;
        [SerializeField] string customRoomSceneName = "03_RoomScene";

        //>>Suhyeon
        [Header("Information UI")]
        [SerializeField]
        GameObject informationButtons;
        [SerializeField]
        GameObject dollsInformation;
        [SerializeField]
        GameObject exorcistInformation;

        [Header("Dolls Information UI")]
        [SerializeField]
        GameObject rabbitInformation;
        [SerializeField]
        GameObject turtleInformation;
        [SerializeField]
        GameObject wolfInformation;
        [SerializeField]
        GameObject penguinInformation;

        [Header("Developer UI")]
        [SerializeField]
        GameObject developerCanvas;

        [Header("Option UI")]
        [SerializeField]
        GameObject optionCanvas;
        //<<Suhyeon

        [Header( "Character Select UI" )]
        [SerializeField]
        TextMeshProUGUI roleTextTMP;


        [Header( "CustomRoom UI" )]
        [SerializeField]
        TextMeshProUGUI customRoomTypeTMP;
        [SerializeField]
        TextMeshProUGUI actionButtonTMP;


        [Header( "For Debug" )]
        public GameObject DebugUIObj;
        public TextMeshProUGUI RoomNameTMP;
        public RoomType roomType;


        /*--- Private Fields ---*/
        public bool IsJoinedRoom { get; private set; }

        CharacterSelectCanvasController charaSelectCanvasController;


        /*--- MonoBehaviour Callbacks ---*/
        private void Start()
        {
            mainCanvasGroup.alpha = 0.0f;
            mainCanvasGroup.LeanAlpha(1.0f, 1.0f);
            EnableCanvasObjects();
            DisableCanvasesAll();
            mainLobbyCanvas.enabled = true;
            EnableMainButtonsPanel();
            UIAudioManager.instance.Play("LobbyBGM");
            //PhotonNetwork.AutomaticallySyncScene = false;
            // Deubg
            //skipButtonObj.SetActive( true );
            //lshSkipButtonObj.SetActive( false );
            //kshSkipButtonObj.SetActive( false );
        }
        //private void Update()
        //{
            //    if ( isJoinedRoom )
            //    {
            //        GameManager.Instance.CurPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            //        ChangePlayerImage();
            //        ChangePlayerCountText();

            //        if ( PhotonNetwork.IsMasterClient )
            //        {
            //            if (GameManager.Instance.CurPlayerCount == 5 )
            //            {
            //                LoadRoomScene();
            //            }

            //            if ( skipButtonObj.activeInHierarchy == false )
            //            {
            //                skipButtonObj.SetActive( true );
            //                lshSkipButtonObj.SetActive( true );
            //                kshSkipButtonObj.SetActive( true );
            //            }
            //        }
            //        }
        //}


        /*--- MonoBehaviourPun Callbacks ---*/
        public override void OnJoinRandomFailed( short returnCode, string message )
        {
            base.OnJoinRandomFailed( returnCode, message );
            //Debug.Log( "OnJoinRandomFailed Called " + message );
            // Need To Implement fail to Join popup message
        }
        public override void OnCreatedRoom()
        {
            //add new script
            PhotonNetwork.CurrentRoom.SetCustomProperties( new Hashtable { { "roomNumber", GameManager.Instance.GetRoomNumber() } } );
            //
            //Debug.Log( $"OnCreatedRoom Called, Room Name: {PhotonNetwork.CurrentRoom.Name}" );
        }
        public override void OnCreateRoomFailed( short returnCode, string message )
        {
            base.OnCreateRoomFailed( returnCode, message );
            //Debug.Log( "OnCreateRoomFailed Called " + message );
        }
        public override void OnJoinedRoom()
        {
            //Debug.Log( "OnJoindRoom Called" );

            DataManager.Instance.SetPlayerIdx();
            IsJoinedRoom = true;
            if (roomType == RoomType.QuickMatch)
            {
                EnableMatchingCanvas();
            }
            else if (roomType == RoomType.Custom)
            {
                
                DataManager.Instance.InitPlayerDatasCustomRoom();
                DataManager.Instance.ShareAccountData();
                GameManager.Instance.LoadScene(customRoomSceneName);
            }
        }
        public override void OnJoinRoomFailed( short returnCode, string message )
        {
            base.OnJoinRoomFailed( returnCode, message );
            //Debug.Log( "OnJoinRoomFailed Called " + message );
        }
        public override void OnPlayerEnteredRoom( Player newPlayer )
        {
            DataManager.Instance.SetPlayerIdx();
        }
        public override void OnPlayerLeftRoom( Player otherPlayer )
        {
            DataManager.Instance.SetPlayerIdx();
        }

        public override void OnLeftRoom()
        {
            //Debug.Log( "OnLeftRoom Called" );
            DataManager.Instance.ResetLocalRoleData();
            IsJoinedRoom = false;
            EnableMainLobbyCanvas();
        }

        /*--- Public Methods ---*/
        //public void EnableCharacterSelectCanvas( string roleName )
        //{
        //    DisableCanvasesAll();
        //    roleText.text = roleName;

        //    switch ( roleName )
        //    {
        //        case "Doll":
        //        {
        //            DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Doll;
        //            charaSelectCanvasController.SendMessage( "OnSelectRole" );
        //        }
        //        break;
        //        case "Exorcist":
        //        {
        //            DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Exorcist;
        //            charaSelectCanvasController.SendMessage( "OnSelectRole" );
        //        }
        //        break;
        //        default:
        //        {
        //            //GameManager.Instance.Data.ChangeRole( RoleType.Null );
        //        }
        //        break;
        //    }
        //}

        /*--- Private Methods ---*/

        //void LoadRoomScene()
        //{
        //    cancelButtonObj.SetActive(false);
        //    PhotonNetwork.CurrentRoom.IsOpen = false;
        //    GameManager.Instance.LoadPhotonScene(loadSceneName);
        //}
        void EnableCanvasObjects()
        {
            mainLobbyCanvas.gameObject.SetActive( true );
            matchingCanvas.gameObject.SetActive( true );
            customRoomCanvas.gameObject.SetActive( true );
            searchCustomCanvas.gameObject.SetActive( true );
            informationCanvas.gameObject.SetActive(true);   
        }
        public void DisableCanvasesAll()
        {
            mainLobbyCanvas.enabled = false;
            matchingCanvas.enabled = false;
            customRoomCanvas.enabled = false;
            searchCustomCanvas.enabled = false;
            informationCanvas.enabled = false;
            
        }
        void DisableMainLobbyPanelAll()
        {
            mainLobbyButtons.SetActive( false );
            //playButtons.SetActive( false );
            quickMatchButtons.SetActive( false );
            informationButtons.SetActive(false);
            dollsInformation.SetActive(false);
            exorcistInformation.SetActive(false);
            rabbitInformation.SetActive(false);
            turtleInformation.SetActive(false);
            wolfInformation.SetActive(false);
            penguinInformation.SetActive(false);

        }
        //void ChangePlayerImage()
        //{
        //    for (int i = 0; i < GameManager.Instance.CurPlayerCount; ++i)
        //    {
        //        playerLoadImgs[i].sprite = refPlayerOnSprite;
        //    }
        //    for (int i = GameManager.Instance.CurPlayerCount; i < GameManager.Instance.MaxPlayerCount; ++i)
        //    {
        //        playerLoadImgs[i].sprite = refPlayerOffSprite;
        //    }
        //}

        void EnableMainButtonsPanel()
        {
            DisableMainLobbyPanelAll();
            mainLobbyButtons.SetActive( true );
        }
        void EnablePlayButtonsPanel()
        {
            DisableMainLobbyPanelAll();
            //playButtons.SetActive( true );
        }
        void EnableQuickMatchButtonsPanel()
        {
            DisableMainLobbyPanelAll();
            quickMatchButtons.SetActive( true );
        }
        //>>suhyeon
        void EnableInformationButtonPanel()
        {
            DisableMainLobbyPanelAll();
            informationButtons.SetActive(true);
        }
        void EnableInformationCanvas()
        {
            DisableCanvasesAll();
            informationCanvas.enabled = true;
        }
        void EnableDeveloperCanvas()
        {
            developerCanvas.SetActive(true);
        }
        void DisableDeveloperCanvas()
        {
            developerCanvas.SetActive(false);
        }
        void EnableOptionCanvas()
        {
            optionCanvas.SetActive(true);
        }
        void DisableOptionCanvas()
        {
            optionCanvas.SetActive(false);
        }
        //
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
        void EnableCustomSearchCanvas()
        {
            DisableCanvasesAll();
            searchCustomCanvas.enabled = true;
        }
        
        //<<suhyeon
        
        void EnableDollInformationPanel()
        {
            DisableMainLobbyPanelAll();
            dollsInformation.SetActive(true);
        }
        void EnbaleExorcistInformationPanel()
        {
            DisableMainLobbyPanelAll();
            exorcistInformation.SetActive(true);
        }
        void EnableRabbitInformation()
        {
            DisableMainLobbyPanelAll();
            rabbitInformation.SetActive(true);
        }
        void EnableTurtleInformation()
        {
            DisableMainLobbyPanelAll();
            turtleInformation.SetActive(true);
        }
        void EnableWolfInformation()
        {
            DisableMainLobbyPanelAll();
            wolfInformation.SetActive(true);
        }
        void EnablePenguinInformation()
        {
            DisableMainLobbyPanelAll();
            penguinInformation.SetActive(true);
        }

        //void ChangePlayerCountText()
        //{
        //    userCntTMP.text = $"{GameManager.Instance.CurPlayerCount} / {GameManager.Instance.MaxPlayerCount}";
        //}
        string CreateRandomRoomName()
        {
            return $"RandomRoom{PhotonNetwork.CountOfRooms + 1}";
        }
        //void OnMatchingStartButton()
        //{
        //    if(DataManager.Instance.PreRoleType == RoleData.RoleType.Null)
        //    {
        //        Debug.LogWarning( "LobbyUI_Manager: Need to Select role" );
        //        return;
        //    }

        //    DataManager.Instance.InitLocalRoleData();


        //    switch ( DataManager.Instance.PreRoleType )
        //    {
        //        case RoleData.RoleType.Doll:
        //        {
        //            //PhotonNetwork.JoinRandomRoom();
        //            PhotonNetwork.JoinRoom( roomName );
        //        }
        //        break;
        //        case RoleData.RoleType.Exorcist:
        //        {
        //            PhotonNetwork.CreateRoom( roomName, new RoomOptions { MaxPlayers = GameManager.Instance.MaxPlayerCount });
        //            //PhotonNetwork.CreateRoom( CreateRandomRoomName(), new RoomOptions { MaxPlayers = GameManager.Instance.MaxPlayerCount } );
        //        }
        //        break;
        //        default:
        //        {
        //            Debug.LogWarning( "LobbyUI_Manager: Need to Select role" );
        //        }
        //        break;
        //    }
        //}
        public void OnMatchingStartButton(string roleType)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            if (roleType == "Exorcist")
            {
                DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Exorcist;
                //PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = GameManager.Instance.MaxPlayerCount });
                //PhotonNetwork.CreateRoom( RoomNameTMP.text , new RoomOptions { MaxPlayers = GameManager.Instance.MaxPlayerCount } );

                if( DebugUIObj.activeInHierarchy)
                {
                    PhotonNetwork.CreateRoom( RoomNameTMP.text, new RoomOptions { MaxPlayers = GameManager.Instance.MaxPlayerCount } );
                }
                else
                {
                    PhotonNetwork.CreateRoom( null );
                }
            }
            else if (roleType == "Doll")
            {
                DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Doll;
                //PhotonNetwork.JoinRoom( RoomNameTMP.text );
                if ( DebugUIObj.activeInHierarchy )
                {
                    PhotonNetwork.JoinRoom( RoomNameTMP.text );
                }
                else
                {
                    PhotonNetwork.JoinRandomRoom();
                }
            }
            else 
            {
                Debug.LogWarning("LobbyUI_Manager: Need to Select role");
                return;
            }
            roomType = RoomType.QuickMatch;
            //EnableMatchingCanvas();
        }
        void OnMatchingCancelButton()
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            if (PhotonNetwork.IsMasterClient)
            {
                //photonView.RPC( "LeaveRoomRPC", RpcTarget.Others );
                
                for(int i = 1; i < PhotonNetwork.PlayerList.Length; ++i )
                {
                    PhotonNetwork.CloseConnection( PhotonNetwork.PlayerList[i] );
                }
            }
            PhotonNetwork.LeaveRoom();
            
            //IsJoinedRoom = false;
            //matchingUIController.isJoinedRoom = false;
            //EnableMainLobbyCanvas();
        }
        



        /*--- Debug Methods ---*/
        //void OnSkipButtonClicked()
        //{
        //    LoadRoomScene();
        //}
        //void OnLSHButtonClicked()
        //{
        //    loadSceneName = lshSceneName;
        //    OnSkipButtonClicked();
        //}
        //void OnKSHButtonClicked()
        //{
        //    loadSceneName = kshSceneName;
        //    OnSkipButtonClicked();
        //}

        /*---Debug Server ---*/
        void CreateDebugServer()
        {
            DataManager.Instance.InitLocalRoleData();
            PhotonNetwork.CreateRoom( "DebugServer2", new RoomOptions { MaxPlayers = GameManager.Instance.MaxPlayerCount } );
            //GameManager.Instance.Data.ChangeRole( RoleType.Exorcist );
        }
        void JoinDebugServer()
        {
            DataManager.Instance.InitLocalRoleData();
            PhotonNetwork.JoinRoom( "DebugServer2" );
            //GameManager.Instance.Data.ChangeRole( RoleType.Doll );
        }

        void QuitApplication()
        {
            Application.Quit();
        }
    }

}