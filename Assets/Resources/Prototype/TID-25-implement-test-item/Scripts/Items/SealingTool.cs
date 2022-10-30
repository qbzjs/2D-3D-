using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
using Photon;
using Photon.Pun;
using Photon.Realtime;

namespace LSH_Lib
{
	public class SealingTool : Item
	{
        protected override void InitItemData()
        {
            data = DataManager.Instance.ItemInfos[(int)Item.ItemOrder.SealingTool];
        }
        protected override void ActionContent()
        {
            throw new System.NotImplementedException(); 
        }
        //public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
