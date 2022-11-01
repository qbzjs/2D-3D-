using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using KSH_Lib;
using KSH_Lib.Data;

namespace GHJ_Lib
{
	public class BishopAttackBox: AttackBox
	{
		/*--- Public Fields ---*/
		public float attackpower;
		public float skillRate = 0.15f;

		/*--- Protected Fields ---*/


		/*--- Private Fields ---*/


		/*--- MonoBehaviour Callbacks ---*/
		protected override void OnTriggerStay(Collider other)
		{
			if (other.CompareTag("Doll"))
			{
				DollController doll = other.GetComponent<DollController>();
				doll.HitBy();
				doll.doHit = Hit;
				this.gameObject.SetActive(false);
			}
		}


		public override void Hit(DollData targetData)
		{
			ExorcistData exorcistData = (DataManager.Instance.PlayerDatas[0].roleData as ExorcistData);
			targetData.DollHP -= exorcistData.AttackPower;
			targetData.DevilHP -= exorcistData.AttackPower*0.15f;
			DataManager.Instance.ShareRoleData();
		}

		/*--- Public Methods ---*/


		/*--- Protected Methods ---*/


		/*--- Private Methods ---*/
	}
}