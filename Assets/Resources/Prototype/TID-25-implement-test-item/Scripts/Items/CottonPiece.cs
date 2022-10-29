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
	public class CottonPiece : Item
	{
        protected override void InitItemData()
        {
            data = DataManager.Instance.ItemInfos[(int)Item.ItemOrder.CottonPiece];
        }
        protected override void ActionContent()
        {
            (DataManager.Instance.LocalPlayerData.roleData as DollData).DollHP += 25;
            DataManager.Instance.ShareRoleData();
        }
        //public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
