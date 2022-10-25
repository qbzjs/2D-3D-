using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class CottonBall : Item
	{
        public CottonBall()
            :
            base(ItemDataLoader.Instance.GetDollItem("CottonBall"))
        { }
        ItemManager itemManager;
        MeshRenderer mesh;
        private void Start()
        {
            mesh = GetComponent<MeshRenderer>();
            itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
            if(itemManager == null)
            {
                this.gameObject.AddComponent<ItemManager>();
            }

        }
        protected override string Name { get { return "CottonBall"; } }
        protected override void DoAction()
        {
            itemManager.Doll.dollStatus.CottonBall();
            Destroy(this.gameObject);
        }
    }
}
