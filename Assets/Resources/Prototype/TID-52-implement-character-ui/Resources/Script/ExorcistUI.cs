using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{ 
    public class ExorcistUI : InGameUI
    {
        [Header("UI Objects")]
        public GameObject[] DollUiObjects;

        [Header("Doll1 HP")]
        public Image Doll1HP;
        public Image Devil1HP;
        [Header("Doll2 HP")]
        public Image Doll2HP;
        public Image Devil2HP;
        [Header("Doll3 HP")]
        public Image Doll3HP;
        public Image Devil3HP;
        [Header("Doll4 HP")]
        public Image Doll4HP;
        public Image Devil4HP;

        [Header("Passive Skill")]
        public Image PassiveCoolTime;


        private bool canUpdate = false;
        
        
        //private float[] maxDollHP =new float[4];
        //private float[] maxDevilHP = new float[4];

        private List<Image> dollHP = new List<Image>();
        private List<Image> devilHP = new List<Image>();

        List<float> maxDollHP = new List<float>();
        List<float> maxDevilHP = new List<float>();


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
            if (!canUpdate)
            {
                return;
            }
            for (int i = 1; i < GameManager.Instance.CurPlayerCount; ++i)
            { 
                dollHP[i-1].fillAmount = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DollHP / maxDollHP[i-1];
                devilHP[i-1].fillAmount = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DevilHP / maxDevilHP[i-1];
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
            for(int i = 0; i < GameManager.Instance.CurPlayerCount - 1; ++i)
            {
                DollUiObjects[i].SetActive(true);
            }
        }

        IEnumerator InitUI()
        {
            while (true)
            {
                if (DataManager.Instance.PlayerDatas[GameManager.Instance.CurPlayerCount - 1].roleData != null)
                {
                    for (int i = 1; i < GameManager.Instance.CurPlayerCount; ++i)
                    {
                        if (DataManager.Instance.PlayerDatas[i].roleData is DollData)
                        {
                            maxDollHP[i - 1] = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DollHP;
                            maxDevilHP[i - 1] = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DevilHP;
                        }
                    }
                    canUpdate = true;
                    yield return true;
                }
            }
        }


    }
}
