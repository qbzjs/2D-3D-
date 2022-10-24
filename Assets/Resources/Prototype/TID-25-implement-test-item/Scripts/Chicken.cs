using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class Chicken : Item
	{
        public Chicken() {

        }
		AudioSource chickenaudio;
        MeshRenderer mesh;
        private void Start()
        {
            chickenaudio = this.gameObject.GetComponent<AudioSource>();
            mesh = this.gameObject.GetComponent<MeshRenderer>();
        }
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Exorcist"))
            {
                DoAction();
            }
        }
        public override void DoAction()
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
