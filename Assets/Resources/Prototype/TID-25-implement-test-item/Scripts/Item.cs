using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class Item : MonoBehaviour
	{
        public Item(string ItemName)
        {
            this.itemName = ItemName;
        }
        string itemName;
        virtual protected void OnTriggerEnter(Collider other){}
        virtual protected void DoAction() { }
    }
}
