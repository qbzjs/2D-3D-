using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace LSH_Lib
{
	public class DollItemBox : ItemBox
	{
        protected override void InitBox()
        {
            var cottonBallCode = Item.ItemOrder.CottonBall;
            var chickenCode = Item.ItemOrder.Chicken;
            var cottonPieceCode = Item.ItemOrder.CottonPiece;
            var crowFeatherCode = Item.ItemOrder.CrowFeather;
            var whistleCode = Item.ItemOrder.Whistle;
            var bondCode = Item.ItemOrder.Bond;
            var metalCode = Item.ItemOrder.Metal;
            var oilCode = Item.ItemOrder.Oil;

            randomList.Add(cottonBallCode, DataManager.Instance.ItemInfos[(int)cottonBallCode].frequency);
            randomList.Add(chickenCode, DataManager.Instance.ItemInfos[(int)chickenCode].frequency);
            randomList.Add(cottonPieceCode, DataManager.Instance.ItemInfos[(int)cottonPieceCode].frequency);
            randomList.Add(crowFeatherCode, DataManager.Instance.ItemInfos[(int)crowFeatherCode].frequency);
            randomList.Add(whistleCode, DataManager.Instance.ItemInfos[(int)whistleCode].frequency);
            randomList.Add(bondCode, DataManager.Instance.ItemInfos[(int)bondCode].frequency);
            randomList.Add(metalCode, DataManager.Instance.ItemInfos[(int)metalCode].frequency);
            randomList.Add(oilCode, DataManager.Instance.ItemInfos[(int)oilCode].frequency);
        }

        protected override void GetItem(GameObject target)
        {
            if (target.CompareTag("Doll"))
            {
                Item.ItemOrder itemOrder = randomList.GetItem();
                target.GetComponent<Inventory>().AddToInventory(itemPrefabs[(int)itemOrder]);
                Destroy(this.gameObject);
            }
        }
    }
}
