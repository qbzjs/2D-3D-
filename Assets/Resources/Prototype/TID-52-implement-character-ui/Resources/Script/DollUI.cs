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

        private int MyIdx;
        private bool canUpdate = false;
        #endregion

        #region Private Fields

        private List<Image> friendDollHP=new List<Image>();
        private List<Image> friendDevilHP = new List<Image>();

        private float[] maxDollHP = new float[4];
        private float[] maxDevilHP = new float[4];
        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            friendDollHP.Add(Friend1DollHP);
            friendDollHP.Add(Friend2DollHP);
            friendDollHP.Add(Friend3DollHP);

            friendDevilHP.Add(Friend1DevilHP);
            friendDevilHP.Add(Friend2DevilHP);
            friendDevilHP.Add(Friend3DevilHP);

            StartCoroutine(InitMaxHP());
        }

        private void Update()
        {
            if (!canUpdate)
            {
                return;
            }

            int j = 0;

            for (int i = 0; i < 4; ++i)
            {
                if (MyIdx == i)
                {
                    PlayerDollHP.fillAmount = (DataManager.Instance.PlayerDatas[MyIdx].roleData as DollData).DollHP / maxDollHP[MyIdx];
                    PlayerDevilHP.fillAmount = (DataManager.Instance.PlayerDatas[MyIdx].roleData as DollData).DevilHP / maxDevilHP[MyIdx];
                }
                else
                { 
                    friendDollHP[j].fillAmount = (DataManager.Instance.PlayerDatas[j+1].roleData as DollData).DollHP / maxDollHP[j];
                    friendDevilHP[j].fillAmount = (DataManager.Instance.PlayerDatas[j+1].roleData as DollData).DevilHP / maxDevilHP[j];
                    j++;
                }
            }
           
        }

        IEnumerator InitMaxHP()
        {
            while (true)
            {
                if (DataManager.Instance.PlayerDatas[4].roleData != null)
                { 
                    MyIdx = DataManager.Instance.PlayerIdx - 1;
                    for (int i = 0; i < 4; ++i)
                    {
                        maxDollHP[i] = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DollHP;
                        maxDevilHP[i] = (DataManager.Instance.PlayerDatas[i].roleData as DollData).DevilHP;
                    }
                    canUpdate = true;
                    yield return true;
                }
            }
        }

        #endregion	

        
    }
}
