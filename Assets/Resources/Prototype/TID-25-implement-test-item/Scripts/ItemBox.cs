using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
namespace LSH_Lib
{
    public class ItemBox : MonoBehaviour
    {
        public GameObject[] itemPrefabs;

        RandomGenerator<ItemOrder> randomList = new RandomGenerator<ItemOrder>();

        private void Start()
        {
            randomList.Add(ItemOrder.CottonBall, ItemDataLoader.Instance.GetDollItem("CottonBall").frequency);
            randomList.Add(ItemOrder.Chicken, ItemDataLoader.Instance.GetDollItem("Chicken").frequency);
        }
        private void OnTriggerEnter(Collider other)
        {
            string playerTag;
            if (other.gameObject.CompareTag("Exorcist"))
            {
                playerTag = "Exorcist";
                randomList.GetItem();
            }
            if (other.gameObject.CompareTag("Doll"))
            {
                playerTag = "Doll";
                ItemOrder itemOrder = randomList.GetItem();
                int a = (int)itemOrder;
                other.gameObject.GetComponent<Inventory>().AddToInventory(itemPrefabs[(int)itemOrder]);


            }
        }
    }
}
