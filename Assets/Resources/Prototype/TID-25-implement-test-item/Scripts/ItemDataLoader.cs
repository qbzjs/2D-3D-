using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using System.IO;
namespace LSH_Lib
{


    public class ItemDataLoader : MonoBehaviour
    {
        /*public static ItemDataLoader Instance
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
        */
        //static ItemDataLoader instance;

        //const string Path = "Prototype/TID-25-implement-test-item/Data/ItemData";

        //public Dictionary<string, Item.ItemData> dollItemDatas;
        //public Dictionary<string, Item.ItemData> exorcistItemDatas;

        //public void LoadData()
        //{
        //    List<Dictionary<string, object>> data = KSH_Lib.Util.CSVReader.Read(Path);
        //    for (var i = 0; i < data.Count; i++)
        //    {
        //        string type = data[i]["Type"].ToString();
        //        string name = data[i]["Name"].ToString();
        //        string number = data[i]["Number"].ToString();
        //        string isUsing = data[i]["isUsing"].ToString();
        //        int frequency = int.Parse(data[i]["Frequency"].ToString());
        //        if (type == "Doll")
        //        {
        //            dollItemDatas.Add(name, new ItemData(type, number, isUsing, frequency));
        //        }
        //        else
        //        {
        //            exorcistItemDatas.Add(name, new ItemData(type, number, isUsing, frequency));
        //        }
        //    }
        //}
        //public ItemData GetDollItem(in string name)
        //{
        //    ItemData data;

        //    if(dollItemDatas == null)
        //    {
        //        dollItemDatas = new Dictionary<string, ItemData>();
        //        exorcistItemDatas = new Dictionary<string, ItemData>();
        //        LoadData();
        //    }

        //    if(!dollItemDatas.TryGetValue(name, out data))
        //    {
        //        Debug.LogError($"ItemDataLoader: Can not Find {name} in data table");
        //    }
        //    return data;
        //}
        //public ItemData GetExorcistItem(in string name)
        //{
        //    ItemData data;

        //    if(exorcistItemDatas == null)
        //    {
        //        dollItemDatas = new Dictionary<string, ItemData>();
        //        exorcistItemDatas = new Dictionary<string, ItemData>();
        //        LoadData();
        //    }

        //    if(!exorcistItemDatas.TryGetValue(name, out data))
        //    {
        //        Debug.LogError($"ItemDataLoader: Can not Find {name} in data table");
        //    }
        //    return data;
        //}
    }
}
