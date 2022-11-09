using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using Photon.Pun;
namespace GHJ_Lib
{
	public class HunterSkill: ExorcistSkill
	{
		public GameObject TrapPrefab;
		public int TrapCount { get; protected set; }

        protected override void OnEnable()
        {
            base.OnEnable();
            TrapCount = 5;
            Controller.AllocSkill(new BvHunterActSkill());
        }


        public override bool CanActiveSkill()
        {
            return true;
        }

        protected override IEnumerator ExcuteActiveSkill()
        {
            throw new System.NotImplementedException();
        }
    }
}