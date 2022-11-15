using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KSH_Lib;
using KSH_Lib.Data;

using Photon.Pun;

namespace GHJ_Lib
{ 
    public class ExorcistUI : InGameUI
    {
        [Header("UI Objects")]
        public GameObject[] DollUiObjects;

        [Header("Doll1 HP")]
        public Slider Doll1HP;
        public Slider Devil1HP;

        [Header("Doll2 HP")]
        public Slider Doll2HP;
        public Slider Devil2HP;

        [Header("Doll3 HP")]
        public Slider Doll3HP;
        public Slider Devil3HP;

        [Header("Doll4 HP")]
        public Slider Doll4HP;
        public Slider Devil4HP;

        [Header("Passive Skill")]
        public Image PassiveCoolTime;

        [Header("Active Skill")]
        public InputActionUI CharacterSkill;

        List<Slider> dollHP = new List<Slider>();
        List<Slider> devilHP = new List<Slider>();
        List<float> maxDollHP = new List<float>();
        List<float> maxDevilHP = new List<float>();

        bool isInited;


        void Start()
        {
            DisableUI_All();
            EnableUI();

            dollHP.Add(Doll1HP);
            dollHP.Add(Doll2HP);
            dollHP.Add(Doll3HP);
            dollHP.Add(Doll4HP);

            devilHP.Add(Devil1HP);
            devilHP.Add(Devil2HP);
            devilHP.Add(Devil3HP);
            devilHP.Add(Devil4HP);

            StartCoroutine(InitUI());
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
