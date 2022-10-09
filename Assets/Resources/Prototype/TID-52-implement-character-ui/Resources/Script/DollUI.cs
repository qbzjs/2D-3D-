using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GHJ_Lib
{ 
    public class DollUI : MonoBehaviour
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
        private DollStatus PlayerStatus = null;
        private List<DollStatus> FriendsStatus = new List<DollStatus>();
        private List<Image> FriendDollHP=new List<Image>();
        private List<Image> FriendDevilHP = new List<Image>();
        #endregion

        #region MonoBehaviour CallBacks
        void Start()
        {
            FriendDollHP.Add(Friend1DollHP);
            FriendDollHP.Add(Friend2DollHP);
            FriendDollHP.Add(Friend3DollHP);

            FriendDevilHP.Add(Friend1DevilHP);
            FriendDevilHP.Add(Friend2DevilHP);
            FriendDevilHP.Add(Friend3DevilHP);

        }

        private void Update()
        {
            if (PlayerStatus == null)
            {
                return;
            }
            
            ApplyStatusHPToHPUI(PlayerStatus, PlayerDollHP, PlayerDevilHP);
            for (int i = 0; i < FriendsStatus.Count; ++i)
            { 
                ApplyStatusHPToHPUI(FriendsStatus[i], FriendDollHP[i], FriendDevilHP[i]);
            }


        }
        #endregion	

        #region Public Methods
        public void SetStatus(DollStatus dollStatus)
        {
            this.PlayerStatus = dollStatus;
        }

        public void SetFriendStatus(DollStatus dollStatus)
        {
            FriendsStatus.Add(dollStatus);
        }

        #endregion	

        #region Private Methods
        private void ApplyStatusHPToHPUI(DollStatus dollStatus, Image DollHPImage,Image DevilHPImage)
        {
            if (dollStatus == null)
            {
                Debug.LogError("Missing status");
                return;
            }

            Debug.Log("CurrentHP:" + dollStatus.CurrentRateOfDollHP);
            DollHPImage.fillAmount = dollStatus.CurrentRateOfDollHP;
            DevilHPImage.fillAmount = dollStatus.CurrentRateOfDevilHP; 
        }
        #endregion	
    }
}
