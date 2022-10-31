using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace LSH_Lib
{
	public class ExorcistItemBox : ItemBox
	{
        protected override void InitBox()
        {
            var sealingTool = Item.ItemOrder.SealingTool;
            var baitPotion = Item.ItemOrder.BaitPotion;
            var neckless = Item.ItemOrder.Neckless;
            var pigeonFeather = Item.ItemOrder.PigeonFeather;

            randomList.Add(sealingTool, DataManager.Instance.ItemInfos[(int)sealingTool].frequency);
            randomList.Add(baitPotion, DataManager.Instance.ItemInfos[(int)baitPotion].frequency);
            randomList.Add(neckless, DataManager.Instance.ItemInfos[(int)neckless].frequency);
            randomList.Add(pigeonFeather, DataManager.Instance.ItemInfos[(int)pigeonFeather].frequency);
        }
        protected override void GetItem(GameObject target)
        {
            if(target.CompareTag("Exorcist"))
            {
                Item.ItemOrder itemOrder = randomList.GetItem();
                target.GetComponent<Inventory>().AddToInventory(itemPrefabs[(int)itemOrder]);
                Destroy(this.gameObject);
            }
        }
    }
}