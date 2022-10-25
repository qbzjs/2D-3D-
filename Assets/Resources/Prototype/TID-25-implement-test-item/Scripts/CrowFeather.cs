using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class CrowFeather : Item
	{
        protected override void InitItemData()
        {
            ItemDataLoader.Instance.GetDollItem("CrowFeatehr");
        }
        protected override void DoAction()
        {
            ItemManager.Instance.Doll.CrowFeather();
            Destroy(this.gameObject);
        }
    }
}
