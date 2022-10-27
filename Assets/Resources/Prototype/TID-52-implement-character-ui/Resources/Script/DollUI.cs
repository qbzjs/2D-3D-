using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{ 
    public class DollUI : InGameUI
    {
        #region Public Fields
        [Header("UI Objects")]
        public GameObject PlayerUiObject;
        public GameObject[] FriendUiObjets;


        [Header("PlayerHP")]
        public Image PlayerDollHP;
        public Image PlayerDevilHP;

        [Header("Friend1 HP")]
        public Image Friend1DollHP;
        public Image Friend1DevilHP;
        [Header("Friend2 HP")]
        public Image Friend2DollHP;
        public Image Friend2DevilHP;
        [Header("Friend3 HP")]
        public Image Friend3DollHP;
        public Image Friend3DevilHP;

        #endregion

        #region Private Fields

        private List<Image> friendDollHP=new List<Image>();
        private List<Image> friendDevilHP = new List<Image>();

        private List<float> maxDollHP = new List<float>();
        private List<float> maxDevilHP = new List<float>();

        //private float[] maxDollHP = new float[4];
        //private float[] maxDevilHP = new float[4];

        private int myIdx;
        private bool canUpdate = false;

        int curDollCount;
        int curFriendCount;

        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            curDollCount = GameManager.Instance.CurPlayerCount - 1;
            curFriendCount = curDollCount - 1;

            DisableUI_All();
            EnableUI();

            friendDollHP.Add(Friend1DollHP);
            friendDollHP.Add(Friend2DollHP);
            friendDollHP.Add(Friend3DollHP);

            friendDevilHP.Add(Friend1DevilHP);
            friendDevilHP.Add(Friend2DevilHP);
            friendDevilHP.Add(Friend3DevilHP);
        }

        private void Update()
        {
            if (!canUpdate)
            {
                return;
            }

            int j = 0;

            for (int i = 0; i < curDollCount; ++i)
            {
                if (myIdx == i)
                {
                    PlayerDollHP.fillAmount = (DataManager.Instance.PlayerDatas[myIdx].roleData as DollData).DollHP / maxDollHP[myIdx];
                    PlayerDevilHP.fillAmount = (DataManager.Instance.PlayerDatas[myIdx].roleData as DollData).DevilHP / maxDevilHP[myIdx];
                }
                else
                { 
                    friendDollHP[j].fillAmount = (DataManager.Instance.PlayerDatas[j+1].roleData as DollData).DollHP / maxDollHP[j];
                    friendDevilHP[j].fillAmount = (DataManager.Instance.PlayerDatas[j+1].roleData as DollData).DevilHP / maxDevilHP[j];
                    j++;
                }
            }
           
        }
        public void InitUI()
        {
            myIdx = DataManager.Instance.PlayerIdx - 1;

            for (int i = 1; i <= curDollCount; ++i)
            {
                if (DataManager.Instance.PlayerDatas[i].roleData is DollData)
                {
                    maxDollHP[i - 1] = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DollHP;
                    maxDevilHP[i - 1] = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DevilHP;
                }
            }
            canUpdate = true;
        }


        void DisableUI_All()
        {
            PlayerUiObject.SetActive(false);
            foreach(var ui in FriendUiObjets)
            {
                ui.SetActive(false);
            }
        }

        void EnableUI()
        {
            PlayerUiObject.SetActive(true);
            for(int i = 0; i < curFriendCount; ++i)
            {
                FriendUiObjets[i].SetActive(true);
            }
        }

 

        #endregion	

        
    }
}
