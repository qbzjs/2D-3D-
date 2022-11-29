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
    public class ExorcistUI : InGameUI
    {
        [Header("UI Objects")]
        public GameObject[] DollUiObjects;

        [Header("Sprites")]
        [SerializeField]
        Sprite[] iconSprites;

        [Header("Doll1 HP")]
        public Image Doll1Image;
        public TextMeshProUGUI Doll1Name;
        public Slider Doll1HP;
        public Slider Devil1HP;

        [Header("Doll2 HP")]
        public Image Doll2Image;
        public TextMeshProUGUI Doll2Name;
        public Slider Doll2HP;
        public Slider Devil2HP;

        [Header("Doll3 HP")]
        public Image Doll3Image;
        public TextMeshProUGUI Doll3Name;
        public Slider Doll3HP;
        public Slider Devil3HP;

        [Header("Doll4 HP")]
        public Image Doll4Image;
        public TextMeshProUGUI Doll4Name;
        public Slider Doll4HP;
        public Slider Devil4HP;

        [Header("Passive Skill")]
        public Image PassiveCoolTime;

        [Header("Active Skill")]
        public InputActionUI CharacterSkill;


        List<Image> dollImage = new List<Image>();
        List<TextMeshProUGUI> dollName = new List<TextMeshProUGUI>();
        List<Slider> dollHP = new List<Slider>();
        List<Slider> devilHP = new List<Slider>();
        List<float> maxDollHP = new List<float>();
        List<float> maxDevilHP = new List<float>();

        bool isInited;


        void Start()
        {
            DisableUI_All();
            EnableUI();

            dollImage.Add(Doll1Image);
            dollImage.Add(Doll2Image);
            dollImage.Add(Doll3Image);
            dollImage.Add(Doll4Image);

            dollName.Add(Doll1Name);
            dollName.Add(Doll2Name);
            dollName.Add(Doll3Name);
            dollName.Add(Doll4Name);

            dollHP.Add(Doll1HP);
            dollHP.Add(Doll2HP);
            dollHP.Add(Doll3HP);
            dollHP.Add(Doll4HP);

            devilHP.Add(Devil1HP);
            devilHP.Add(Devil2HP);
            devilHP.Add(Devil3HP);
            devilHP.Add(Devil4HP);
            for(int i = 1; i<PhotonNetwork.CurrentRoom.PlayerCount-1;++i)
            {
                dollImage[i].sprite = SetSprite(DataManager.Instance.PlayerDatas[i].roleData.Type);
                dollName[i].text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
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
        void Update()
        {
            if(!isInited || DollUiObjects == null)
            {
                return;
            }

            if(!PhotonNetwork.InRoom)
            {
                return;
            }

            for (int i = 1; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
            {
                dollImage[i - 1].sprite = SetSprite(DataManager.Instance.PlayerDatas[i].roleData.Type);
                dollName[i - 1].text = DataManager.Instance.PlayerDatas[i].accountData.Nickname;
                dollHP[i - 1].value = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DollHP / maxDollHP[i - 1];
                devilHP[i - 1].value = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DevilHP / maxDevilHP[i - 1];
            }
        }

        void DisableUI_All()
        {
            foreach(var ui in DollUiObjects)
            {
                ui.SetActive(false);
            }
        }

        void EnableUI()
        {
            for(int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount - 1; ++i)
            {
                DollUiObjects[i].SetActive(true);
            }
        }

        IEnumerator InitUI()
        {
            while( !DataManager.Instance.IsAllClientInited )
            {
                yield return null;
            }

            for (int i = 1; i < PhotonNetwork.CurrentRoom.PlayerCount; ++i)
            {
                maxDollHP.Add((DataManager.Instance.PlayerDatas[i].roleData as DollData).DollHP);
                maxDevilHP.Add((DataManager.Instance.PlayerDatas[i].roleData as DollData).DevilHP);
            }
            isInited = true;
            yield return true;
        }
    }
}
