using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KSH_Lib;
using KSH_Lib.UI;
namespace LSH_Lib
{
	public class ItemInformationController : MonoBehaviour
	{
		[SerializeField]
		GameObject[] Items;
        private void Start()
        {
            for(var i = 0; i<Items.Length; ++i)
            {
                Items[i].SetActive(false);
            }
        }
        void DisableAllText()
        {
            for(var i = 0; i<Items.Length; ++i)
            {
                Items[i].SetActive(false);
            }
        }
        public void EnableItemText(int number)
        {
            DisableAllText();
            Items[number].SetActive(true);
        }
    }
}
