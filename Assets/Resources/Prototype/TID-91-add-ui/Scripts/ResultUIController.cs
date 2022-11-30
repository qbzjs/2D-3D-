using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using KSH_Lib;
using KSH_Lib.UI;
using KSH_Lib.Data;
using GHJ_Lib;
namespace LSH_Lib
{
    public class ResultUIController : MonoBehaviour
    {
        [System.Serializable]
        public struct PlayerInfor
        {
            public Image Backgroud;
            public Image PlayerIcon;
            public TextMeshProUGUI NickName;
            public TextMeshProUGUI RoleType;
            public Image StatusIcon;
        }
        [System.Serializable]
        public struct InforResult
        {
            public int playerIdx;
            public PlayerInfor ui;
        }

        [SerializeField]
        PlayerInfor[] playerInfors;
        [SerializeField]
        InforResult[] inforResults;
        [SerializeField]
        Sprite[] backgrounds;
        [SerializeField]
        Sprite[] statusIcons;
        [SerializeField]
        Sprite[] playerIcons;
        [SerializeField]
        GameObject[] playermodels;

        [Header("player's ui(mine)")]
        [SerializeField]
        TextMeshProUGUI mynameText;
        [SerializeField]
        TextMeshProUGUI myTypeText;
        [SerializeField]
        Image myImage;
        [SerializeField]
        Image myState;
        [SerializeField]
        GameObject[] uiModels;

        [SerializeField] List<PlayerData> copiedPlayerDatas = new List<PlayerData>();

        int playeridx;
        string type;
        private void Start()
        {
            playeridx = DataManager.Instance.PlayerIdx;
            CopyRoleDatas();
        }
        private void Update()
        {
            if(DataManager.Instance.PreRoleGroup.Equals(RoleData.RoleGroup.Exorcist))
            {
                playerInfors[3].Backgroud.sprite = backgrounds[0];
                mynameText.text = DataManager.Instance.PlayerDatas[0].accountData.Nickname;
                type = DataManager.Instance.PlayerDatas[0].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[0].roleData.Type);
                myTypeText.text = type;
                for (int i = 1; i < DataManager.Instance.PlayerDatas.Count; ++i)
                {
                    if(DataManager.Instance.PlayerDatas[i].roleData == null)
                    {
                        break;
                    }
                    string icontype;
                    playerInfors[i-1].NickName.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                    icontype = DataManager.Instance.PlayerDatas[i].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i].roleData.Type);
                    playerInfors[i-1].RoleType.text = icontype;
                    playerInfors[i-1].PlayerIcon.sprite = SetIcon(icontype);
                    int statustype = (int)DataManager.Instance.PlayerDatas[i].behaviorType;
                    playerInfors[i-1].StatusIcon.sprite = SetStatusIcon(statustype);
                }
            }
            if(DataManager.Instance.PreRoleGroup.Equals(RoleData.RoleGroup.Doll))
            {
                playerInfors[3].Backgroud.sprite = backgrounds[1];
                for (int i = 0; i < DataManager.Instance.PlayerDatas.Count; ++i)
                {
                    if (DataManager.Instance.PlayerDatas[i].roleData == null)
                    {
                        break;
                    }

                    else if (i == playeridx)
                    {
                        if ( DataManager.Instance.PlayerDatas[i].roleData == null )
                        {
                            break;
                        }
                        mynameText.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                        type = DataManager.Instance.PlayerDatas[i].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i].roleData.Type);
                        myTypeText.text = type;
                        int statustype = (int)DataManager.Instance.PlayerDatas[i].behaviorType;
                        myState.sprite = SetStatusIcon(statustype);
                    }
                    else if( i == 0)
                    {
                        if ( DataManager.Instance.PlayerDatas[i].roleData == null )
                        {
                            break;
                        }
                        string icontype;
                        playerInfors[3].NickName.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                        icontype = DataManager.Instance.PlayerDatas[i].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i].roleData.Type);
                        playerInfors[3].RoleType.text = icontype;
                        playerInfors[3].PlayerIcon.sprite = SetIcon(icontype);
                    }
                    else
                    {
                        for(int uiOrder = 0; uiOrder< playerInfors.Length - 1; ++uiOrder)
                        {
                            if (DataManager.Instance.PlayerDatas[i + 1].roleData == null)
                            {
                                break;
                            }
                            string icontype;
                            playerInfors[uiOrder].NickName.text = DataManager.Instance.PlayerDatas[i+1].accountData.Nickname;
                            icontype = DataManager.Instance.PlayerDatas[i + 1].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i + 1].roleData.Type);
                            playerInfors[uiOrder].RoleType.text = icontype;
                            playerInfors[uiOrder].PlayerIcon.sprite = SetIcon(icontype);
                            int statustype = (int)DataManager.Instance.PlayerDatas[i + 1].behaviorType;
                            playerInfors[uiOrder].StatusIcon.sprite = SetStatusIcon(statustype);
                        }
                    }
                }
            }
        }
        Sprite SetIcon(string target)
        {
            switch(target)
            {
                case "Wolf":
                    return playerIcons[0];
                case "Rabbit":
                    return playerIcons[1];
                case "Bishop":
                    return playerIcons[2];
                case "Hunter":
                    return playerIcons[3];
                default:
                    return null;
            }

        }
        Sprite SetStatusIcon(int state)
        {
            switch (state)
            {
                case 1:
                    return statusIcons[0];
                case 13:
                    return statusIcons[1];
                case 10:
                    return statusIcons[2];
                default:
                    return statusIcons[2];
            }
        }
        void CopyRoleDatas()
        {
            for(int i = 0; i < DataManager.Instance.PlayerDatas.Count; ++i)
            {
                if(DataManager.Instance.PlayerDatas[i].roleData == null)
                {
                    break;
                }
                PlayerData data = new PlayerData();
                data.roleData = DataManager.Instance.PlayerDatas[i].roleData.Clone();
                data.accountData = DataManager.Instance.PlayerDatas[i].accountData;
                data.behaviorType = DataManager.Instance.PlayerDatas[i].behaviorType;
                copiedPlayerDatas.Add(data);
            }
        }
    }
}
