using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class Chicken : Item
	{
        public Chicken() 
            :
            base(ItemDataLoader.Instance.GetDollItem("Chicken"))
        { }
        AudioSource chickenaudio;
        MeshRenderer mesh;
        GameObject ChickenDoll;
        private void Start()
        {
            chickenaudio = this.gameObject.GetComponent<AudioSource>();
            mesh = this.gameObject.GetComponent<MeshRenderer>();
        }
        private void Update()
        {
            KeyDown();
        }
        void KeyDown()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Instantiate(ChickenDoll);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Exocist"))
            {
                DoAction();
            }
        }
        protected override void DoAction()
        {
            chickenaudio.Play();
            mesh.enabled = false;
            StartCoroutine("Audio");
        }
        IEnumerator Audio()
        {
            yield return new WaitForSeconds(3.0f);
            Destroy(this.gameObject);
        }
    }
}
