using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class CottonPiece : Item
	{
        public CottonPiece(string itemName)
            :base(itemName)
        { }
		ItemManager itemManager;
        private void Start()
        {
            itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        }
        protected override void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Doll"))
            {
                DoAction();
            }
        }
        protected override void DoAction()
        {
            itemManager.Doll.CottonPiece();
            Destroy(this.gameObject);
        }
    }
}
