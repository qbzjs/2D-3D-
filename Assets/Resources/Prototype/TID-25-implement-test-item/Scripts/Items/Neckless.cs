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
	public class Neckless : Item
	{
        protected override void InitItemData()
        {
            data = DataManager.Instance.ItemInfos[(int)Item.ItemOrder.Neckless];
        }
        protected override void ActionContent()
        {
            Debug.Log("Neckless");
        }
        //public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
