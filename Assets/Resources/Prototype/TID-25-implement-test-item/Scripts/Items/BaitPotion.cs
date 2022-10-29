using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.Data;
using Photon;
using Photon.Pun;
using Photon.Realtime;

namespace LSH_Lib
{
	public class BaitPotion : Item
	{
        
        protected override void InitItemData()
        {
            data = DataManager.Instance.ItemInfos[(int)Item.ItemOrder.BaitPotion];
        }
        protected override void ActionContent()
        {
            StartCoroutine(Bait());
        }
        IEnumerator Bait()
        {
            int i = 0;
            i++;
            yield return new WaitForSeconds(20.0f);
            Debug.Log(i);
        }
        //public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
