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
        [SerializeField]
        PlayerInfor[] playerInfors;
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
                for (int i = 0; i < DataManager.Instance.PlayerDatas.Count; ++i)
                {
                    if (i == playeridx)
                    {
                        mynameText.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                        type = DataManager.Instance.PlayerDatas[i].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i].roleData.Type);
                        myTypeText.text = type;
                        //myState.sprite = DataManager.Instance.PlayerDatas[i].roleData.CurState;
                    }
                    else
                    {
                        string icontype;
                        playerInfors[i].NickName.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                        icontype = DataManager.Instance.PlayerDatas[i].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i].roleData.Type);
                        playerInfors[i].RoleType.text = icontype;
                        playerInfors[i].PlayerIcon.sprite = SetIcon(icontype);
                    }
                }
            }
            if(DataManager.Instance.PreRoleGroup.Equals(RoleData.RoleGroup.Doll))
            {
                playerInfors[3].Backgroud.sprite = backgrounds[1];
                
                for(int i = 0; i<DataManager.Instance.PlayerDatas.Count; ++i)
                {
                    if(i == playeridx)
                    {
                        mynameText.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                        type = DataManager.Instance.PlayerDatas[i].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i].roleData.Type);
                        myTypeText.text = type;
                    }
                    else //if(DataManager.Instance.PlayerDatas[i].roleData.Type)
                    {
                        string icontype;
                        playerInfors[i].NickName.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                        icontype = DataManager.Instance.PlayerDatas[i].roleData.GetTypeStr(DataManager.Instance.PlayerDatas[i].roleData.Type);
                        playerInfors[i].RoleType.text = icontype;
                        playerInfors[i].PlayerIcon.sprite = SetIcon(icontype);
                        //playerInfors[i].StatusIcon.sprite = SetSprite(DataManager.Instance.PlayerDatas[i].roleData.Group.)
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
        //Sprite SetStatusIcon(string target)
        //{
        //    switch (target)
        //    {
        //        case "Wolf":
        //            return playerIcons[0];
        //        case "Rabbit":
        //            return playerIcons[1];
        //        case "Bishop":
        //            return playerIcons[2];
        //        case "Hunter":
        //            return playerIcons[3];
        //        default:
        //            return null;
        //    }
        //}
    }
}
