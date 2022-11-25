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

namespace LSH_Lib
{
    public class CustomRoomCanvasController : MonoBehaviourPunCallbacks
    {
        [System.Serializable]
        public struct PlayerUI
        {
            public Image State;
            public Image UnReadyImage;
            public TextMeshProUGUI NickName;
            public Image RoleType;
        }

        [Header("Players UI")]
        [SerializeField]
        PlayerUI[] playeruis;

        [Header("Sprites")]
        [SerializeField]
        Sprite[] exorcistSprites;
        [SerializeField]
        Sprite[] dollSprites;
        [SerializeField]
        Sprite[] stateSprites;


        [Header("PopUP Panel")]
        [SerializeField]
        GameObject InvitePanel;
        [SerializeField]
        GameObject SettingPanel;
        [Header("Invite UI")]
        [SerializeField]
        TextMeshProUGUI invitecode;

        void DisalbeAllPanel()
        {
            InvitePanel.SetActive(false);
            SettingPanel.SetActive(false);
        }
        void EnableInvitePanel()
        {
            DisalbeAllPanel();
            InvitePanel.SetActive(true);
        }
        void EnableSettingPanel()
        {
            DisalbeAllPanel();
            SettingPanel.SetActive(true);
        }
        void DisableInvitePanel()
        {
            InvitePanel.SetActive(false);
        }
        void DisableSettingPanel()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                DisalbeAllPanel();
            }
        }
        void CopyButton()
        {
            GUIUtility.systemCopyBuffer = invitecode.text;
        }

        private void Start()
        {
            //if(PhotonNetwork.IsMasterClient)
            //    {
            //        RoomManager.Instance.PlayerDatas[0].Init(
            //            DataManager.Instance.PlayerDatas[0].accountData.Id,
            //            DataManager.Instance.PlayerDatas[0].accountData.Nickname,
            //            false, null);
            //    }
            //else
            //{
            //    for (int i = 1; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
            //    {
            //        RoomManager.Instance.PlayerDatas[i].Init(
            //            DataManager.Instance.PlayerDatas[i].accountData.Id,
            //            DataManager.Instance.PlayerDatas[i].accountData.Nickname,
            //            false, null);
            //    }
            //}
            PhotonNetwork.AutomaticallySyncScene = true;
            SetRoll();
            //InitializeImage();
        }
        private void Update()
        {
            ChangeImage();
        }
        //void CountPlayer()
        //{
        //    for(int i= 0; i<PhotonNetwork.CurrentRoom.PlayerCount;++i)
        //    {
        //        if(i == 0)
        //        {
        //            DataManager.Instance.PlayerDatas[i].RollType = "Exorcist";
        //            InitializeImage(i, RoomManager.Instance.PlayerDatas[i].RollType);
        //        }
        //        else
        //        {
        //            DataManager.Instance.PlayerDatas[i].RollType = "Doll";
        //            InitializeImage(i, RoomManager.Instance.PlayerDatas[i].RollType);
        //        }

        //    }
        //}
        void ChangeImage()
        {
            if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Exorcist) 
            {
                playeruis[DataManager.Instance.PlayerIdx].RoleType.sprite = exorcistSprites[0];
                //playeruis[DataManager.Instance.PlayerIdx].NickName.text = DataManager.Instance.LocalPlayerData.accountData.Nickname;
                //DataManager.Instance.LocalPlayerData.accountData.SheetIdx = DataManager.Instance.PlayerIdx;
                for(int i = 1; i<PhotonNetwork.CurrentRoom.PlayerCount; ++i)
                {
                    playeruis[i].RoleType.sprite = dollSprites[0];
                }
            }
            if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Doll)
            {
                playeruis[4].RoleType.sprite = exorcistSprites[0];
                playeruis[0].RoleType.sprite = dollSprites[0];
                //DataManager.Instance.LocalPlayerData.accountData.SheetIdx = DataManager.Instance.PlayerIdx + 1;
                for (int i = 1; i<PhotonNetwork.CurrentRoom.PlayerCount-1; ++i)
                {
                    playeruis[i].RoleType.sprite = dollSprites[0];
                }
            }
        }

        //[System.Serializable]
        //public struct CustomMatch
        //{
        //    public int playerIdx;
        //    public PlayerUI ui;
        //}
        //public int PlayerIdx { get { return DataManager.Instance.PlayerIdx; } }
        //public int exorcistIdx
        //{
        //    get
        //    {
        //        if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Exorcist)
        //        {
        //            return 0;
        //        }
        //        else if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Doll)
        //        {
        //            return 4;
        //        }
        //        //else
        //        //{
        //        //    Debug.LogError("No PreRoleType set");
        //        //    return -1;
        //        //}
        //    }
        //}
        [SerializeField]
        string loadSceneName;
        //[Header("PopUP Panel")]
        //[SerializeField]
        //GameObject InvitePanel;
        //[SerializeField]
        //GameObject SettingPanel;
        //[Header("Invite UI")]
        //[SerializeField]
        //TextMeshProUGUI invitecode;
        //[SerializeField]
        //Image[] player;
        //[SerializeField]
        //TextMeshProUGUI RoleSelectButton;
        //[SerializeField]
        //Sprite exorcistSprite;
        //[SerializeField]
        //Sprite exorcistOffSprite;
        //[SerializeField]
        //Sprite dollSprite;
        //[SerializeField]
        //Sprite dollOffSprite;
        //[SerializeField]
        //Sprite MasterImage;
        //[SerializeField]
        //Sprite ReadyImage;

        //[SerializeField]
        //CustomMatch[] customMatches;

        //private void Start()
        //{
        //    int playeridx = DataManager.Instance.PlayerIdx;
        //}
        //private void Update()
        //{
        //    CountPlayer();
        //}
        //void CountPlayer()
        //{
        //for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        //{
        //if(i == customMatches[i].playerIdx)
        //{

        //}
        //if(PhotonNetwork.IsMasterClient)
        //{
        //    customMatches[i].ui.State.sprite = MasterImage;
        //    customMatches[i].ui.RoleType.sprite = exorcistSprite;
        //}
        //else
        //{
        //    customMatches[i].ui.UnReadyImage.enabled = true;
        //    customMatches[i].ui.RoleType.sprite = dollSprite;
        //}
        //customMatches[i].ui.NickName.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
        //}
        //}
        //    private void Start()
        //    {
        //        ResetImage();
        //        SetRoll();
        //        InvitePanel.SetActive(false);
        //        SettingPanel.SetActive(false);
        //        invitecode.text = PhotonNetwork.CurrentRoom.Name;
        //    }
        //    private void Update()
        //    {
        //        ChangeImage();
        //        IndexingInfo();
        //        DisableSettingPanel();
        //    }
        //    void DisalbeAllPanel()
        //    {
        //        InvitePanel.SetActive(false);
        //        SettingPanel.SetActive(false);
        //    }
        //    void EnableInvitePanel()
        //    {
        //        DisalbeAllPanel();
        //        InvitePanel.SetActive(true);
        //    }
        //    void EnableSettingPanel()
        //    {
        //        DisalbeAllPanel();
        //        SettingPanel.SetActive(true);
        //    }
        //    void DisableInvitePanel()
        //    {
        //        InvitePanel.SetActive(false);
        //    }
        //    void DisableSettingPanel()
        //    {
        //        if (Input.GetKeyDown(KeyCode.Escape))
        //        {
        //            DisalbeAllPanel();
        //        }
        //    }
        //    void CopyButton()
        //    {
        //        GUIUtility.systemCopyBuffer = invitecode.text;
        //    }
        void SetRoll()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Exorcist;
            }
            else
            {
                DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Doll;
            }
            //DataManager.Instance.ShareRoleData();
        }
        //    //public CustomMatch GetSelectRoleType( CustomMatch customMatch, RoleData.RoleGroup type)
        //    //{
        //    //    Color color = new Color(32, 32, 32);
        //    //    switch(type)
        //    //    {
        //    //        case RoleData.RoleGroup.Exorcist:
        //    //            {
        //    //                if(PhotonNetwork.IsMasterClient)
        //    //                {
        //    //                    customMatch.ui.Change(MasterImage, "test", exorcistSprite);
        //    //                }
        //    //                customMatch.ui.Change(ReadyImage, "test", exorcistSprite);
        //    //            }
        //    //            break;
        //    //        case RoleData.RoleGroup.Doll:
        //    //            {
        //    //                if (PhotonNetwork.IsMasterClient)
        //    //                {
        //    //                    customMatch.ui.Change(MasterImage, "test", dollSprite);
        //    //                }
        //    //                customMatch.ui.Change(ReadyImage, "text", dollSprite);
        //    //            }
        //    //            break;
        //    //        default:
        //    //            {
        //    //                Debug.LogError("GetSelectInfoByRoleType: No Selected ");
        //    //                return new CustomMatch();
        //    //            }
        //    //    }
        //    //    customMatch.ui.ChangeColor(customMatch.ui.NickName, color);
        //    //    return customMatch;
        //    //}
        //    void ChangeRoll()
        //    {
        //        if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Exorcist)
        //        {
        //            DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Doll;
        //            RoleSelectButton.text = "퇴마사로 변경하기";
        //        }
        //        else
        //        {
        //            DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Exorcist;
        //            RoleSelectButton.text = "인형으로 변경하기";
        //        }
        //    }
        //    void ChangeImage()
        //    {
        //        if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Exorcist)
        //        {
        //            RoleSelectButton.text = "인형으로 변경하기";
        //            player[0].sprite = exorcistSprite;
        //            player[4].sprite = dollSprite;
        //            for (int i = 1; i <PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        //            {
        //                player[i].sprite = dollSprite;
        //            }
        //        }
        //        else
        //        {
        //            RoleSelectButton.text = "퇴마사로 변경하기";
        //            player[4].sprite = exorcistOffSprite;
        //            player[0].sprite = dollSprite;
        //            for (int i = 1; i < PhotonNetwork.CurrentRoom.PlayerCount - 1; ++i)
        //            {
        //                player[i].sprite = dollSprite;
        //            }
        //        }
        //    }
        //    void ResetImage()
        //    {
        //        for(int i = 0; i<PhotonNetwork.CurrentRoom.PlayerCount; ++i)
        //        {
        //            player[i].sprite = dollOffSprite;
        //        }
        //    }
        //    void IndexingInfo()
        //    {
        //        if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Doll)
        //        {
        //            customMatches[customMatches.Length - 1].playerIdx = 0;

        //            for (int i = 0; i < customMatches.Length - 1; ++i)
        //            {
        //                if (customMatches[i].playerIdx == 0)
        //                {
        //                    customMatches[i].playerIdx = -1;
        //                }
        //            }
        //            for (int i = 0; i < customMatches.Length - 1; ++i)
        //            {
        //                if (i == PlayerIdx)
        //                {
        //                    customMatches[0].playerIdx = PlayerIdx;
        //                    break;
        //                }
        //            }

        //            for (int i = 1, pIdx = 1; i < customMatches.Length - 1; ++i, ++pIdx)
        //            {
        //                if (customMatches[i].playerIdx == -1)
        //                {
        //                    if (i == PlayerIdx)
        //                    {
        //                        pIdx++;
        //                    }
        //                    customMatches[i].playerIdx = pIdx;
        //                    break;
        //                }
        //            }
        //        }
        //        else if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Exorcist)
        //        {
        //            for (int i = 0; i < customMatches.Length; ++i)
        //            {
        //                customMatches[i].playerIdx = i;
        //            }
        //        }
        //        else
        //        {
        //            Debug.LogError("No RoleType set");
        //        }
        //    }
        void GameStart()
        {
            //DataManager.Instance.InitLocalRoleData();
            //cancelButtonObj.SetActive(false);
            //PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.AutomaticallySyncScene = true;
                PhotonNetwork.CurrentRoom.IsOpen = false;
                GameManager.Instance.LoadPhotonScene(loadSceneName);
            //else
            //{
            //    PhotonNetwork.AutomaticallySyncScene = true;
            //    //PhotonNetwork.CurrentRoom.IsOpen = false;
            //}
            //GameManager.Instance.LoadPhotonScene(loadSceneName);
        }
        void ExitScene()
        {
            //if (PhotonNetwork.IsMasterClient)
            //{
            //    //photonView.RPC( "LeaveRoomRPC", RpcTarget.Others );

            //    for (int i = 1; i < PhotonNetwork.PlayerList.Length; ++i)
            //    {
            //        PhotonNetwork.CloseConnection(PhotonNetwork.PlayerList[i]);
            //    }
            //}
            DataManager.Instance.ResetPlayerDatas();
            PhotonNetwork.LeaveRoom();
            GameManager.Instance.LoadScene("02_MainLobbyScene");
        }
        //} 
    }
}

