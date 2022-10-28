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
	public class Oil : Item
	{
        protected override void InitItemData()
        {
            throw new System.NotImplementedException();
        }
        protected override void ActionContent()
        {
            throw new System.NotImplementedException();
        }
        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new System.NotImplementedException();
        }
    }
}
