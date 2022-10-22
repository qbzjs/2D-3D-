using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class Whistle : Item
	{
        ItemManager itemManager;
        AudioSource audio;
        private void Start()
        {
            itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
            audio = GetComponent<AudioSource>();
        }
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Doll")) 
            {
                DoAction();
            }
        }
        protected override void DoAction()
        {
            audio.Play();
            StartCoroutine("Audio");
        }
        IEnumerator Audio()
        {
            yield return new WaitForSeconds(2.0f);
            Destroy(this.gameObject);
        }
    }
}
