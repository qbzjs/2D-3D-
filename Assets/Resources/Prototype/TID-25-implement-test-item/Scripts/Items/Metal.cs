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
	public class Metal : Item
	{
        protected override void InitItemData()
        {
            data = DataManager.Instance.ItemInfos[(int)Item.ItemOrder.Metal];
        }
        protected override void ActionContent()
        {
            (DataManager.Instance.LocalPlayerData.roleData as DollData).MoveSpeed *= 1.20f;
            if(GameManager.Instance.DollControllers[DataManager.Instance.PlayerIdx].CurCharacterAction.ToString() == "caught")
            {
                (DataManager.Instance.LocalPlayerData.roleData as ExorcistData).MoveSpeed *= 0.75f;
            }
        }
        //public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
