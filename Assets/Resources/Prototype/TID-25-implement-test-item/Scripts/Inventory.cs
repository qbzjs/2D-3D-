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
        [Header("Debug Only")]
        [SerializeField] GameObject inventory;

        public void AddToInventory(in GameObject obj, in Transform targetTF)
        {
            inventory = Instantiate( obj, targetTF );
        }
    }
}

