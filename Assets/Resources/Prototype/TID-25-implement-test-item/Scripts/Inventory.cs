using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
namespace LSH_Lib
{
    public class Inventory : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        static string ItemPath = "Prototype/TID-25-implement-test-item/Prefabs/Items/";
        GameObject inventory;

        public void AddToInventory(GameObject obj)
        {
            string path = ItemPath + obj.name;
           inventory = PhotonNetwork.Instantiate(path, this.gameObject.transform.position, Quaternion.identity, 0);
        }



        ////[PunRPC]
        //public void AddToInventory(GameObject obj)
        //{
        //    string path = ItemPath + obj.name;
        //    //inventory = obj;
        //    inventory = PhotonNetwork.Instantiate(path, this.gameObject.transform.position, Quaternion.identity, 0);
        //    //inventory.Add(PhotonNetwork.Instantiate(obj.name, this.gameObject.transform.position, this.gameObject.transform.rotation, 0, null));
        //}
    }
}

