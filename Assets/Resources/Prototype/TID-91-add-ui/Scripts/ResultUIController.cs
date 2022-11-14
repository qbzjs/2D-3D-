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
        Sprite[] PlayerIcons;
        
        private void Update()
        {
            if(DataManager.Instance.PreRoleGroup.Equals(RoleData.RoleGroup.Exorcist))
            {
                playerInfors[3].Backgroud.sprite = backgrounds[0];
            }
            if(DataManager.Instance.PreRoleGroup.Equals(RoleData.RoleGroup.Doll))
            {
                playerInfors[3].Backgroud.sprite = backgrounds[1];
            }
        }
    }
}
