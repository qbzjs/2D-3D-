using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KSH_Lib;
using KSH_Lib.Data;

using Photon.Pun;

namespace GHJ_Lib
{ 
    public class DollUI : InGameUI
    {
        
        [Header("UI Objects")]
        public GameObject PlayerUiObject;
        public GameObject[] FriendUiObjects;


        [Header("PlayerHP")]
        public Slider PlayerDollHP;
        public Slider PlayerDevilHP;

        [Header("Friend1 HP")]
        public Slider Friend1DollHP;
        public Slider Friend1DevilHP;

        [Header("Friend2 HP")]
        public Slider Friend2DollHP;
        public Slider Friend2DevilHP;

        [Header("Friend3 HP")]
        public Slider Friend3DollHP;
        public Slider Friend3DevilHP;

        [Header("Skill ICon")]
        public InputActionUI CommomSkill;
        public InputActionUI CharacterSkill;

        private List<Slider> friendDollHP = new List<Slider>();
        private List<Slider> friendDevilHP = new List<Slider>();

        private List<float> maxDollHP = new List<float>();
        private List<float> maxDevilHP = new List<float>();

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

            StartCoroutine(InitUI());
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
                    PlayerDollHP.value = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DollHP / maxDollHP[i - 1];
                    PlayerDevilHP.value = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DevilHP / maxDevilHP[i - 1];
                    PlayerDollHP.value = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DollHP / maxDollHP[i - 1];
                    PlayerDevilHP.value = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DevilHP / maxDevilHP[i - 1];
                }
                else
                {
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
