using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using KSH_Lib;
using KSH_Lib.Data;
namespace LSH_Lib
{
	public class CottonBall : Item
    {
        [SerializeField]
        private int recovery = 40;
        [SerializeField]
        private int reduced = 10;
        protected override void InitItemData()
        {
            data = DataManager.Instance.ItemInfos[(int)Item.ItemOrder.CottonBall];
        }
        protected override void ActionContent()
        {
            (DataManager.Instance.LocalPlayerData.roleData as DollData).DollHP += recovery;
            (DataManager.Instance.LocalPlayerData.roleData as DollData).DevilHP -= reduced;
            DataManager.Instance.ShareRoleData();
        }

        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            
        }
    }
}
