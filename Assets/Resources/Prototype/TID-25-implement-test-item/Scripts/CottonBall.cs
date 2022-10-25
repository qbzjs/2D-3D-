using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class CottonBall : Item
	{
        MeshRenderer mesh;
        protected override void Start()
        {
            base.Start();
            mesh = GetComponent<MeshRenderer>();
        }
        protected override void InitItemData()
        {
            ItemDataLoader.Instance.GetDollItem("CottonBall");
        }
        private void Update()
        {
            if(Input.GetKey(KeyCode.Space))
            {
                DoAction();
            }
        }
        protected override void DoAction()
        {
            ItemManager.Instance.Doll.dollStatus.CottonBall();
            Destroy(this.gameObject);
            
        }
    }
}
