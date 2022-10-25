using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib
{
    public class Inventory : MonoBehaviour
    {

        GameObject inventory;
        [PunRPC]
        public void AddToInventory(GameObject obj)
        {
            inventory = PhotonNetwork.Instantiate(obj.name, this.gameObject.transform.position, this.gameObject.transform.rotation, 0, null);
            //inventory.Add(PhotonNetwork.Instantiate(obj.name, this.gameObject.transform.position, this.gameObject.transform.rotation, 0, null));
        }
    }
}

