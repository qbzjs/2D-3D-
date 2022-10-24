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
            //������ �����ͼ� �𸶻� �����۸� �о�´�����
            //�� �������� ����ŭ ����Ʈ�� ������
            //�׸��� �������� �ƹ��ų� ����
        }
    }
}
