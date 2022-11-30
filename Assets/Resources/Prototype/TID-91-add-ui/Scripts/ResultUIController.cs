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

        int playeridx;
        string type;
        private void Start()
        {
            playeridx = DataManager.Instance.PlayerIdx;
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
                    if (i == playeridx)
                    {
                        mynameText.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                        type = DataManager.Instance.PlayerDatas[i].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i].roleData.Type);
                        myTypeText.text = type;
                        myState.sprite = SetIcon(type);
                    }
                    else if( i == 0)
                    {
                        string icontype;
                        playerInfors[3].NickName.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                        icontype = DataManager.Instance.PlayerDatas[i].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i].roleData.Type);
                        playerInfors[3].RoleType.text = icontype;
                        playerInfors[3].PlayerIcon.sprite = SetIcon(icontype);
                    }
                    else
                    {
                        for(int j = 0; j<DataManager.Instance.PlayerDatas.Count-2; ++j)
                        {
                            string icontype;
                            playerInfors[j].NickName.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                            icontype = DataManager.Instance.PlayerDatas[i].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i].roleData.Type);
                            playerInfors[j].RoleType.text = icontype;
                            playerInfors[j].PlayerIcon.sprite = SetIcon(icontype);
                            int statustype = (int)DataManager.Instance.PlayerDatas[i].behaviorType;
                            playerInfors[j].StatusIcon.sprite = SetStatusIcon(statustype);
                        }
                    }
                }
                ////inforResults[playerInfors.Length - 1].playerIdx = 0;
                //for(int i = 0; i<DataManager.Instance.PlayerDatas.Count; ++i)
                //{
                //    if(i == playeridx)
                //    {
                //        mynameText.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                //        type = DataManager.Instance.PlayerDatas[i].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i].roleData.Type);
                //        myTypeText.text = type;
                //    }
                //    else 
                //    {
                //        string icontype;
                //        if(DataManager.Instance.PlayerDatas[i].roleData.Group == RoleData.RoleGroup.Exorcist)
                //        {
                            
                //            playerInfors[3].NickName.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                //            icontype = DataManager.Instance.PlayerDatas[i].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i].roleData.Type);
                //            playerInfors[3].RoleType.text = icontype;
                //            playerInfors[3].PlayerIcon.sprite = SetIcon(icontype);
                //        }
                //        else
                //        {
                //            for (int j = 0; j < playerInfors.Length - 1; j++)
                //            {
                //                playerInfors[j].NickName.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                //                icontype = DataManager.Instance.PlayerDatas[i].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i].roleData.Type);
                //                playerInfors[j].RoleType.text = icontype;
                //                playerInfors[j].PlayerIcon.sprite = SetIcon(icontype);
                //            }
                //            //playerInfors[i].StatusIcon.sprite = SetSprite(DataManager.Instance.PlayerDatas[i].roleData.Group.)
                //        }
                //    }
                //}    
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
    }
}
