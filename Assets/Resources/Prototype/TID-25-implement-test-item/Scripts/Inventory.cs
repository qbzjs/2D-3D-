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
        public void AddToInventory(GameObject obj)
        {
            string path = "Prototype/TID-25-implement-test-item/Prefabs/" + obj.name;
            //inventory = obj;
            inventory = PhotonNetwork.Instantiate(path, this.gameObject.transform.position, this.gameObject.transform.rotation, 0, null);
            //inventory.Add(PhotonNetwork.Instantiate(obj.name, this.gameObject.transform.position, this.gameObject.transform.rotation, 0, null));
        }
    }
}

