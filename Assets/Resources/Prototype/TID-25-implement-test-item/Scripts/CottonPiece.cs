using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class CottonPiece : Item
	{
        protected override void InitItemData()
        {
            ItemDataLoader.Instance.GetDollItem("CottonPiece");
        }
        protected override void DoAction()
        {
            ItemManager.Instance.Doll.CottonPiece();
            Destroy(this.gameObject);
        }
    }
}
