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
    public class CustomRoomCanvasController : MonoBehaviour
    {   
        [System.Serializable]
        public struct PlayerUI
        {
            public Image State;
            public TextMeshProUGUI NickName;
            public Image RoleType;
            public void Change(Sprite statesprite, string nickname, Sprite rolesprite )
            {
                State.sprite = statesprite;
                NickName.text = nickname;
                RoleType.sprite = rolesprite;
            }
            public void ChangeColor(TextMeshProUGUI target, Color color)
            {
                target.color = color;
            }
        }
        [System.Serializable]
        public struct CustomMatch
        {
            public int playerIdx;
            public PlayerUI ui;
        }

        public int PlayerIdx { get { return DataManager.Instance.PlayerIdx; } }
        public int exorcistIdx
        {
            get
            {
                if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Exorcist)
                {
                    return 0;
                }
                else if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Doll)
                {
                    return 4;
                }
                else
                {
                    Debug.LogError("No PreRoleType set");
                    return -1;
                }
            }
        }
        [SerializeField]
        string loadSceneName;
        [SerializeField]
        Image[] player;
        [SerializeField]
        TextMeshProUGUI RoleSelectButton;
        [SerializeField]
        Sprite exorcistSprite;
        [SerializeField]
        Sprite exorcistOffSprite;
        [SerializeField]
        Sprite dollSprite;
        [SerializeField]
        Sprite dollOffSprite;
        [SerializeField]
        Sprite MasterImage;
        [SerializeField]
        Sprite ReadyImage;

        [SerializeField]
        CustomMatch[] customMatches;
        SortedSet<CustomMatch> customSets;
        private void Start()
        {
            SetRoll();
        }
        private void Update()
        {
            ResetImage();
            ChangeImage();
            IndexingInfo();
        }
        void SetRoll()
        {
            if(PhotonNetwork.IsMasterClient)
            {
                DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Exorcist;
            }
            else
            {
                DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Doll;
            }
        }
        public CustomMatch GetSelectRoleType( CustomMatch customMatch, RoleData.RoleGroup type)
        {
            Color color = new Color(32, 32, 32);
            switch(type)
            {
                case RoleData.RoleGroup.Exorcist:
                    {
                        if(PhotonNetwork.IsMasterClient)
                        {
                            customMatch.ui.Change(MasterImage, "test", exorcistSprite);
                        }
                        customMatch.ui.Change(ReadyImage, "test", exorcistSprite);
                    }
                    break;
                case RoleData.RoleGroup.Doll:
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            customMatch.ui.Change(MasterImage, "test", dollSprite);
                        }
                        customMatch.ui.Change(ReadyImage, "text", dollSprite);
                    }
                    break;
                default:
                    {
                        Debug.LogError("GetSelectInfoByRoleType: No Selected ");
                        return new CustomMatch();
                    }
            }
            customMatch.ui.ChangeColor(customMatch.ui.NickName, color);
            return customMatch;
        }
        void ChangeRoll()
        {
            if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Exorcist)
            {
                DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Doll;
                RoleSelectButton.text = "퇴마사로 변경하기";
            }
            else
            {
                DataManager.Instance.PreRoleGroup = RoleData.RoleGroup.Exorcist;
                RoleSelectButton.text = "인형으로 변경하기";
            }
        }
        void ChangeImage()
        {
            //ResetImage();
            if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Exorcist)
            {
                RoleSelectButton.text = "인형으로 변경하기";
                player[0].sprite = exorcistSprite;
                for (int i = 1; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
                {
                    player[i].sprite = dollSprite;
                }
            }
            else
            {
                RoleSelectButton.text = "퇴마사로 변경하기";
                player[4].sprite = exorcistOffSprite;
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount - 1; ++i)
                {
                    player[i].sprite = dollSprite;
                }
            }
        }
        void ResetImage()
        {
            for(int i = 0; i<PhotonNetwork.CurrentRoom.PlayerCount; ++i)
            {
                player[i].sprite = dollOffSprite;
            }
        }
        void IndexingInfo()
        {
            if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Doll)
            {
                customMatches[customMatches.Length - 1].playerIdx = 0;

                for (int i = 0; i < customMatches.Length - 1; ++i)
                {
                    if (customMatches[i].playerIdx == 0)
                    {
                        customMatches[i].playerIdx = -1;
                    }
                }
                for (int i = 0; i < customMatches.Length - 1; ++i)
                {
                    if (i == PlayerIdx)
                    {
                        customMatches[0].playerIdx = PlayerIdx;
                        break;
                    }
                }

                for (int i = 1, pIdx = 1; i < customMatches.Length - 1; ++i, ++pIdx)
                {
                    if (customMatches[i].playerIdx == -1)
                    {
                        if (i == PlayerIdx)
                        {
                            pIdx++;
                        }
                        customMatches[i].playerIdx = pIdx;
                        break;
                    }
                }
            }
            else if (DataManager.Instance.PreRoleGroup == RoleData.RoleGroup.Exorcist)
            {
                for (int i = 0; i < customMatches.Length; ++i)
                {
                    customMatches[i].playerIdx = i;
                }
            }
            else
            {
                Debug.LogError("No RoleType set");
            }
        }
        void GameStart()
        {
            //DataManager.Instance.InitLocalRoleData();
            //cancelButtonObj.SetActive(false);
            if(PhotonNetwork.IsMasterClient && DataManager.Instance.PreRoleGroup.Equals(RoleData.RoleGroup.Exorcist))
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                GameManager.Instance.LoadPhotonScene(loadSceneName);
            }
        }
    } 
}

