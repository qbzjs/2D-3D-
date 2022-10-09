using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GHJ_Lib
{ 
    public class ExorcistUI : InGameUI
    {
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

        private List<DollStatus> dollsStatus = new List<DollStatus>();
        private List<Image> dollHP = new List<Image>();
        private List<Image> devilHP = new List<Image>();

        void Start()
        {
            dollHP.Add(Doll1HP);
            dollHP.Add(Doll2HP);
            dollHP.Add(Doll3HP);
            dollHP.Add(Doll4HP);

            devilHP.Add(Devil1HP);
            devilHP.Add(Devil2HP);
            devilHP.Add(Devil3HP);
            devilHP.Add(Devil4HP);
        }

        void Update()
        {
            if (dollsStatus[0] == null)
            {
                return;
            }

            for (int i = 0; i < dollsStatus.Count; ++i)
            {
                ApplyStatusHPToHPUI(dollsStatus[i], dollHP[i], devilHP[i]);
            }
        }

        public void SetDollStatus(DollStatus dollStatus)
        {
            dollsStatus.Add(dollStatus);
        }
    }
}
