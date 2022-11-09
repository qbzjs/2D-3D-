using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;
using Photon;
using Photon.Pun;
using Photon.Realtime;

using KSH_Lib;
using KSH_Lib.Data;
using KSH_Lib.UI;

namespace LSH_Lib
{
	public class MatchingUIController : MonoBehaviourPunCallbacks
	{
        [Header("Matching UI")]
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
        private GameObject cancelButtonObj;

        [Header("CharacterSelectCanvas")]
        [SerializeField]
        Canvas characterSelectCanvas;
        [SerializeField]
        TextMeshProUGUI roleText;

        CharacterSelectCanvasController charaSelectCanvasController;

        private void Update()
        {
            //if (isJoinedRoom)
            {
                GameManager.Instance.CurPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
                ChangePlayerImage();
                ChangePlayerCountText();

                if (PhotonNetwork.IsMasterClient)
                {
                    if (GameManager.Instance.CurPlayerCount == 5)
                    {
                        LoadRoomScene();
                    }

                    //if (skipButtonObj.activeInHierarchy == false)
                    //{
                    //    skipButtonObj.SetActive(true);
                    //    lshSkipButtonObj.SetActive(true);
                    //    kshSkipButtonObj.SetActive(true);
                    //}
                }
            }
        }
        public void EnableCharacterSelectCanvas(string roleName)
        {
            LobbyUI_Manager.Instace.DisableCanvasesAll();
            characterSelectCanvas.enabled = true;
            roleText.text = roleName;

            switch (roleName)
            {
                case "Doll":
                    {
                        DataManager.Instance.PreRoleType = RoleData.RoleType.Doll;
                        charaSelectCanvasController.SendMessage("OnSelectRole");
                    }
                    break;
                case "Exorcist":
                    {
                        DataManager.Instance.PreRoleType = RoleData.RoleType.Exorcist;
                        charaSelectCanvasController.SendMessage("OnSelectRole");
                    }
                    break;
                default:
                    {
                        //GameManager.Instance.Data.ChangeRole( RoleType.Null );
                    }
                    break;
            }
        }
        public void OnMatchingStartButton(string roleType)
        {
            if (roleType == "Exorcist")
            {
                DataManager.Instance.PreRoleType = RoleData.RoleType.Exorcist;
                PhotonNetwork.CreateRoom(LobbyUI_Manager.Instace.roomName, new RoomOptions { MaxPlayers = GameManager.Instance.MaxPlayerCount });
            }
            else if (roleType == "Doll")
            {
                DataManager.Instance.PreRoleType = RoleData.RoleType.Doll;
                PhotonNetwork.JoinRoom(LobbyUI_Manager.Instace.roomName);
            }
            else
            {
                Debug.LogWarning("LobbyUI_Manager: Need to Select role");
                return;
            }
        }
        void ChangePlayerImage()
        {
            for (int i = 0; i < GameManager.Instance.CurPlayerCount; ++i)
            {
                playerLoadImgs[i].sprite = refPlayerOnSprite;
            }
            for (int i = GameManager.Instance.CurPlayerCount; i < GameManager.Instance.MaxPlayerCount; ++i)
            {
                playerLoadImgs[i].sprite = refPlayerOffSprite;
            }
        }
        void ChangePlayerCountText()
        {
            userCntTMP.text = $"{GameManager.Instance.CurPlayerCount} / {GameManager.Instance.MaxPlayerCount}";
        }
        void OnSkipButtonClicked()
        {
            LoadRoomScene();
        }
        void LoadRoomScene()
        {
            cancelButtonObj.SetActive(false);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.Instance.LoadPhotonScene(loadSceneName);
        }
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
    }
}
