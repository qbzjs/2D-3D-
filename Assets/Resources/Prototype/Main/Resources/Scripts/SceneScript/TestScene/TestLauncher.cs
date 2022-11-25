using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using KSH_Lib.Data;

namespace KSH_Lib.Test
{
    public class TestLauncher : MonoBehaviourPunCallbacks
    {
        [SerializeField] string nextScene = "TestGameScene";
        [SerializeField] string roomName = "TestTestTest";
        [SerializeField] GameObject joinRoomButton;
        [SerializeField] GameObject gameStartButton;
        [SerializeField] GameObject characterButtons;
        private void Start()
        {
            joinRoomButton.SetActive( true );
            characterButtons.SetActive( false );
            gameStartButton.SetActive( false );
        }
        public override void OnJoinedRoom()
        {
            joinRoomButton.SetActive( false );
            characterButtons.SetActive( true );
            gameStartButton.SetActive( true );
            DataManager.Instance.InitPlayerDatas();
        }

        public void OnClickJoinRoom()
        {
            Debug.Log( "Joined" );
            PhotonNetwork.JoinOrCreateRoom( roomName, new RoomOptions { MaxPlayers = GameManager.Instance.MaxPlayerCount }, TypedLobby.Default );
            DataManager.Instance.SetLocalAccount( -1, "Test", "Test" );
        }

        public void OnSelectCharacter( string name )
        {
            DataManager.Instance.PreRoleName = (RoleData.RoleType)System.Enum.Parse( typeof( RoleData.RoleType ), name );
            if( DataManager.Instance.PreRoleName > RoleData.RoleType.Hunter)
            {
                DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Doll;
            }
            else
            {
                DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Exorcist;
            }
            DataManager.Instance.InitLocalRoleData();
            DataManager.Instance.ShareRoleData();

            Debug.Log( $"Selected {DataManager.Instance.PreRoleName}" );
        }
        public void OnStartGame()
        {
            GameManager.Instance.LoadPhotonScene( nextScene );
        }
    }
}
