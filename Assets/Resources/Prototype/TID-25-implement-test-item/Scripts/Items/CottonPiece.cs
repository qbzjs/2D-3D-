using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib
{
	public class CottonPiece : Item
	{
        protected override void InitItemData()
        {
            ItemDataLoader.Instance.GetDollItem("CottonPiece");
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
            ItemManager.Instance.Doll.CottonPiece();
            Destroy(this.gameObject);
        }
        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new System.NotImplementedException();
        }
    }
}
