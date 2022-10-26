using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib
{
	public class CrowFeather : Item
	{
        protected override void InitItemData()
        {
            ItemDataLoader.Instance.GetDollItem("CrowFeatehr");
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
            ItemManager.Instance.Doll.CrowFeather();
            Destroy(this.gameObject);
        }
        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new System.NotImplementedException();
        }
    }
}
