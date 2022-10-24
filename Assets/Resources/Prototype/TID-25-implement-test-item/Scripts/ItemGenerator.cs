using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class ItemGenerator : MonoBehaviour
	{
		public ItemGenerator(int totalfrequency)
        {
			this.totalFrequency = totalfrequency;
        }

		private int totalFrequency;
		List<Item> test;
        ItemData itemdatas;
        private void Start()
        {
            
            test = new List<Item>(10);
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Exorcist"))
            {
                string playerTag = "Exorcist";
                AddItemToList(playerTag, totalFrequency);
            }
            if(other.gameObject.CompareTag("Doll"))
            {
                string playerTag = "Doll";
                AddItemToList(playerTag,totalFrequency);
            }
        }
        void AddItemToList(string playerTag, int total)
        {
            //아이템 데이터서 퇴마사 아이템만 읽어온다음에
            //각 아이템의 수만큼 리스트에 더해줌
            //그리고 랜덤으로 아무거나 뽑음
        }
    }
}
