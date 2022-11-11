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
    public class MatchingUIController : MonoBehaviour//PunCallbacks
    {
        [Header("Matching UI")]
        [SerializeField]
        private string loadSceneName = "02_MainGameScene";
        [SerializeField]
        private TextMeshProUGUI userCntTMP;
        [SerializeField]
        private Sprite refPlayerOnSprite;
        [SerializeField]
        private Sprite exorcistOnSprite;
        [SerializeField]
        private Sprite refPlayerOffSprite;
        [SerializeField]
        private Sprite exorcistOffSprite;
        [SerializeField]
        private Image[] playerLoadImgs;
        [SerializeField]
        private GameObject cancelButtonObj;

        [SerializeField] LobbyUI_Manager uiManager;

        [Header("CharacterSelectCanvas")]
        [SerializeField]
        Canvas characterSelectCanvas;
        [SerializeField]
        TextMeshProUGUI roleText;


        [Header("Debug Only")]
        [SerializeField]
        private GameObject skipButtonObj;
        [SerializeField]
        private GameObject lshSkipButtonObj;
        [SerializeField]
        private GameObject kshSkipButtonObj;
        [SerializeField]
        public string roomName = "Debug";

        [SerializeField]
        string lshSceneName = "";
        [SerializeField]
        string kshSceneName = "";

        //bool isJoinedRoom = false;
        CharacterSelectCanvasController charaSelectCanvasController;

        private void Start()
        {
            // Deubg
            //isJoinedRoom = true;
            skipButtonObj.SetActive(false);
            lshSkipButtonObj.SetActive( false );
            kshSkipButtonObj.SetActive( false );
        }

        private void Update()
        {
            if ( uiManager.IsJoinedRoom )
            {
                InitializedPlayerImages();
                GameManager.Instance.CurPlayerCount = PhotonNetwork.CurrentRoom.PlayerCount;
                ChangePlayerImage();
                ChangePlayerCountText();

                if (PhotonNetwork.IsMasterClient)
                {
                    if (GameManager.Instance.CurPlayerCount == 5)
                    {
                        LoadRoomScene();
                    }

                    if (skipButtonObj.activeInHierarchy == false)
                    {
                        skipButtonObj.SetActive(true);
                        lshSkipButtonObj.SetActive(true);
                        kshSkipButtonObj.SetActive(true);
                    }
                }
            }
        }

        public void EnableCharacterSelectCanvas()
        {
            //LobbyUI_Manager.Instace.DisableCanvasesAll();
            characterSelectCanvas.enabled = true;
            if(DataManager.Instance.PreRoleType.Equals("Exorcist"))
            {
                charaSelectCanvasController.SendMessage("OnSelectRole");
            }
            else if(DataManager.Instance.PreRoleType.Equals("Doll"))
            {
                charaSelectCanvasController.SendMessage("OnSelectRole");
            }
        }
        public void EnableCharacterSelectCanvas(string roleName)
        {
            //LobbyUI_Manager.Instace.DisableCanvasesAll();
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
        //public void OnMatchingStartButton(string roleType)
        //{
        //    if (roleType == "Exorcist")
        //    {
        //        DataManager.Instance.PreRoleType = RoleData.RoleType.Exorcist;
        //        PhotonNetwork.CreateRoom(LobbyUI_Manager.Instace.roomName, new RoomOptions { MaxPlayers = GameManager.Instance.MaxPlayerCount });
        //    }
        //    else if (roleType == "Doll")
        //    {
        //        DataManager.Instance.PreRoleType = RoleData.RoleType.Doll;
        //        PhotonNetwork.JoinRoom(LobbyUI_Manager.Instace.roomName);
        //    }
        //    else
        //    {
        //        Debug.LogWarning("LobbyUI_Manager: Need to Select role");
        //        return;
        //    }
        //}
        void ChangePlayerImage()
        {   

            if(DataManager.Instance.PreRoleType == RoleData.RoleType.Exorcist)
            {
                playerLoadImgs[0].sprite = exorcistOnSprite;
                for ( int i = 1; i < GameManager.Instance.CurPlayerCount; ++i )
                {
                    playerLoadImgs[i].sprite = refPlayerOnSprite;
                }
            }
            else
            {
                playerLoadImgs[4].sprite = exorcistOnSprite;

                for(int i = 0; i < GameManager.Instance.CurPlayerCount - 1; ++i )
                {
                    playerLoadImgs[i].sprite = refPlayerOnSprite;
                }
            }




            //if(PhotonNetwork.IsMasterClient)
            //{
            //    playerLoadImgs[4].sprite = exorcistOnSprite;
            //}
            //else
            //{
            //    for (int i =0; i < GameManager.Instance.CurPlayerCount-1; ++i)
            //    {

            //        playerLoadImgs[i].sprite = refPlayerOnSprite;

            //    }

            //    for (int i = GameManager.Instance.CurPlayerCount-1; i < GameManager.Instance.MaxPlayerCount-1; ++i)
            //    {

            //        playerLoadImgs[i].sprite = refPlayerOffSprite;
            //    }
            //}
            
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
            DataManager.Instance.InitLocalRoleData();
            cancelButtonObj.SetActive(false);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.Instance.LoadPhotonScene(loadSceneName);
        }
        //void OnLSHButtonClicked()
        //{
        //    loadSceneName = lshSceneName;
        //    OnSkipButtonClicked();
        //}
        void OnKSHButtonClicked()
        {
            loadSceneName = kshSceneName;
            OnSkipButtonClicked();
        }
        void InitializedPlayerImages()
        {
            for(var i = 0; i<playerLoadImgs.Length;++i)
            {
                playerLoadImgs[i].sprite = refPlayerOffSprite;
            }
        }

    }
}
