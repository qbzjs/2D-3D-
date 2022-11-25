using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
namespace LSH_Lib
{
    public abstract class ItemBox : MonoBehaviour
    {
        [SerializeField]
        protected GameObject[] itemPrefabs;

        protected RandomGenerator<Item.ItemOrder> randomList = new RandomGenerator<Item.ItemOrder>();


        protected virtual void Start()
        {
            InitBox();
        }
        private void OnTriggerEnter(Collider other)
        {
            GetItem(other.gameObject);
        }

        protected abstract void InitBox();
        protected abstract void GetItem(GameObject target);
    }
}
