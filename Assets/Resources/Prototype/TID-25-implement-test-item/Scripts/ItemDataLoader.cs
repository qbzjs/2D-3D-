using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using System.IO;
namespace LSH_Lib
{
    [System.Serializable]
    public struct ItemData
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


    public enum ItemOrder
    {
        ConttonBall,
        CottonPiece,
        CrowFeather,
        Oil,
        Bond,
        Chicken,
        Metal,
        Whistle,
        SealingTool,
        BaitPotion,
        Neckless,
        PigeonFeather,
    }

    public class ItemDataLoader : MonoBehaviour
    {
        public static ItemDataLoader Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject obj = new GameObject("_ItemDataLoader");
                    instance = obj.AddComponent<ItemDataLoader>();
                }
                return instance;
            }
        }
        static ItemDataLoader instance;

        const string Path = "Prototype/TID-25-implement-test-item/Data/ItemDataTest";

        public Dictionary<string, ItemData> dollItemDatas = new Dictionary<string, ItemData>();
        public Dictionary<string, ItemData> exorcistItemDatas = new Dictionary<string, ItemData>();

        public void LoadData()
        {
            List<Dictionary<string, object>> data = KSH_Lib.Util.CSVReader.Read(Path);
            for (var i = 0; i < data.Count; i++)
            {
                string type = data[i]["Type"].ToString();
                string name = data[i]["Name"].ToString();
                string number = data[i]["Number"].ToString();
                string isUsing = data[i]["isUsing"].ToString();
                int frequency = int.Parse(data[i]["Frequency"].ToString());
                if (type == "Doll")
                {
                    dollItemDatas.Add(name, new ItemData(type, number, isUsing, frequency));
                }
                else
                {
                    exorcistItemDatas.Add(name, new ItemData(type, number, isUsing, frequency));
                }
            }
        }
        public ItemData GetDollItem(in string name)
        {
            ItemData data;

            if(dollItemDatas == null)
            {
                LoadData();
            }

            dollItemDatas.TryGetValue(name, out data);
            return data;
        }
        public ItemData GetExorcistItem(in string name)
        {
            ItemData data;

            if(exorcistItemDatas == null)
            {
                LoadData();
            }

            exorcistItemDatas.TryGetValue(name, out data);
            return data;
        }
    }
}
