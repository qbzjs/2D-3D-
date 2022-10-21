using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class CottonBall : Item
	{
		ItemManager itemManager;
        MeshRenderer mesh;
        private void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
            if(itemManager == null)
            {
                Debug.LogError("ItemManager is null");
            }
            else
            {
                Debug.Log("ItemManager is not null");
            }
        }
        protected override void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Doll"))
            {
                mesh.enabled = false;
                DoAction();
            }
        }
        protected override void DoAction()
        {
            itemManager.Doll.dollStatus.CottonBall();
            this.gameObject.SetActive(false);
        }
    }
}
