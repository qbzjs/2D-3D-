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

using DEM;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace KSH_Lib
{
    public class LobbyUI_Manager : MonoBehaviourPunCallbacks, IPunObservable
    {
        //public static LobbyUI_Manager Instace { get { return instance; } }

        //private static LobbyUI_Manager instance;

        /*--- Serialized Fields ---*/
        [Header( "Canvases" )]
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
        [SerializeField]
        Canvas informationCanvas;


        [Header( "MainLobby Buttons UI" )]
        [SerializeField]
        GameObject mainLobbyButtons;
        //[SerializeField]
        //GameObject playButtons;
        [SerializeField]
        GameObject quickMatchButtons;

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
        GameObject monkeyInformation;
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
        TextMeshProUGUI roleText;

        //[Header("Matching UI")]
        //[SerializeField]
        //private string loadSceneName = "02_MainGameScene";
        //[SerializeField]
        //private TextMeshProUGUI userCntTMP;
        //[SerializeField]
        //private Sprite refPlayerOnSprite;
        //[SerializeField]
        //private Sprite refPlayerOffSprite;
        //[SerializeField]
        //private Image[] playerLoadImgs;
        //[SerializeField]
        //private GameObject cancelButtonObj;


        [Header( "CustomRoom UI" )]
        [SerializeField]
        TextMeshProUGUI customRoomTypeTMP;
        [SerializeField]
        TextMeshProUGUI actionButtonTMP;


        //[Header("Debug Only")]
        //[SerializeField]
        //private GameObject skipButtonObj;
        //[SerializeField]
        //private GameObject lshSkipButtonObj;
        //[SerializeField]
        //private GameObject kshSkipButtonObj;
        [SerializeField]
        public string roomName = "Debug";

        //[SerializeField]
        //string lshSceneName = "";
        //[SerializeField]
        //string kshSceneName = "";


        /*--- Private Fields ---*/
        public bool IsJoinedRoom { get; private set; }

        CharacterSelectCanvasController charaSelectCanvasController;


        /*--- MonoBehaviour Callbacks ---*/
        private void Start()
        {
            charaSelectCanvasController = characterSelectCanvas.GetComponent<CharacterSelectCanvasController>();

            EnableCanvasObjects();
            DisableCanvasesAll();
            mainLobbyCanvas.enabled = true;
            EnableMainButtonsPanel();

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
            Debug.Log( "OnJoinRandomFailed Called " + message );
            // Need To Implement fail to Join popup message
        }
        public override void OnCreatedRoom()
        {
            //add new script
            PhotonNetwork.CurrentRoom.SetCustomProperties( new Hashtable { { "roomNumber", GameManager.Instance.GetRoomNumber() } } );
            //
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
            DataManager.Instance.SetPlayerIdx();
            IsJoinedRoom = true;
        }
        public override void OnJoinRoomFailed( short returnCode, string message )
        {
            base.OnJoinRoomFailed( returnCode, message );
            Debug.Log( "OnJoinRoomFailed Called " + message );
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
            Debug.Log( "OnLeftRoom Called" );
            IsJoinedRoom = false;
        }

        /*--- Public Methods ---*/
        public void EnableCharacterSelectCanvas( string roleName )
        {
            DisableCanvasesAll();
            characterSelectCanvas.enabled = true;
            roleText.text = roleName;

            switch ( roleName )
            {
                case "Doll":
                {
                    DataManager.Instance.PreRoleType = RoleData.RoleType.Doll;
                    charaSelectCanvasController.SendMessage( "OnSelectRole" );
                }
                break;
                case "Exorcist":
                {
                    DataManager.Instance.PreRoleType = RoleData.RoleType.Exorcist;
                    charaSelectCanvasController.SendMessage( "OnSelectRole" );
                }
                break;
                default:
                {
                    //GameManager.Instance.Data.ChangeRole( RoleType.Null );
                }
                break;
            }
        }

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
            characterSelectCanvas.gameObject.SetActive( true );
            matchingCanvas.gameObject.SetActive( true );
            customRoomCanvas.gameObject.SetActive( true );
            customRoomLobbyCanvas.gameObject.SetActive( true );
            informationCanvas.gameObject.SetActive(true);
            
        }
        public void DisableCanvasesAll()
        {
            mainLobbyCanvas.enabled = false;
            characterSelectCanvas.enabled = false;
            matchingCanvas.enabled = false;
            customRoomCanvas.enabled = false;
            customRoomLobbyCanvas.enabled = false;
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
            monkeyInformation.SetActive(false);
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
        void EnableCustomRoomLobbyCanvas()
        {
            DisableCanvasesAll();
            customRoomLobbyCanvas.enabled = true;
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
        void EnableMonkeyInformation()
        {
            DisableMainLobbyPanelAll();
            monkeyInformation.SetActive(true);
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
            if(roleType == "Exorcist")
            {
                DataManager.Instance.PreRoleType = RoleData.RoleType.Exorcist;
                PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = GameManager.Instance.MaxPlayerCount });
            }
            else if (roleType == "Doll")
            {
                DataManager.Instance.PreRoleType = RoleData.RoleType.Doll;
                PhotonNetwork.JoinRoom(roomName);
            }
            else 
            {
                Debug.LogWarning("LobbyUI_Manager: Need to Select role");
                return;
            }
        }
        void OnMatchingCancelButton()
        {
            if(PhotonNetwork.IsMasterClient)
            {
                //photonView.RPC( "LeaveRoomRPC", RpcTarget.Others );
                
                for(int i = 1; i < PhotonNetwork.PlayerList.Length; ++i )
                {
                    PhotonNetwork.CloseConnection( PhotonNetwork.PlayerList[i] );
                }
            }

            //PhotonNetwork.LeaveRoom();
            
            //IsJoinedRoom = false;
            //matchingUIController.isJoinedRoom = false;
            EnableMainLobbyCanvas();
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

        [PunRPC]
        void LeaveRoomRPC()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info )
        {
        }
    }

}