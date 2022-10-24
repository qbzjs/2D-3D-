using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class Whistle : Item
	{
        ItemManager itemManager;
        AudioSource whistleaudio;
        private void Start()
        {
            itemManager = GameObject.Find("ItemManager").GetComponent<ItemManager>();
            whistleaudio = GetComponent<AudioSource>();
        }
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Doll")) 
            {
                DoAction();
            }
        }
        public override void DoAction()
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
