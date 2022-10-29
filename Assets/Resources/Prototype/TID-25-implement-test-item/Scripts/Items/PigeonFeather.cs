using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
namespace LSH_Lib
{
	public class PigeonFeather : Item
	{
        protected override void InitItemData()
        {
            data = DataManager.Instance.ItemInfos[(int)Item.ItemOrder.PigeonFeather];
        }
        protected override void ActionContent()
        {
            Debug.Log("PigeonFeather");
        }
    }
}
