using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace GHJ_Lib
{
	public class AttackBox: MonoBehaviour
	{
        protected virtual void OnTriggerStay(Collider other)
        {
			if (other.CompareTag("Doll"))
			{
                DollController doll =  other.GetComponent<DollController>();
                doll.ChangeBvToGetHit();
                //doll.doHit = Hit;
                this.gameObject.SetActive(false);
			}
        }

        public virtual void Hit(DollData targetData)
        {
            targetData.DollHP -= (DataManager.Instance.PlayerDatas[0].roleData as ExorcistData).AttackPower;
            DataManager.Instance.ShareRoleData();
        }
    }
}