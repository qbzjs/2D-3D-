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
            //randomList.Add(Item.ItemOrder.CottonBall, ItemDataLoader.Instance.GetDollItem("CottonBall").frequency);
            //randomList.Add(Item.ItemOrder.Chicken, ItemDataLoader.Instance.GetDollItem("Chicken").frequency);
            //randomList.Add(Item.ItemOrder.Chicken, ItemDataLoader.Instance.GetDollItem("CottonPiece").frequency);
            //randomList.Add(Item.ItemOrder.Chicken, ItemDataLoader.Instance.GetDollItem("CrowFeather").frequency);
            //randomList.Add(Item.ItemOrder.Chicken, ItemDataLoader.Instance.GetDollItem("Whistle").frequency);
            InitBox();
        }
        private void OnTriggerEnter(Collider other)
        {
            GetItem(other.gameObject);

            //string playerTag;
            //if (other.gameObject.CompareTag("Exorcist"))
            //{
            //    playerTag = "Exorcist";
            //    randomList.GetItem();
            //}
            //if (other.gameObject.CompareTag("Doll"))
            //{
            //    playerTag = "Doll";
            //    ItemOrder itemOrder = randomList.GetItem();
            //    int a = (int)itemOrder;
            //    other.gameObject.GetComponent<Inventory>().AddToInventory(itemPrefabs[(int)itemOrder]);
            //    Destroy(this.gameObject);
            //}
        }

        protected abstract void InitBox();
        protected abstract void GetItem(GameObject target);
    }

    public class DollItemBox : ItemBox
    {
        protected override void InitBox()
        {
            //randomList.Add(Item.ItemOrder.CottonBall, ItemDataLoader.Instance.GetDollItem("CottonBall").frequency);
            //randomList.Add(Item.ItemOrder.Chicken, ItemDataLoader.Instance.GetDollItem("Chicken").frequency);
            //randomList.Add(Item.ItemOrder.Chicken, ItemDataLoader.Instance.GetDollItem("CottonPiece").frequency);
            //randomList.Add(Item.ItemOrder.Chicken, ItemDataLoader.Instance.GetDollItem("CrowFeather").frequency);
            //randomList.Add(Item.ItemOrder.Chicken, ItemDataLoader.Instance.GetDollItem("Whistle").frequency);

            var cottonBallCode = Item.ItemOrder.CottonBall;
            var chickenCode = Item.ItemOrder.Chicken;
            var cottonPieceCode = Item.ItemOrder.CottonPiece;
            var crowFeatherCode = Item.ItemOrder.CrowFeather;
            var whistleCode = Item.ItemOrder.Whistle;

            randomList.Add(cottonBallCode, DataManager.Instance.ItemInfos[(int)cottonBallCode].frequency);
            randomList.Add(chickenCode, DataManager.Instance.ItemInfos[(int)chickenCode].frequency);
            randomList.Add(cottonPieceCode, DataManager.Instance.ItemInfos[(int)cottonPieceCode].frequency);
            randomList.Add(crowFeatherCode, DataManager.Instance.ItemInfos[(int)crowFeatherCode].frequency);
            randomList.Add(whistleCode, DataManager.Instance.ItemInfos[(int)whistleCode].frequency);
        }

        protected override void GetItem(GameObject target)
        {
            if (target.CompareTag("Doll"))
            {
                Item.ItemOrder itemOrder = randomList.GetItem();
                target.GetComponent<Inventory>().AddToInventory(itemPrefabs[(int)itemOrder]);
                Destroy(this.gameObject);
            }
        }
    }
}
