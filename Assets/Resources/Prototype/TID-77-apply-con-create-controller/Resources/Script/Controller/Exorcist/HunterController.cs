using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
namespace GHJ_Lib
{
	public class HunterController: ExorcistController
	{
		public GameObject TrapPrefab;
		public int TrapCount { get; protected set; }

        public override void OnEnable()
        {
            base.OnEnable();
            TrapCount = 5;
            BvActiveSkill = new BvHunterActSkill();
        }

        public override void DoSkill()
        {
            photonView.RPC("DoActiveSkill", RpcTarget.AllViaServer);
        }
    }
}