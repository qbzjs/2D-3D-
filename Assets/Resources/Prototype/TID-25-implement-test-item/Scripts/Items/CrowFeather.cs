using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;

using KSH_Lib;
using KSH_Lib.Data;
namespace LSH_Lib
{
	public class CrowFeather : Item
	{
        [SerializeField]
        float addSpeedRatio = 0.15f;
        [SerializeField]
        float effectTime = 30.0f;

        protected override void InitItemData()
        {
           data = DataManager.Instance.ItemInfos[(int)Item.ItemOrder.CrowFeather];
        }
        protected override void ActionContent()
        {
            //ItemManager.Instance.Doll.CrowFeather();

            StartCoroutine(Effect());
        }

        IEnumerator Effect()
        {
            (DataManager.Instance.LocalPlayerData.roleData).MoveSpeed *= addSpeedRatio;
            DataManager.Instance.ShareRoleData();

            yield return new WaitForSeconds(effectTime);

            var type = DataManager.Instance.LocalPlayerData.roleData.TypeOrder;
            (DataManager.Instance.LocalPlayerData.roleData).MoveSpeed = DataManager.Instance.RoleInfos[(int)type].MoveSpeed;
            DataManager.Instance.ShareRoleData();
        }

        public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new System.NotImplementedException();
        }
    }
}
