using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KSH_Lib;
using KSH_Lib.Data;
using TMPro;
using Photon.Pun;

namespace GHJ_Lib
{ 
    public class DollUI : InGameUI
    {
        
        [Header("UI Objects")]
        public GameObject PlayerUiObject;
        public GameObject[] FriendUiObjects;

        [Header("Sprites")]
        [SerializeField]
        Sprite[] iconSprites;

        [Header("PlayerHP")]
        [SerializeField]
        Image PlayerImage;
        [SerializeField]
        TextMeshProUGUI playerName;
        public Slider PlayerDollHP;
        public Slider PlayerDevilHP;

        [Header("Friend1 HP")]
        [SerializeField] Image Friend1Sprite;
        [SerializeField] TextMeshProUGUI Friend1Name;
        public Slider Friend1DollHP;
        public Slider Friend1DevilHP;

        [Header("Friend2 HP")]
        [SerializeField] Image Friend2Sprite;
        [SerializeField] TextMeshProUGUI Friend2Name;
        public Slider Friend2DollHP;
        public Slider Friend2DevilHP;

        [Header("Friend3 HP")]
        [SerializeField] Image Friend3Sprite;
        [SerializeField] TextMeshProUGUI Friend3Name;
        public Slider Friend3DollHP;
        public Slider Friend3DevilHP;

        [Header("Skill ICon")]
        public InputActionUI CommomSkill;
        public InputActionUI CharacterSkill;

        private List<Slider> friendDollHP = new List<Slider>();
        private List<Slider> friendDevilHP = new List<Slider>();

        private List<float> maxDollHP = new List<float>();
        private List<float> maxDevilHP = new List<float>();
        private List<TextMeshProUGUI> friendNickName = new List<TextMeshProUGUI>();
        private List<Image> friendSprite = new List<Image>();

        private int myIdx;

        int curDollCount;
        int curFriendCount;

        bool isInited;

        void Start()
        {
            curDollCount = PhotonNetwork.CurrentRoom.PlayerCount - 1;
            curFriendCount = curDollCount - 1;

            DisableUI_All();
            EnableUI();

            friendDollHP.Add(Friend1DollHP);
            friendDollHP.Add(Friend2DollHP);
            friendDollHP.Add(Friend3DollHP);

            friendDevilHP.Add(Friend1DevilHP);
            friendDevilHP.Add(Friend2DevilHP);
            friendDevilHP.Add(Friend3DevilHP);

            friendNickName.Add(Friend1Name);
            friendNickName.Add(Friend2Name);
            friendNickName.Add(Friend3Name);

            friendSprite.Add(Friend1Sprite);
            friendSprite.Add(Friend2Sprite);
            friendSprite.Add(Friend3Sprite);
            for (int i = 1; i < PhotonNetwork.CurrentRoom.PlayerCount - 1; ++i)
            {
                if (i == myIdx)
                {
                    PlayerImage.sprite = SetSprite(DataManager.Instance.PlayerDatas[i].roleData.Type);
                    playerName.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                }
                else
                {
                    friendSprite[i].sprite = SetSprite(DataManager.Instance.PlayerDatas[i].roleData.Type);
                    friendNickName[i].text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                }
            }
            StartCoroutine(InitUI());
            
        }
        Sprite SetSprite(RoleData.RoleType type)
        {
            switch (type)
            {
                case RoleData.RoleType.Rabbit:
                {
                        return iconSprites[0];
                }
                case RoleData.RoleType.Wolf:
                    {
                        return iconSprites[1];
                    }
                default:
                    return iconSprites[2];
                      
            }
        }
        private void Update()
        {
            if(!isInited || PlayerUiObject == null || FriendUiObjects == null)
            {
                return;
            }

            if ( !PhotonNetwork.InRoom )
            {
                return;
            }

            int j = 0;
            for (int i = 1; i < curDollCount + 1; ++i)
            {
                if (myIdx == i)
                {
                    PlayerImage.sprite = SetSprite(DataManager.Instance.PlayerDatas[i].roleData.Type);
                    playerName.text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                    PlayerDollHP.value = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DollHP / maxDollHP[i - 1];
                    PlayerDevilHP.value = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DevilHP / maxDevilHP[i - 1];
                    PlayerDollHP.value = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DollHP / maxDollHP[i - 1];
                    PlayerDevilHP.value = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DevilHP / maxDevilHP[i - 1];
                }
                else
                {
                    friendSprite[j].sprite = SetSprite(DataManager.Instance.PlayerDatas[i].roleData.Type);
                    friendNickName[j].text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                    friendDollHP[j].value = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DollHP / maxDollHP[i - 1];
                    friendDevilHP[j].value = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DevilHP / maxDevilHP[i - 1];
                    j++;
                }
            }
        }

        void DisableUI_All()
        {
            PlayerUiObject.SetActive(false);
            foreach(var ui in FriendUiObjects)
            {
                ui.SetActive(false);
            }
        }

        void EnableUI()
        {
            PlayerUiObject.SetActive(true);
            for(int i = 0; i < curFriendCount; ++i)
            {
                FriendUiObjects[i].SetActive(true);
            }
        }

        IEnumerator InitUI()
        {
            while (!DataManager.Instance.IsAllClientInited)
            {
                yield return null;
            }


            myIdx = DataManager.Instance.PlayerIdx;
            for (int i = 1; i <= curDollCount; ++i)
            {
                
                maxDollHP.Add((DataManager.Instance.PlayerDatas[i].roleData as DollData).DollHP);
                maxDevilHP.Add((DataManager.Instance.PlayerDatas[i].roleData as DollData).DevilHP);
            }
            isInited = true;
            yield return true;
        }
    }
}
