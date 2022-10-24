using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class CottonPiece : Item
	{
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
        public override void DoAction()
        {
            itemManager.Doll.CottonPiece();
            Destroy(this.gameObject);
        }
    }
}
