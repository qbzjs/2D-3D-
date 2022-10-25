using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class CrowFeather : Item
	{
        public CrowFeather(string itemName)
            :base(itemName)
        { }
        ItemManager itemManager;
        private void Start()
        {
            itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
        }
        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Doll"))
            {
                DoAction();
            }
        }
        void DoAction()
        {
            itemManager.Doll.CrowFeather();
            Destroy(this.gameObject);
        }
    }
}
