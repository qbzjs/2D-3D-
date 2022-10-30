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
	public class Bond : Item
	{
        protected override void Start()
        {
            base.Start();
        }
        protected override void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (photonView.IsMine)
                {
                    //gameObject.transform.position = ItemManager.Instance.Doll.transform.position;
                    gameObject.transform.position = GameManager.Instance.DollControllers[DataManager.Instance.PlayerIdx].transform.position;
                }
            }
        }
        protected override void InitItemData()
        {
            data = DataManager.Instance.ItemInfos[(int)Item.ItemOrder.Bond];
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Exorcist"))
            {
                ActionContent();
            }
        }
        protected override void ActionContent()
        {
            StartCoroutine(Slow());
        }
        IEnumerator Slow()
        {
            (DataManager.Instance.LocalPlayerData.roleData as ExorcistData).MoveSpeed *= 0.8f;
            DataManager.Instance.ShareRoleData();
            yield return new WaitForSeconds(20.0f);
            (DataManager.Instance.LocalPlayerData.roleData as ExorcistData).MoveSpeed = DataManager.Instance.RoleInfos[0].MoveSpeed;
            DataManager.Instance.ShareRoleData();
        }
        //public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
