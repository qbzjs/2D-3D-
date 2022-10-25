using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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



        #endregion

        #region Private Fields
        private DollStatus playerStatus = null;
        private List<DollStatus> friendsStatus = new List<DollStatus>();
        private List<Image> friendDollHP=new List<Image>();
        private List<Image> friendDevilHP = new List<Image>();
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

        }

        private void Update()
        {
            
            if (playerStatus == null)
            {
                return;
            }
            
            ApplyStatusHPToHPUI(playerStatus, PlayerDollHP, PlayerDevilHP);
            for (int i = 0; i < friendsStatus.Count; ++i)
            { 
                ApplyStatusHPToHPUI(friendsStatus[i], friendDollHP[i], friendDevilHP[i]);
            }


        }
        #endregion	

        #region Public Methods
        public void SetStatus(DollStatus dollStatus)
        {
            this.playerStatus = dollStatus;
        }

        public void SetFriendStatus(DollStatus dollStatus)
        {
            friendsStatus.Add(dollStatus);
        }

        #endregion	

    }
}
