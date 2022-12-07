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


        //bool isJoinedRoom = false;
        CharacterSelectCanvasController charaSelectCanvasController;
        bool isStartLoadGame = false;

        private void Start()
        {
            // Deubg
            //isJoinedRoom = true;
            skipButtonObj.SetActive(false);
        }

        public override void OnEnable()
        {
            base.OnEnable();
        }

        private void Update()
        {
            if (uiManager.IsJoinedRoom)
            {
                InitializedPlayerImages();
                ChangePlayerImage();
                ChangePlayerCountText();

                if (PhotonNetwork.IsMasterClient)
                {
                    if (PhotonNetwork.CurrentRoom.PlayerCount == 5 && !isStartLoadGame )
                    {
                        isStartLoadGame = true;
                        LoadRoomScene();
                        //isFull = true;
                        //if (isFull)
                        //{
                        //    LoadNextScene();
                        //    isFull = false;
                        //    return;
                        //}
                        //return;
                        //LoadNextScene();
                        //StartCoroutine(why());
                    }
                    
                    if (skipButtonObj.activeInHierarchy == false)
                    {
                        skipButtonObj.SetActive(true);
                    }
                }
            }
        }
        //IEnumerator IsJoinRoom()
        //{
        //    if (uiManager.IsJoinedRoom)
        //    {
        //        InitializedPlayerImages();
        //        ChangePlayerImage();
        //        ChangePlayerCountText();

        //        if (PhotonNetwork.IsMasterClient)
        //        {
        //            if (PhotonNetwork.CurrentRoom.PlayerCount == 5)
        //            {
        //                //isFull = true;
        //                //if(isFull)
        //                //{
        //                //    LoadNextScene();
        //                //    isFull = false;
        //                //    return;
        //                //}
        //                //return;
        //                LoadNextScene();
        //            }
        //            LoadNextScene();
        //            if (skipButtonObj.activeInHierarchy == false)
        //            {
        //                skipButtonObj.SetActive(true);
        //            }
        //        }
        //    }
        //    yield return new WaitForSeconds(1.0f);
        //}
        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            skipButtonObj.SetActive( false );
        }

        public void EnableCharacterSelectCanvas()
        {
            //LobbyUI_Manager.Instace.DisableCanvasesAll();
            characterSelectCanvas.enabled = true;
            if(DataManager.Instance.PreRoleGroup.Equals(GameManager.ExorcistTag))
            {
                charaSelectCanvasController.SendMessage("OnSelectRole");
            }
            else if(DataManager.Instance.PreRoleGroup.Equals(GameManager.DollTag))
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
                        DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Doll;
                        charaSelectCanvasController.SendMessage("OnSelectRole");
                    }
                    break;
                case "Exorcist":
                    {
                        DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Exorcist;
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

            if(DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Exorcist)
            {
                playerLoadImgs[0].sprite = exorcistOnSprite;
                for ( int i = 1; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i )
                {
                    playerLoadImgs[i].sprite = refPlayerOnSprite;
                }
            }
            else
            {
                playerLoadImgs[4].sprite = exorcistOnSprite;

                for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount - 1; ++i )
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
            userCntTMP.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {GameManager.Instance.MaxPlayerCount}";
        }
        void OnSkipButtonClicked()
        {
            LoadRoomScene();
        }
        void LoadNextScene()
        {
            ////if(isFull)
            ////{
            ////    LoadRoomScene();
            ////    isFull = false;
            ////}
            //isFull = true;
            //if(isFull)
            //{
            //    LoadRoomScene();
            //    return;
            //}
            //if(PhotonNetwork.CurrentRoom.PlayerCount == 5)
            //{
            //    StartCoroutine(why());
            //}
        }
        //IEnumerator why()
        //{
        //    yield return new WaitForSeconds(5.0f);
        //    LoadRoomScene();
        //}
        void LoadRoomScene()
        {
            //DataManager.Instance.InitLocalRoleData();
            cancelButtonObj.SetActive(false);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            GameManager.Instance.LoadPhotonScene(loadSceneName);
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
