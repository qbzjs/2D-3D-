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
        public ItemData(string types, string names,string numbers, string isUsings, int frequency)
        {
            type = types;
            name = names;
            number = numbers;
            isUsing = isUsings;
            this.frequency = frequency;
        }
        public string type, name, number, isUsing; 
        public int frequency;
    }
    public class ItemBox : MonoBehaviour
	{
        List<ItemData> itemDatas = new List<ItemData>();
        List<ItemData> itemTypelist = new List<ItemData>();
        List<Item> items = new List<Item>();

        private void Start()
        {
            DataLoad();
            AddItemToList("Doll");
        }
        private void Update()
        {
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Exorcist"))
            {
                string playerTag = "Exorcist";
                AddItemToList(playerTag);
            }
            if(other.gameObject.CompareTag("Doll"))
            {
                string playerTag = "Doll";
                AddItemToList(playerTag);
            }
        }

        void DataLoad()
        {
            List<Dictionary<string, object>> data = KSH_Lib.Util.CSVReader.Read("Prototype/TID-25-implement-test-item/Data/ItemData");
            for (var i = 0; i < data.Count; i++)
            {
                string type = data[i]["Type"].ToString();
                string name = data[i]["Name"].ToString();
                string Number = data[i]["Number"].ToString();
                string isUsing = data[i]["isUsing"].ToString();
                int frequency = int.Parse(data[i]["Frequency"].ToString());
                itemDatas.Add(new ItemData(type, name, Number, isUsing, frequency));
            }
        }
        void AddItemToList(string playerTag)
        {
            FindType(playerTag);
            Addtolist();
        }
        void Addtolist()
        {
            
        }
        void FindType(string playerTag)
        {
            
            for(var i = 0; i< itemDatas.Count; i++)
            {
                if (itemDatas[i].type == playerTag)
                {
                    itemTypelist.Add(itemDatas[i]);
                }
                else
                    break;
            }
        }
    }
}
