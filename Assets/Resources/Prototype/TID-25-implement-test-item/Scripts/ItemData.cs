//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using KSH_Lib;
//using System.IO;
//namespace LSH_Lib
//{
//    public class ItemDatass
//    {
//        public ItemDatass(string types, string names, string explain, string numbers, string isUsings)
//        {
//            type = types;
//            name = names;
//            explains = explain;
//            number = numbers;
//            isUsing = isUsings;
//        }
//        string type, name, explains, number, isUsing;
//    }
//	public class ItemData : MonoBehaviour
//	{
//        List<ItemDatas> datas;
//        private void Start()
//        {
//            GetItemDataFromCSV("Assets/Resources/Prototype/TID-25-implement-test-item/Data/ItemData.csv", out datas);
//        }
//        public bool GetItemDataFromCSV(string csvPath, out List<ItemDatas> itemDatas)
//        {
//            itemDatas = default;
//            List<Dictionary<string, object>> data = Read(csvPath);
//            if (data == null)
//            {
//                return false;
//            }

//            for (int i = 0; i < data.Count; i++)
//            {
//                string type = data[i]["Type"].ToString();
//                string name = data[i]["Name"].ToString();
//                string explains = data[i]["Explains"].ToString();
//                string number =data[i]["Number"].ToString();
//                string isUsing = data[i]["isUsing"].ToString();
//                itemDatas.Add(new ItemDatas(type, name, explains, number, isUsing));
//            }

//            for(int i = 0; i<data.Count; i++)
//            {
//                Debug.Log(itemDatas[i].ToString());
//            }
//            return true;
//        }
//        public List<Dictionary<string, object>> Read(string csvPath)
//        {
//            var list = new List<Dictionary<string, object>>();
//            StreamReader sr = new StreamReader(csvPath, System.Text.Encoding.Default);
//            bool endOfFile = false;
//            while(!endOfFile)
//            {
//                string data_string = sr.ReadLine();
//                if(data_string == null)
//                {
//                    endOfFile = true;
//                    break;
//                }
//                var data_values = data_string.Split(',');
//                var temp = new ItemDatas(data_values[0], data_values[1], data_values[2], data_values[3], data_values[4]);
//                var tempDic = new Dictionary<string, object>();
//                tempDic.Add(data_values[0], temp);
//                list.Add(tempDic);
//            }
//			return list;
//        }
//	}
//}
