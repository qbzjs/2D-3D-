using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LSH_Lib
{
	public class Chicken : Item
	{
		public AudioSource audio;
        MeshRenderer mesh;
        private void Start()
        {
            mesh = this.gameObject.GetComponent<MeshRenderer>();
        }
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Exorcist"))
            {
                audio.Play();
                mesh.enabled = false;
                StartCoroutine("Audio");
            }
        }
        IEnumerator Audio()
        {
            yield return new WaitForSeconds(3.0f);
            Destroy(this.gameObject);
        }
    }
}
