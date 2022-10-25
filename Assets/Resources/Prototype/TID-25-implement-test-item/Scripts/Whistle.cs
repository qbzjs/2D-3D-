using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class Whistle : Item
	{
        public Whistle(string itemName)
            :base(itemName)
        { }
        ItemManager itemManager;
        AudioSource whistleaudio;
        private void Start()
        {
            itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
            whistleaudio = GetComponent<AudioSource>();
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Doll")) 
            {
                DoAction();
            }
        }
        void DoAction()
        {
            whistleaudio.Play();
            StartCoroutine("Audio");
        }
        IEnumerator Audio()
        {
            yield return new WaitForSeconds(2.0f);
            Destroy(this.gameObject);
        }
    }
}
