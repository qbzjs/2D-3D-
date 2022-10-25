using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
namespace LSH_Lib
{
    [System.Serializable]
    public class ItemData
    {
        public ItemData(string type, string number, string isUsing, int frequency)
        {
            this.type = type;
            this.number = number;
            this.isUsing = isUsing;
            this.frequency = frequency;
        }
        public string type, number, isUsing; 
        public int frequency;
    }
    public class ItemBox : MonoBehaviour
	{
        List<ItemData> itemDatas = new List<ItemData>();
        Dictionary<string, ItemData> dollItems = new Dictionary<string, ItemData>();
        Dictionary<string, ItemData> exorcistItems = new Dictionary<string, ItemData>();
        RandomGenerator<string> randomGenerator = new RandomGenerator<string>();
        Item item;
        private void Start()
        {
            DataLoad();
        }
        private void OnTriggerEnter(Collider other)
        {
            string playerTag;
            if(other.gameObject.CompareTag("Exorcist"))
            {
                playerTag = "Exorcist";
                DoAction(playerTag);
            }
            if(other.gameObject.CompareTag("Doll"))
            {
                playerTag = "Doll";
                DoAction(playerTag);
            }
        }

        void DataLoad()
        {
            List<Dictionary<string, object>> data = KSH_Lib.Util.CSVReader.Read("Prototype/TID-25-implement-test-item/Data/ItemData");
            for (var i = 0; i < data.Count; i++)
            {
                string type = data[i]["Type"].ToString();
                string name = data[i]["Name"].ToString();
                string number = data[i]["Number"].ToString();
                string isUsing = data[i]["isUsing"].ToString();
                int frequency = int.Parse(data[i]["Frequency"].ToString());
                if(type == "Doll")
                {
                    dollItems.Add(name, new ItemData(type, number, isUsing, frequency));
                }
                else
                {
                    exorcistItems.Add(name, new ItemData(type, number, isUsing, frequency));
                }
            }
        }
        void DoAction(string playerTag)
        {
            AddItemToList(playerTag);
            RandomPick();
        }
        void AddItemToList(string playerTag)
        {
            if(playerTag == "Doll")
            {
                foreach(string key in dollItems.Keys)
                {
                    randomGenerator.Add(key,10);
                }
            }
            if(playerTag == "Exorcist")
            {
                foreach(string key in exorcistItems.Keys)
                {
                    randomGenerator.Add(key,10);
                }
            }
        }
        void RandomPick()
        {
            string result = randomGenerator.GetItem();
            item = new Item(result);
        }
    }
}
