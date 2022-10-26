using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib
{
	public class CottonBall : Item
    {
        protected override void Start()
        {
            base.Start();
        }
        protected override void InitItemData()
        {
            ItemDataLoader.Instance.GetDollItem("CottonBall");
        }
        private void Update()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (photonView.IsMine)
                {
                    DoAction();
                }
            }
        }
        protected override void DoAction()
        {
            ItemManager.Instance.Doll.dollStatus.CottonBall();
            Destroy(this.gameObject);
            
        }

        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            
        }
    }
}
